using System.Configuration;
using App.Core.Contracts;
using App.Exceptions;

namespace App.Api
{
    public class WebConfiguration : IConfiguration
    {
        private string GetErrorMessage(string configurationKey)
        {
            return $"Could not find the configuration key {configurationKey}";
        }

        public string GetString(string configurationKey)
        {
            var item = ConfigurationManager.AppSettings[configurationKey];

            if(item == null)
                throw new ConfigurationNotFoundException(GetErrorMessage(configurationKey));

            return item;
        }

        public int GetInt(string configurationKey)
        {
            return int.Parse(GetString(configurationKey));
        }

        public bool GetBool(string configurationKey)
        {
            return bool.Parse(GetString(configurationKey));
        }
    }
}