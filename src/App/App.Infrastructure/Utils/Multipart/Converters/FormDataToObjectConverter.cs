using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using App.Dto.Request;
using App.Infrastructure.Extensions;
using App.Infrastructure.Utils.Multipart.Infrastructure.Logger;

namespace App.Infrastructure.Utils.Multipart.Converters
{
    public class FormDataToObjectConverter
    {
        private readonly FormData _sourceData;
        private readonly IFormDataConverterLogger _logger;

        public FormDataToObjectConverter(FormData sourceData, IFormDataConverterLogger logger) 
        {
            _sourceData = sourceData ?? throw new ArgumentNullException(nameof(sourceData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public object Convert(Type destinitionType) 
        {
            if (destinitionType == null)
                throw new ArgumentNullException(nameof(destinitionType));

            if (destinitionType == typeof(FormData))
                return _sourceData;

            var objResult = CreateObject(destinitionType);
            return objResult;
        } 

        private object CreateObject(Type destinitionType, string propertyName = "")
        {
            object propValue = null;


            if (TryGetFromFormData(destinitionType, propertyName, out object buf)
                || TryGetAsGenericDictionary(destinitionType, propertyName, out buf)
                || TryGetAsGenericListOrArray(destinitionType, propertyName, out buf)
                || TryGetAsCustomType(destinitionType, propertyName, out buf))
            {
                propValue = buf;
            }
            else if (!IsFileOrConvertableFromString(destinitionType))
            {
                _logger.LogError(propertyName, $"Cannot parse type \"{destinitionType.FullName}\".");
            }

            return propValue;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private bool TryGetFromFormData(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;

            if (destinitionType == typeof(HttpFile))
            {
                if (!_sourceData.TryGetValue(propertyName, out HttpFile httpFile))
                    return false;

                propValue = httpFile;
            }
            else
            {
                if (!_sourceData.TryGetValue(propertyName, out string val))
                {
                    return false;
                }

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
                    catch (Exception ex)
                    {
                        _logger.LogError(propertyName, $"Error parsing field \"{propertyName}\": {ex.Message}");
                    }
                }
            }

            return true;
        }

        private bool TryGetAsGenericDictionary(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            var isGenericDictionary = IsGenericDictionary(destinitionType, out Type keyType, out Type valueType);
            if (!isGenericDictionary)
            {
                return false;
            }

            var dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var add = dictType.GetMethod("Add");

            var pValue = Activator.CreateInstance(dictType);

            var index = 0;
            var origPropName = propertyName;
            var isFilled = false;
            while (true)
            {
                var propertyKeyName = $"{origPropName}[{index}].Key";
                var objKey = CreateObject(keyType, propertyKeyName);
                if (objKey != null)
                {
                    var propertyValueName = $"{origPropName}[{index}].Value";
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

            return true;
        }

        private bool TryGetAsGenericListOrArray(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            var isGenericList = IsGenericListOrArray(destinitionType, out Type genericListItemType);
            if (!isGenericList)
            {
                return false;
            }

            var listType = typeof(List<>).MakeGenericType(genericListItemType);

            var add = listType.GetMethod("Add");
            var pValue = Activator.CreateInstance(listType);

            var index = 0;
            var origPropName = propertyName;
            var isFilled = false;
            while (true)
            {
                propertyName = $"{origPropName}[{index}]";
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

            if (!isFilled)
            {
                return true;
            }

            if (destinitionType.IsArray)
            {
                var toArrayMethod = listType.GetMethod("ToArray");
                propValue = toArrayMethod.Invoke(pValue, new object[0]);
            }
            else
            {
                propValue = pValue;
            }

            return true;
        }

        private bool TryGetAsCustomType(Type destinitionType, string propertyName, out object propValue)
        {
            propValue = null;
            var isCustomNonEnumerableType = destinitionType.IsCustomNonEnumerableType();

            if (!isCustomNonEnumerableType)
            {
                return false;
            }

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
                if (objValue == null)
                {
                    continue;
                }

                propertyInfo.SetValue(obj, objValue);
                isFilled = true;
            }
            if (isFilled)
            {
                propValue = obj;
            }
            return true;
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
