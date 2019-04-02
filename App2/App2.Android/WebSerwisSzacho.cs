using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App2.Model;
using Xamarin.Forms;

//[assembly: Dependency(typeof(App1.Droid.WebSerwisSzacho))]
namespace App2.Droid
{
    public sealed class WebSerwisSzacho : IMagazynSerwis
    {
        public List<Magazynn> Items { get; private set; } 
        public List<RaportListaMM> TwrkodList { get; private set; } 

        public WebSzacho.CDNOffLineSrv client;

        public WebSerwisSzacho()
        {
            client = new WebSzacho.CDNOffLineSrv();
        }
        #region magazynDodaj
        static Magazynn FromASMXServiceTodoItem(Magazynn item)
        {
            return new Magazynn
            {
                Id = item.Id,
                Magazyn = item.Magazyn,
                Region = item.Region,
                Ilosc = item.Ilosc
            };
        }
        #endregion
        public async Task<List<Magazynn>> GetAllCustomers(string  criteria = null)
        {
            return await Task.Run(() =>
            {
                Items = new List<Magazynn>();
                var respone = client.ExecuteSQLCommand(criteria);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respone);

                TextReader reader = new StringReader(respone);
                
                    XmlSerializer serializer = new XmlSerializer(typeof(MagazynLista));
                    MagazynLista maglista = (MagazynLista)serializer.Deserialize(reader);

                foreach (var mag in maglista.MagazynList)
                {
                      Items.Add(new Magazynn{
                       Id = mag.Id,
                       Magazyn = mag.Magazyn,
                       Region = mag.Region,
                       Ilosc = mag.Ilosc
                    });
                }
                return Items;
             });
        }

        string odp;
        public async Task<string> InsertDataNiezgodnosci(ObservableCollection<Model.ListDiffrence> polecenie)
        {
            return await Task.Run(() =>
            {
                foreach (var lista in polecenie)
                {
                    var InsertString = $"cdn.PC_InsertRaportDostaw " +
                    $"{lista.XLGidMM}," +
                    $"{lista.GidMagazynu}," +
                    $"'{lista.NrDokumentu}',"+
                    $"'{lista.Operator}',"+
                    $"'{lista.twrkod}',"+
                    $"{lista.IleZMM},"+
                    $"{lista.IleZeSkan},"+
                    $"{lista.ilosc},"+
                    $"'{lista.DataDokumentu}'";
                    
                    var respone = client.ExecuteSQLCommand(InsertString);
                     
                    odp = respone;
                    odp = odp.Replace("<ROOT>\r\n  <Table>\r\n    <statuss>", "").Replace("</statuss>\r\n  </Table>\r\n</ROOT>","");
                }
                return odp;
            });
        }

         

        public async Task<List<RaportListaMM>> PobierzTwr(string ean)
        {
            return await Task.Run(() =>
            {
                TwrkodList = new List<RaportListaMM>();

                var respone = client.ExecuteSQLCommand(ean);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respone);

                TextReader reader = new StringReader(respone);

                XmlSerializer serializer = new XmlSerializer(typeof(TwrLista));
                TwrLista twrlista = (TwrLista)serializer.Deserialize(reader);

                foreach (var mag in twrlista.TwrList)
                {
                    TwrkodList.Add(new RaportListaMM
                    {
                        twrkod = mag.twrkod,
                        TwrGidnumer = mag.TwrGidnumer,
                        ean = mag.ean,
                        url = mag.url,
                        nazwa = mag.nazwa,
                        cena = mag.cena,
                        symbol = mag.symbol
                    });
                }

                return TwrkodList;
            });
        }
        Wersja wersja;

        


        public string _wersja;
        public async Task<string> PobierzWersjeApki()
        {

            return await Task.Run(() =>
            {
                wersja = new Wersja();
                var respone =   client.ExecuteSQLCommand("cdn.PC_SprawdzWersje");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respone);

                TextReader reader = new StringReader(respone);

                XmlSerializer serializer = new XmlSerializer(typeof(AppVersionList)); 

                AppVersionList odczytaj = (AppVersionList)serializer.Deserialize(reader); 

                _wersja= odczytaj.wersja[0].VersionApp;
                     
                return _wersja;
                //return ver;
            });
        }

        public class Wersja
        {
            public string VersionApp { get; set; }
        }

        [XmlRoot("ROOT")]
        public class AppVersionList
        {
            [XmlElement("Table", typeof(Wersja))] 
            public List<Wersja> wersja { get; set; }

        }


        [XmlRoot("ROOT")]
        public class MagazynLista
        {
            [XmlElement("Table", typeof(Magazynn))]
            public List<Magazynn> MagazynList { get; set; }
        }

        [XmlRoot("ROOT")]
        public class TwrLista
        {
            [XmlElement("Table", typeof(RaportListaMM))]
            public List<RaportListaMM> TwrList { get; set; }
        }
    }
}