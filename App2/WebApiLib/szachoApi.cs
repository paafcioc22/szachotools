using RestSharp;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
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
                // Jeśli RestClientOptions zawiera właściwość Proxy, to:
                Proxy = new WebProxy("http://10.0.2.2:8888")
            };

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Post);
            request.RequestFormat = DataFormat.Xml;

            request.AddHeader("SOAPAction", "http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLCommand");
            request.AddHeader("Content-Type", "text/xml; charset=utf-8");

            string xmlContent = @$"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body><ExecuteSQLCommand xmlns='http://www.comarch.pl/cdn/Products/CDN XL/' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'><sqlCommand>{command}</sqlCommand></ExecuteSQLCommand></s:Body></s:Envelope>";

            request.AddParameter("text/xml", xmlContent, ParameterType.RequestBody);
            request.AddHeader("Accept", "");

            var response = await client.ExecuteAsync(request);

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



        //public async Task<TwrKarty> PobierzTwrAsync(string ean)
        //{
        //    //var request = new ExecuteSQLCommandRequest()
        //    //{
        //    //    Body = new ExecuteSQLCommandRequestBody()
        //    //    {
        //    //        sqlCommand = ean
        //    //    }
        //    //};

        //    var respone = await client.ExecuteSQLCommandAsync(ean);

        //    //var result = respone.Body.ExecuteSQLCommandResult;

        //    var twr = DeserializeFromXml<TwrKarty>(respone);

        //    return twr[0];
        //}

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
