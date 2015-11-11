using System;
using System.Configuration;

namespace DocumentDB
{
    public interface IConfig
    {
        string ConnectionString(string name);
        T Value<T>(string key, T defaultValue = default(T));
    }


    public class AppSettingsConfig : IConfig
    {
        public string ConnectionString(string name)
        {
            if (ConfigurationManager.ConnectionStrings[name] == null)
            {
                throw new ConfigurationErrorsException();
            }
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public T Value<T>(string key, T defaultValue = default(T))
        {
            var value = ConfigurationManager.AppSettings[key];
            return value == null ? defaultValue : (T) Convert.ChangeType(value, typeof (T));
                //Conversion.ConvertTo<T>(value);
        }
    }

    public class DBConfig
    {
        public DBConfig(IConfig config)
        {
            Url = config.Value<string>("DBURL");
            Key = config.Value<string>("privatekey");
        }

        public string Url { get; set; }
        public string Key { get; set; }
    }
}