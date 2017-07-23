using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace App.Infrastructure.Tracing.Resources
{
    public class XmlPrettifyHelper
    {
        private const string 
            Filename = "xml.xslt";

        public static string Prettify(string xml)
        {
            var xmlDoc = new XmlDocument();
            var xsl = new System.Xml.Xsl.XslCompiledTransform();
            var enc = new ASCIIEncoding();
            var writer = new StringWriter();

            // Get Xsl
            //xsl.Load(HttpContext.Current.Server.MapPath("defaultss.xslt"));
            xsl.Load(GetReader());

            // Remove the utf encoding as it causes problems with the XPathDocument
            xmlDoc.LoadXml(xml);

            // Get the bytes
            Stream xmlStream = new MemoryStream(enc.GetBytes(xmlDoc.OuterXml), true);
            // Load Xpath document
            var xp = new XPathDocument(xmlStream);

            // Perform Transform
            xsl.Transform(xp, null, writer);
            return writer.ToString();
        }

        private static XmlReader GetReader()
        {
            var xsltContent = GetEmbeddedXslt(Filename);
            var xmlReader = XmlReader.Create(new StringReader(xsltContent));

            return xmlReader;
        }

        private static string GetEmbeddedXslt(string resource)
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