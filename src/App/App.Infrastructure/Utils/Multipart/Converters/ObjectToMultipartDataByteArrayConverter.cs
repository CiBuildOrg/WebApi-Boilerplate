using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using App.Dto.Request;
using App.Infrastructure.Utils.Multipart.Infrastructure;
using App.Infrastructure.Utils.Multipart.Infrastructure.Extensions;

namespace App.Infrastructure.Utils.Multipart.Converters
{
    public class ObjectToMultipartDataByteArrayConverter
    {
        public byte[] Convert(object value, string boundary)
        {
            if(value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrWhiteSpace(boundary))
                throw new ArgumentNullException("boundary");

            var propertiesList = ConvertObjectToFlatPropertiesList(value);
            var buffer = GetMultipartFormDataBytes(propertiesList, boundary);
            return buffer;
        }

        private List<KeyValuePair<string, object>> ConvertObjectToFlatPropertiesList(object value)
        {
            var propertiesList = new List<KeyValuePair<string, object>>();
            if (value is FormData)
            {
                FillFlatPropertiesListFromFormData((FormData) value, propertiesList);
            }
            else
            {
                FillFlatPropertiesListFromObject(value, "", propertiesList);   
            }

            return propertiesList;
        }

        private static void FillFlatPropertiesListFromFormData(FormData formData,
            List<KeyValuePair<string, object>> propertiesList)
        {
            propertiesList.AddRange(
                formData.Fields.Select(field => new KeyValuePair<string, object>(field.Name, field.Value)));

            propertiesList.AddRange(
                formData.Files.Select(field => new KeyValuePair<string, object>(field.Name, field.Value)));
        }

        private void FillFlatPropertiesListFromObject(object obj, string prefix, List<KeyValuePair<string, object>> propertiesList)
        {
            if (obj == null) return;
            var type = obj.GetType();

            if (obj is IDictionary)
            {
                var dict = obj as IDictionary;
                var index = 0;
                foreach (var key in dict.Keys)
                {
                    var indexedKeyPropName = string.Format("{0}[{1}].Key", prefix, index);
                    FillFlatPropertiesListFromObject(key, indexedKeyPropName, propertiesList);

                    var indexedValuePropName = string.Format("{0}[{1}].Value", prefix, index);
                    FillFlatPropertiesListFromObject(dict[key], indexedValuePropName, propertiesList);

                    index++;
                }
            }
            else
            {
                var collection = obj as ICollection;
                if (collection != null)
                {
                    var list = collection;
                    var index = 0;
                    foreach (var indexedPropValue in list)
                    {
                        var indexedPropName = string.Format("{0}[{1}]", prefix, index);
                        FillFlatPropertiesListFromObject(indexedPropValue, indexedPropName, propertiesList);

                        index++;
                    }
                }
                else if (type.IsCustomNonEnumerableType())
                {
                    foreach (var propertyInfo in type.GetPublicAccessibleProperties())
                    {
                        var propName = string.IsNullOrWhiteSpace(prefix)
                            ? propertyInfo.Name
                            : string.Format("{0}.{1}", prefix, propertyInfo.Name);

                        var propValue = propertyInfo.GetValue(obj);
                        
                        FillFlatPropertiesListFromObject(propValue, propName, propertiesList);
                    }
                }
                else
                {
                    propertiesList.Add(new KeyValuePair<string, object>(prefix, obj));
                }
            }
        }

        private static byte[] GetMultipartFormDataBytes(List<KeyValuePair<string, object>> postParameters, string boundary)
        {
            var encoding = Encoding.UTF8;

            using (var formDataStream = new MemoryStream())
            {
                var needsClrf = false;

                foreach (var param in postParameters)
                {
                    // Add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsClrf)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsClrf = true;

                    var file = param.Value as HttpFile;
                    if (file != null)
                    {
                        var httpFileToUpload = file;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        var header =
                            string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                                boundary,
                                param.Key,
                                httpFileToUpload.FileName ?? param.Key,
                                httpFileToUpload.MediaType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(httpFileToUpload.Buffer, 0, httpFileToUpload.Buffer.Length);
                    }
                    else
                    {
                        var objString = "";
                        if (param.Value != null)
                        {
                            var typeConverter = param.Value.GetType().GetToStringConverter();
                            if (typeConverter != null)
                            {
                                objString = typeConverter.ConvertToString(null, CultureInfo.CurrentCulture, param.Value);
                            }
                            else
                            {
                                throw new System.Exception(string.Format("Type \"{0}\" cannot be converted to string", param.Value.GetType().FullName));
                            }
                        }

                        var postData =
                            string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                                          boundary,
                                          param.Key,
                                          objString);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                var footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                var formData = formDataStream.ToArray();

                return formData;
            }
        }
    }
}
