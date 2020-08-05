﻿using ExeconWeather.Services.Interface;
using System.IO;
using System.Net;
using System.Text;

namespace ExeconWeather.Services.Implementation
{
    public class WebService : IWebService
    {
        public string GetResponseText(string address)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, encoding))
                    return reader.ReadToEnd();
            }
        }
    }
}
