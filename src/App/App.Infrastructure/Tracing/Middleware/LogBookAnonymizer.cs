using App.Api.Enum;
using App.Core.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Infrastructure.Tracing.Middleware
{
    public class LogBookAnonymizer : ILogBookAnonymizer
    {
        private const string RedactedMessage = "***REDACTED***";
        private const string NotCapturedMessage = "***NOT CAPTURED***";
        /// <summary>
        /// Dum dum anonymizer
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Anonymize(string content, string path, Direction direction)
        {
            switch (direction)
            {
                case Direction.In: return ProcessInput(content, path);
                case Direction.Out: return ProcessOutput(content, path);
                default:
                    throw new Exception("Direction not supported");
            }
        }

        private string ProcessOutput(string content, string path)
        {
            if (path.Contains("auth/token"))
            {
                var anonKeys = new List<string>
                {
                    "access_token", "refresh_token"
                };

                if (IsValidJson(content, out JObject obj))
                {
                    foreach (var key in anonKeys)
                    {
                        if (obj[key] != null) obj[key] = RedactedMessage;
                    }

                    return obj.ToString();
                }
            }

            return content;
        }

        private string ProcessInput(string content, string path)
        {
            if (path.Contains("auth/token"))
            {
                var anonSegments = new List<string>
                {
                    "password", "client_secret", "refresh_token"
                };

                // content is urlencoded
                var splits = content.Split(new[] { '&' }).ToList();
                List<string> parts = new List<string>();

                foreach (var split in splits)
                {
                    var itemSplit = split.Split(new[] { '=' });
                    if (anonSegments.Contains(itemSplit[0]))
                    {
                        parts.Add($"{itemSplit[0]}={RedactedMessage}");
                    }
                    else parts.Add($"{itemSplit[0]}={itemSplit[1]}");
                }

                return string.Join("&", parts);
            }

            return content;
        }

        private static bool IsValidJson(string strInput, out JObject obj)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    obj = JObject.Parse(strInput);
                    return true;
                }
                catch (Exception) //some other exception
                {
                    obj = null;
                    return false;
                }
            }

            obj = null;
            return false;
        }
    }
}
