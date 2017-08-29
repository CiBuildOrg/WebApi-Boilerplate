using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using App.Infrastructure.Utils.Multipart.Converters;
using App.Infrastructure.Utils.Multipart.Infrastructure.Logger;

namespace App.Infrastructure.Utils.Multipart
{
    public class FormMultipartEncodedMediaTypeFormatter : MediaTypeFormatter
    {
        private const string SupportedMediaType = "multipart/form-data";

        public FormMultipartEncodedMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);

            if (headers.ContentType == null)
                headers.ContentType = new MediaTypeHeaderValue(SupportedMediaType);

            if (!string.Equals(headers.ContentType.MediaType, SupportedMediaType, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Not a Multipart Content");

            if (headers.ContentType.Parameters.All(m => m.Name != "boundary"))
                headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "MultipartDataMediaFormatterBoundary1q2w3e"));
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
                                                               IFormatterLogger formatterLogger)
        {
            var httpContentToFormDataConverter = new HttpContentToFormDataConverter();
            var multipartFormData = await httpContentToFormDataConverter.Convert(content);

            IFormDataConverterLogger logger;
            if (formatterLogger != null)
                logger = new FormatterLoggerAdapter(formatterLogger);
            else
                logger = new FormDataConverterLogger();

            var dataToObjectConverter = new FormDataToObjectConverter(multipartFormData, logger);
            var result = dataToObjectConverter.Convert(type);

            logger.EnsureNoErrors();

            return result;
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
                                                TransportContext transportContext)
        {
            if (!content.IsMimeMultipartContent())
                throw new Exception("Not a Multipart Content");

            var boudaryParameter = content.Headers.ContentType.Parameters.FirstOrDefault(m => m.Name == "boundary" && !string.IsNullOrWhiteSpace(m.Value));
            if (boudaryParameter == null)
                throw new Exception("multipart boundary not found");

            var objectToMultipartDataByteArrayConverter = new ObjectToMultipartDataByteArrayConverter();
            var multipartData = objectToMultipartDataByteArrayConverter.Convert(value, boudaryParameter.Value);

            await writeStream.WriteAsync(multipartData, 0, multipartData.Length);

            content.Headers.ContentLength = multipartData.Length;
        }
    }
}