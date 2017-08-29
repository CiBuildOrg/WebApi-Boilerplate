using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Dto.Request;
using App.Infrastructure.Utils.Multipart.Infrastructure;
using App.Infrastructure.Utils.Multipart.Infrastructure.Extensions;
using App.Infrastructure.Utils.Multipart.Infrastructure.Logger;

namespace App.Infrastructure.Utils.Multipart.Converters
{
    public class FormDataToObjectConverter
    {
        private readonly FormData _sourceData;
        private readonly IFormDataConverterLogger _logger;

        public FormDataToObjectConverter(FormData sourceData, IFormDataConverterLogger logger) 
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");
            if (logger == null)
                throw new ArgumentNullException("logger");

            _sourceData = sourceData;
            _logger = logger;
        }

        public object Convert(Type destinitionType) 
        {
            if (destinitionType == null)
                throw new ArgumentNullException("destinitionType");

            if (destinitionType == typeof(FormData))
                return _sourceData;

            var objResult = CreateObject(destinitionType);
            return objResult;
        } 

        private object CreateObject(Type destinitionType, string propertyName = "")
        {
            object propValue = null;
            
            object buf;

            if (TryGetFromFormData(destinitionType, propertyName, out buf)
                || TryGetAsGenericDictionary(destinitionType, propertyName, out buf)
                || TryGetAsGenericListOrArray(destinitionType, propertyName, out buf)
                || TryGetAsCustomType(destinitionType, propertyName, out buf))
            {
                propValue = buf;
            }
            else if (!IsFileOrConvertableFromString(destinitionType))
            {
                _logger.LogError(propertyName, string.Format("Cannot parse type \"{0}\".", destinitionType.FullName));
            }
            
            return propValue;
        }

        private bool TryGetFromFormData(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;

            if (destinitionType == typeof(HttpFile))
            {
                HttpFile httpFile;
                if (!_sourceData.TryGetValue(propertyName, out httpFile)) 
                    return false;
                
                propValue = httpFile;
            }
            else
            {
                string val;
                if (!_sourceData.TryGetValue(propertyName, out val)) 
                    return false;
                
                var typeConverter = destinitionType.GetFromStringConverter();
                if (typeConverter == null)
                {
                    _logger.LogError(propertyName, "Cannot find type converter for field - " + propertyName);
                }
                else
                {
                    try
                    {
                        propValue = typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, val);
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(propertyName, string.Format("Error parsing field \"{0}\": {1}", propertyName, ex.Message));
                    }
                }
            }

            return true;
        }

        private bool TryGetAsGenericDictionary(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            Type keyType, valueType;
            var isGenericDictionary = IsGenericDictionary(destinitionType, out keyType, out valueType);
            if (isGenericDictionary)
            {
                var dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                var add = dictType.GetMethod("Add");

                var pValue = Activator.CreateInstance(dictType);

                var index = 0;
                var origPropName = propertyName;
                var isFilled = false;
                while (true)
                {
                    var propertyKeyName = string.Format("{0}[{1}].Key", origPropName, index);
                    var objKey = CreateObject(keyType, propertyKeyName);
                    if (objKey != null)
                    {
                        var propertyValueName = string.Format("{0}[{1}].Value", origPropName, index);
                        var objValue = CreateObject(valueType, propertyValueName);

                        if (objValue != null)
                        {
                            add.Invoke(pValue, new[] { objKey, objValue });
                            isFilled = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                    index++;
                }

                if (isFilled)
                {
                    propValue = pValue;
                }
            }

            return isGenericDictionary;
        }

        private bool TryGetAsGenericListOrArray(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            Type genericListItemType;
            var isGenericList = IsGenericListOrArray(destinitionType, out genericListItemType);
            if (isGenericList)
            {
                var listType = typeof(List<>).MakeGenericType(genericListItemType);

                var add = listType.GetMethod("Add");
                var pValue = Activator.CreateInstance(listType);

                var index = 0;
                var origPropName = propertyName;
                var isFilled = false;
                while (true)
                {
                    propertyName = string.Format("{0}[{1}]", origPropName, index);
                    var objValue = CreateObject(genericListItemType, propertyName);
                    if (objValue != null)
                    {
                        add.Invoke(pValue, new[] { objValue });
                        isFilled = true;
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                if (isFilled)
                {
                    if (destinitionType.IsArray)
                    {
                        var toArrayMethod = listType.GetMethod("ToArray");
                        propValue = toArrayMethod.Invoke(pValue, new object[0]);
                    }
                    else
                    {
                        propValue = pValue;
                    }
                }
            }

            return isGenericList;
        }

        private bool TryGetAsCustomType(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            var isCustomNonEnumerableType = destinitionType.IsCustomNonEnumerableType();
            
            if (isCustomNonEnumerableType)
            {
                if (!string.IsNullOrWhiteSpace(propertyName) &&
                    !_sourceData.AllKeys()
                        .Any(m => m.StartsWith(propertyName + ".", StringComparison.CurrentCultureIgnoreCase)))
                    return true;

                var obj = Activator.CreateInstance(destinitionType);
                
                var isFilled = false;
                foreach (var propertyInfo in destinitionType.GetPublicAccessibleProperties())
                {
                    var propName = (!string.IsNullOrEmpty(propertyName) ? propertyName + "." : "") + propertyInfo.Name;
                    var objValue = CreateObject(propertyInfo.PropertyType, propName);
                    if (objValue != null)
                    {
                        propertyInfo.SetValue(obj, objValue);
                        isFilled = true;
                    }
                }
                if (isFilled)
                {
                    propValue = obj;
                }
            }
            return isCustomNonEnumerableType;
        }

        private static bool IsGenericDictionary(Type type, out Type keyType, out Type valueType)
        {
            var iDictType = type.GetInterface(typeof (IDictionary<,>).Name);
            if (iDictType != null)
            {
                var types = iDictType.GetGenericArguments();
                if (types.Length == 2)
                {
                    keyType = types[0];
                    valueType = types[1];
                    return true;
                }
            }

            keyType = null;
            valueType = null;
            return false;
        }

        private static bool IsGenericListOrArray(Type type, out Type itemType)
        {
            if (type.GetInterface(typeof(IDictionary<,>).Name) == null) //not a dictionary
            {
                if (type.IsArray)
                {
                    itemType = type.GetElementType();
                    return true;
                }

                var iListType = type.GetInterface(typeof(ICollection<>).Name);
                if (iListType != null) 
                {
                    var genericArguments = iListType.GetGenericArguments();
                    if (genericArguments.Length == 1)
                    {
                        itemType = genericArguments[0];
                        return true;
                    }
                }
            }
          
            itemType = null;
            return false;
        }

        private static bool IsFileOrConvertableFromString(Type type)
        {
            if (type == typeof (HttpFile))
                return true;

            return type.GetFromStringConverter() != null;
        }
    }
}
