using System.IO;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace App.Services.Resources
{
    public class JsonPrettifier
    {
        private const string ReplateToken = "{JSON_HERE}";
        private const string Filename = "json.html";

        public static string BeautifyJson(string json)
        {
            var jsonFormatted = JToken.Parse(json).ToString(Formatting.Indented);
            var formatFile = GetEmbeddedJsonTemplate(Filename);
            return formatFile.Replace(ReplateToken, jsonFormatted);
        }


        private static string GetEmbeddedJsonTemplate(string resource)
        {
            using (var stream = EmbeddedResources.ThisResources.GetStream(resource))
            {
                using (var sReader = new StreamReader(stream))
                {
                    var content = sReader.ReadToEnd();
                    return content;
                }
            }
        }
    }
}