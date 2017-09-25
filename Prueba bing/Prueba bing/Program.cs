using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CSHttpClientSample
{
    static class Program
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "64f004f772a540ab92636c548eafff76");

            // Request parameters
            queryString["q"] = "dog";
            queryString["count"] = "10";
            queryString["offset"] = "0";
            queryString["mkt"] = "en-us";
            queryString["safesearch"] = "Moderate";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

            //URL
            Console.WriteLine("+Requested URL----------------------------------------------------");
            Console.WriteLine(uri);
            Console.WriteLine("\n");
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("+Response    ----------------------------------------------------");
            
            // Console.WriteLine(json);
             //Console.WriteLine("\n");
             
            
            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    var appName = app.Key;
                    var description = (String)app.Value["Description"];
                    var value = (String)app.Value["Value"];

                    Console.WriteLine(appName);
                    Console.WriteLine(description);
                    Console.WriteLine(value);
                    Console.WriteLine("\n");
                }
            }
            
        }
    }
}