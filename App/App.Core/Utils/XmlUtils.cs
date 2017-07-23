using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace App.Core.Utils
{
    public static class XmlUtils
    {
        public static bool IsValidXml(string xmlString)
        {
            var tagsWithData = new Regex("<\\w+>[^<]+</\\w+>");

            //Light checking
            if (string.IsNullOrEmpty(xmlString) || tagsWithData.IsMatch(xmlString) == false)
            {
                return false;
            }

            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}