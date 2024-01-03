using RestSharp;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebApiLib.Model;
using WebApiLib.Serwis;

namespace WebApiLib
{

    public class szachoApi :  IszachoApi
    {
        CDNOffLineSrvSoapClient client;
        public szachoApi()
        {
            client = new CDNOffLineSrvSoapClient(ServiceReference1.CDNOffLineSrvSoapClient.EndpointConfiguration.CDNOffLineSrvSoap);
        }

        public Task<ObservableCollection<TwrKarty>> PobierzTwr(string ean)
        {
            return Task.Run(() =>
            {
                var response = client.ExecuteSQLCommand(ean);

                var odp = DeserializeFromXml<TwrKarty>(response);
                
                //if(odp!=null && odp.Count>0)
                //    return odp[0];
                //return null;

                return odp;
            });
        }

        public async Task<IEnumerable<T>> ExecSQLCommandAsync<T>(string command)
        {
            var options = new RestClientOptions("http://serwer.szachownica.com.pl/cdnofflinesrv/cdnofflinesrv.asmx")
            {
                MaxTimeout = 15000
                // Jeśli RestClientOptions zawiera właściwość Proxy, to:
                //Proxy = new WebProxy("http://10.0.2.2:8888")
            };

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Post);
            request.RequestFormat = DataFormat.Xml;

            request.AddHeader("SOAPAction", "http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLCommand");
            request.AddHeader("Content-Type", "text/xml; charset=utf-8");

            string xmlContent = @$"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body><ExecuteSQLCommand xmlns='http://www.comarch.pl/cdn/Products/CDN XL/' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'><sqlCommand>{command}</sqlCommand></ExecuteSQLCommand></s:Body></s:Envelope>";

            request.AddParameter("text/xml", xmlContent, ParameterType.RequestBody);
            request.AddHeader("Accept", "");

            try
            {
                var response = await client.ExecuteAsync(request);  

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var xmlResponse = response.Content;
                    XmlSerializer serializer = new XmlSerializer(typeof(SoapEnvelope), "http://www.comarch.pl/cdn/Products/CDN XL/");
                    using (StringReader reader = new StringReader(xmlResponse))
                    {
                        SoapEnvelope envelope = (SoapEnvelope)serializer.Deserialize(reader);

                        string encoded = envelope.Body.ExecuteSQLCommandResponse.ExecuteSQLCommandResult;
                        // Dekodowanie encji
                        string decodedXmlContent = System.Web.HttpUtility.HtmlDecode(encoded);

                        var odp = DeserializeFromXml<T>(decodedXmlContent);
                        return odp;
                    }
                }
                else
                {
                    // Handle non-OK responses as appropriate.
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode} and message: {response.Content}");
                }


            }
            catch (Exception ex)
            {
                // Log the exception.
                throw; // Re-throw to allow calling code to handle it as well.
            }

        }


        public static ObservableCollection<T> DeserializeFromXml<T>(string xml)
        {
            ObservableCollection<T> result;
            //Type type = result.GetType();

            XmlSerializer ser = new XmlSerializer(typeof(ObservableCollection<T>), new XmlRootAttribute("ROOT"));
            using (TextReader tr = new StringReader(xml))
            {
                result = (ObservableCollection<T>)ser.Deserialize(tr);
            }
            return result;
        }
    }
}
