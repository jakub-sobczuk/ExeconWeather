using ExeconWeather.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ExeconWeather.Utils
{
    public static class CityNameToIDConverter
    {
        public static int GetIDByCityName(string name)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CityNameToIDConverter)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("ExeconWeather.Assets.city.list.json");

            List<CityModel> cityModels = new List<CityModel>();
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                cityModels = JsonConvert.DeserializeObject<List<CityModel>>(json);
            }

            foreach (var item in cityModels)
            {    
                if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return (int)item.Id;
                }
            }

            return -1;
        }
    }
}
