using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

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
        static string wPages, v, i, rSearches;
        static void Main()
        {
            BorrarA();
            //Leer archivo
            StreamReader objReader = new StreamReader(@"item.txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();

            while (sLine != null)
            {
                sLine = objReader.ReadLine();

                if (sLine != null)
                    arrText.Add(sLine);
            }

            objReader.Close();



            //Correr el programa
            foreach (string sOutput in arrText)
            {
                MakeRequest(sOutput);
                //Console.WriteLine("Hit ENTER to exit...");
                //Console.ReadLine();
                System.Threading.Thread.Sleep(4000);
                return;
            }


        }
        public static void BorrarA()
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("Test.txt");
                //Write a line of text
                sw.Write("");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

        }
        static void escribirText(string valor)
        {
            string path = @"Test.txt";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    //sw.WriteLine("");
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(valor + "[");
                //sw.WriteLine("");

            }
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
            if (stuff.webPages != null)
            {
                datum("WEBPAGES");
                imprimirCosas(stuff.webPages.value, 1);
            }
            //iMPRIMIR IMAGES
            if (stuff.images != null)
            {
                datum("Images");
                imprimirCosas(stuff.images.value, 2);
            }
            //IMPRIMIR RELATEDSEARCHES
            if (stuff.relatedSearches != null)
            {
                datum("RELATEDSEARCHES");
                imprimirCosas(stuff.relatedSearches.value, 3);
            }
            //IMPRIMIR VIDEOS
            if (stuff.videos != null)
            {
                datum("videos");
                imprimirCosas(stuff.videos.value, 4);
            }
            escribirText(wPages);
            escribirText(i);
            escribirText(rSearches);
            escribirText(v);
        }

        static void datum(string ke)
        {
            /*Console.WriteLine("\n***************************************************************************");
            Console.Write(ke);
            Console.WriteLine("\n***************************************************************************");*/
        }

        static async void MakeRequest(string valor)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "64f004f772a540ab92636c548eafff76");
            // Request parameters
            queryString["q"] = valor;
            queryString["count"] = "10";
            queryString["offset"] = "0";
            queryString["mkt"] = "en-us";
            queryString["safesearch"] = "Moderate";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

            //URL
            /*Console.WriteLine("+Requested URL:\n----------------------------------------------------");
            Console.WriteLine(uri);
            Console.WriteLine("\n");*/
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            // Console.WriteLine("+Response:\n----------------------------------------------------");
            dynamic stuff = JsonConvert.DeserializeObject(json);
            //Console.WriteLine(json);

           obtenerDatos(stuff);

        }

        #endregion

        #region Impresion

        static void imprimirCosas(dynamic stuff, int Que)
        {
            //1-webpages 2-images 3-relatedsearches 4-videos
            int i = 1;
            foreach (JObject x in stuff)
            {
                int cc = 0;
                foreach (KeyValuePair<String, JToken> app in x)
                {
                    var appName = app.Key;
                    var description = app.Value;
                    if (cc == 0)
                    {
                        /*Console.WriteLine("\n");
                        Console.Write("Resultado ");
                        Console.Write(i);
                        Console.WriteLine(":");*/
                        i++;
                        cc++;
                    }
                    imprimirQue(Que, appName, description);
                }

            }

        }

        static void imprimirQue(int flag, string dato, JToken desc)
        {
            //1-webpages 2-images 3-relatedsearches 4-videos
            switch (flag)
            {
                case 1:
                    iWebpages(dato, desc);
                    break;
                case 2:
                    iImages(dato, desc);
                    break;
                case 3:
                    iRelatedsearches(dato, desc);
                    break;
                case 4:
                    iVideos(dato, desc);
                    break;
                default:
                    //Console.WriteLine(":(");
                    break;
            }
        }

        static void iWebpages(string dato, JToken desc)
        {

            if (dato.Equals("id") || dato.Equals("displayUrl") || dato.Equals("deepLinks") || dato.Equals("about") || dato.Equals("insightsSourcesSummary"))
            {

                //Console.Write("About");
            }
            else
            {
                /*Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);*/
                string cadena = (string)desc;
                wPages += (cadena + "¬");
            }
        }

        
        static void iImages(string dato, JToken desc)
        {
            if (dato.Equals("thumbnail") || dato.Equals("insightsSourcesSummary") || dato.Equals("hostPageUrl") || dato.Equals("contentSize") || dato.Equals("encodingFormat") || dato.Equals("width") || dato.Equals("height") || dato.Equals("hostPageDisplayUrl"))
            {

                //Console.Write("About");
            }
            else
            {
                /* Console.WriteLine("-------------------------------------------------------------");
                 Console.Write(FirstCharToUpper(dato));
                 Console.WriteLine(":\n-------------------------------------------------------------");
                 Console.WriteLine(desc);*/
                string cadena = desc.ToString();
                i += (cadena + "]");
            }
        }

        static void iRelatedsearches(string dato, JToken desc)
        {
            if (dato.Equals("displayText") || dato.Equals("insightsSourcesSummary"))
            {

                //Console.Write("About");
            }
            else
            {
                /*Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);*/
                string cadena = (string)desc;
                rSearches += (cadena + "¬");
            }
        }

        static void iVideos(string dato, JToken desc)
        {
            if (dato.Equals("insightsSourcesSummary")|| dato.Equals("webSearchUrl") || dato.Equals("thumbnailUrl") || dato.Equals("datePublished") || dato.Equals("publisher") || dato.Equals("hostPageUrl") || dato.Equals("encodingFormat") || dato.Equals("hostPageDisplayUrl") || dato.Equals("width") || dato.Equals("height") || dato.Equals("duration") || dato.Equals("motionThumbnailUrl") || dato.Equals("embedHtml") || dato.Equals("allowHttpsEmbed") || dato.Equals("viewCount") || dato.Equals("thumbnail") || dato.Equals("allowMobileEmbed") || dato.Equals("name"))
            {

                //Console.Write("About");
            }
            else
            {
                /*Console.WriteLine("-------------------------------------------------------------");
                Console.Write(FirstCharToUpper(dato));
                Console.WriteLine(":\n-------------------------------------------------------------");
                Console.WriteLine(desc);*/
                string cadena = (string)desc;
                v += (cadena + "¬");
            }
        }

        #endregion

    }
}