using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

/*
 Referencias utilizadas :
https://dev.cognitive.microsoft.com/docs/services/56b43eeccf5ff8098cef3807/operations/56b4447dcf5ff8098cef380d 
https://docs.microsoft.com/es-mx/rest/api/cognitiveservices/bing-web-api-v5-reference#headers
https://tfsrocks.wordpress.com/2014/04/06/extend-visual-studio-web-test-framework-to-extract-json-value/
https://stackoverflow.com/questions/10928528/receiving-json-data-back-from-http-request
https://stackoverflow.com/questions/12676746/parse-json-string-in-c-sharp
https://stackoverflow.com/questions/40416569/newtonsoft-could-not-be-found
http://www.c-sharpcorner.com/blogs/first-letter-in-uppercase-in-c-sharp1 
 */

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

        public static string FirstCharToUpper(string s)
        {
            //obtenido de http://www.c-sharpcorner.com/blogs/first-letter-in-uppercase-in-c-sharp1 

            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        static void obtenerDatos(dynamic stuff)
        {
            // Imprimir WEBPAGES   
             datum("WEBPAGES");
            imprimirCosas(stuff.webPages.value);
            //iMPRIMIR IMAGES
            datum("Images");
            imprimirCosas(stuff.images.value);
            //IMPRIMIR RELATEDSEARCHES
            datum("RELATEDSEARCHES");
            imprimirCosas(stuff.relatedSearches.value);
            //IMPRIMIR VIDEOS
            datum("videos");
            imprimirCosas(stuff.videos.value);
        }

        static void datum(string ke)
        {
            Console.WriteLine("\n***************************************************************************");
            Console.Write(ke);
            Console.WriteLine("\n***************************************************************************");
        }

        static void imprimirCosas(dynamic stuff)
        {
            int i = 0;

            foreach (JObject x in stuff)
            {
                int cc = 0; 
                foreach (KeyValuePair<String, JToken> app in x)
                {
                    var appName = app.Key;
                    var description = app.Value;

                    if (!appName.Equals("dateLastCrawled"))
                    {
                        if (cc==0)
                        {
                            Console.WriteLine("\n");
                            Console.Write("Resultado ");
                            Console.Write(i);
                            Console.WriteLine(":");
                            i++;
                            cc++;
                        }
                        Console.WriteLine("-------------------------------------------------------------");
                        if (appName.Equals("snippet"))
                        {
                            Console.Write("About");
                        }
                        else
                        {
                            Console.Write(FirstCharToUpper(appName));
                        }
                        Console.WriteLine(":\n-------------------------------------------------------------");
                        Console.WriteLine(description);
                    }
                    
                }
            }
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
            Console.WriteLine("+Requested URL:\n----------------------------------------------------");
            Console.WriteLine(uri);
            Console.WriteLine("\n");
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("+Response:\n----------------------------------------------------");
            dynamic stuff = JsonConvert.DeserializeObject(json);

            obtenerDatos(stuff);
        
        }
    }
}



