using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

#region Referencias
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
#endregion

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

        #region APPs

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
            imprimirCosas(stuff.webPages.value,1);
            //iMPRIMIR IMAGES
            datum("Images");
            imprimirCosas(stuff.images.value,2);
            //IMPRIMIR RELATEDSEARCHES
            datum("RELATEDSEARCHES");
            imprimirCosas(stuff.relatedSearches.value,3);
            //IMPRIMIR VIDEOS
            datum("videos");
            imprimirCosas(stuff.videos.value,4);
        }

        static void datum(string ke)
        {
            Console.WriteLine("\n***************************************************************************");
            Console.Write(ke);
            Console.WriteLine("\n***************************************************************************");
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

        #endregion

        #region Impresion

        static void imprimirCosas(dynamic stuff, int Que)
        {
            //1-webpages 2-images 3-relatedsearches 4-videos
            int i = 0;
            foreach (JObject x in stuff)
            {
                int cc = 0;
                foreach (KeyValuePair<String, JToken> app in x)
                {
                    var appName = app.Key;
                    var description = app.Value;
                    if (cc == 0)
                    {
                        Console.WriteLine("\n");
                        Console.Write("Resultado ");
                        Console.Write(i);
                        Console.WriteLine(":");
                        i++;
                        cc++;
                    }
                    imprimirQue(Que,appName,description);
                }
            }
        }

        static void imprimirQue(int flag,string dato, JToken desc)
        {
            //1-webpages 2-images 3-relatedsearches 4-videos
            switch (flag)
            {
                case 1:
                    iWebpages(dato, desc);
                    break;
                case 2:
                    iImages(dato,desc);
                    break;
                case 3:
                    iRelatedsearches(dato, desc);
                    break;
                case 4:
                    iVideos(dato, desc);
                    break;
                default:
                    Console.WriteLine(":(");
                    break;
            }
        }

        static void iWebpages(string dato, JToken desc)
        {
            if (dato.Equals("id") || dato.Equals("displayUrl") || dato.Equals("deepLinks"))
            {;}
            else
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);
            }
        }

        static void iImages(string dato, JToken desc)
        {
            if (dato.Equals("thumbnail") || dato.Equals("hostPageUrl") || dato.Equals("contentSize") || dato.Equals("encodingFormat") || dato.Equals("width") || dato.Equals("height") || dato.Equals("hostPageDisplayUrl"))
            {;}
            else
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);
            }
        }

        static void iRelatedsearches(string dato, JToken desc)
        {
            if (dato.Equals("displayText"))
            {;}
            else
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);
            }
        }

        static void iVideos(string dato, JToken desc)
        {
            if (dato.Equals("webSearchUrl") || dato.Equals("thumbnailUrl") || dato.Equals("datePublished") || dato.Equals("publisher") || dato.Equals("hostPageUrl") || dato.Equals("encodingFormat") || dato.Equals("hostPageDisplayUrl") || dato.Equals("width") || dato.Equals("height") || dato.Equals("duration") || dato.Equals("motionThumbnailUrl") || dato.Equals("embedHtml") || dato.Equals("allowHttpsEmbed") || dato.Equals("viewCount") || dato.Equals("thumbnail") || dato.Equals("allowMobileEmbed"))
            {;}
            else
            {
            Console.WriteLine("-------------------------------------------------------------");
            Console.Write(FirstCharToUpper(dato));
            Console.WriteLine(":\n-------------------------------------------------------------");
            Console.WriteLine(desc);
            }
        }

        #endregion
    }
}