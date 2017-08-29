using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using App.Dto.Request;
using App.Infrastructure.Utils.Multipart.Infrastructure;

namespace App.Infrastructure.Utils.Multipart.Converters
{
    public class HttpContentToFormDataConverter
    {
        public async Task<FormData> Convert(HttpContent content)
        {
            if(content == null)
                throw new ArgumentNullException(nameof(content));
            var multipartProvider = await content.ReadAsMultipartAsync();

            var multipartFormData = await Convert(multipartProvider);
            return multipartFormData;
        }

        public async Task<FormData> Convert(MultipartMemoryStreamProvider multipartProvider)
        {
            var multipartFormData = new FormData();

            foreach (var file in multipartProvider.Contents.Where(x => IsFile(x.Headers.ContentDisposition)))
            {
                var name = UnquoteToken(file.Headers.ContentDisposition.Name);
                var fileName = FixFilename(file.Headers.ContentDisposition.FileName);
                var mediaType = file.Headers.ContentType.MediaType;

                using (var stream = await file.ReadAsStreamAsync())
                {
                    var buffer = ReadAllBytes(stream);
                    if (buffer.Length > 0)
                    {
                        multipartFormData.Add(name, new HttpFile(fileName, mediaType, buffer));
                    }
                }
            }

            foreach (var part in multipartProvider.Contents.Where(x => x.Headers.ContentDisposition.DispositionType == "form-data"
                                                                  && !IsFile(x.Headers.ContentDisposition)))
            {
                var name = UnquoteToken(part.Headers.ContentDisposition.Name);
                var data = await part.ReadAsStringAsync();
                multipartFormData.Add(name, data);
            }

            return multipartFormData;
        } 

        private static bool IsFile(ContentDispositionHeaderValue disposition)
        {
            return !string.IsNullOrEmpty(disposition.FileName);
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        private static string UnquoteToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

        /// <summary>
        /// Amend filenames to remove surrounding quotes and remove path from IE
        /// </summary>
        private static string FixFilename(string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(originalFileName))
                return string.Empty;

            var result = originalFileName.Trim();
            
            // remove leading and trailing quotes
            result = result.Trim('"');

            // remove full path versions
            if (result.Contains("\\"))
                result = Path.GetFileName(result);

            return result;
        }

        private static byte[] ReadAllBytes(Stream input)
        {
            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}
