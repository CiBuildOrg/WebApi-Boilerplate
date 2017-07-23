using System;
using Newtonsoft.Json.Linq;

namespace App.Core.Utils
{
    public static class JsonUtils
    {
        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((!strInput.StartsWith("{") || !strInput.EndsWith("}")) &&
                (!strInput.StartsWith("[") || !strInput.EndsWith("]")))
            {
                return false;
            }

            try
            {
                JToken.Parse(strInput);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}