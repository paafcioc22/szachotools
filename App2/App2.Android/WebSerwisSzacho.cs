using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        public ObservableCollection<Magazynn> Items { get; private set; } 
        public List<RaportListaMM> TwrkodList { get; private set; } 
        public ObservableCollection<AkcjeNagElem> AkcjeGidNazwaList { get; private set; } 

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
        public async Task<ObservableCollection<Magazynn>> GetAllCustomers(string  criteria = null)
        {
            return await Task.Run(() =>
            {
                Items = new ObservableCollection<Magazynn>();
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
                       Ilosc = mag.Ilosc,
                       MagKod=mag.MagKod
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
                        symbol = mag.symbol,
                        cena1 = mag.cena1
                        
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


                //XmlSerializer serializer = new XmlSerializer(typeof(List<Magazyn>), new XmlRootAttribute("ROOT"));
                //StringReader reader = new StringReader(result);
                //List<Magazyn> magazyny = (List<Magazyn>)serializer.Deserialize(reader); 


                _wersja = odczytaj.wersja[0].VersionApp;
                     
                return _wersja;
                //return ver;
            });
        }

        public async Task<ObservableCollection<AkcjeNagElem>> GetGidAkcje(string query)
        {

            return await Task.Run(() =>
            {
                AkcjeGidNazwaList = new ObservableCollection<AkcjeNagElem>();

                var respone = client.ExecuteSQLCommand(query);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respone);

                TextReader reader = new StringReader(respone);

                XmlSerializer serializer = new XmlSerializer(typeof(GidNazwa));
                GidNazwa gidNazwa = (GidNazwa)serializer.Deserialize(reader);

                foreach (var akcje in gidNazwa.GidNazwaLista)
                {
                    var datastart = Convert.ToDateTime(akcje.AkN_DataStart).AddHours(2);
                    var datakoniec = Convert.ToDateTime(akcje.AkN_DataKoniec).AddHours(2);
                    AkcjeGidNazwaList.Add(new AkcjeNagElem
                    {
                        AkN_GidNumer = akcje.AkN_GidNumer,
                        AkN_GidTyp = akcje.AkN_GidTyp,
                        AkN_GidNazwa = akcje.AkN_GidNazwa,
                        AkN_NazwaAkcji = akcje.AkN_NazwaAkcji,
                        Ake_ElemLp = akcje.Ake_ElemLp,
                        Ake_FiltrSQL = akcje.Ake_FiltrSQL,
                        AkN_DataStart = datastart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        AkN_DataKoniec = datakoniec.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 
                        Ake_NazwaFiltrSQL = akcje.Ake_NazwaFiltrSQL,
                        TwrKod = akcje.TwrKod,
                        TwrNazwa = akcje.TwrNazwa,
                        TwrGrupa = akcje.TwrGrupa,
                        TwrDep = akcje.TwrDep,
                        TwrGidNumer = akcje.TwrGidNumer,
                        TwrStan = akcje.TwrStan,
                        TwrUrl = akcje.TwrUrl,
                        TwrSymbol = akcje.TwrSymbol,
                        TwrCena = akcje.TwrCena,
                        TwrCena1 = akcje.TwrCena1,
                        TwrSkan = 0,
                        TwrEan = akcje.TwrEan,
                        IsSendData = akcje.IsSendData
                    });
                }

                return AkcjeGidNazwaList;
            });
        }

        public async Task<string> InsertDataSkan(IList<AkcjeNagElem> polecenie,Int16 magnumer,string ase_operator)
        {

            try
            {
                return await Task.Run(() =>
                    {
                        foreach (var lista in polecenie)
                        {
                            var InsertString = $@"cdn.PC_InsertAkcjeSkan
                                {lista.AkN_GidNumer},
                                {magnumer},
                                '{lista.TwrGrupa}',
                                '{lista.TwrDep}',
                                {lista.TwrGidNumer},
                                {lista.TwrStan},
                                {lista.TwrSkan},
                                '{ase_operator}'";

                            var respone = client.ExecuteSQLCommand(InsertString);

                            odp = respone;
                            odp = odp.Replace("<ROOT>\r\n  <Table>\r\n    <statuss>", "").Replace("</statuss>\r\n  </Table>\r\n</ROOT>", "");
                        }
                        return odp;
                    });
            }
            catch (Exception s)
            {

                odp = s.Message;
                return odp;
            }
        }

        public async Task<ObservableCollection<T>> PobierzDaneZWeb<T>(string query)
        {
            ObservableCollection<T> res = new ObservableCollection<T>();
            return await Task.Run(() =>
            {                  
                var respone = client.ExecuteSQLCommand(query);

                return  DeserializeFromXml<T>(respone);

            });
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
        public class GidNazwa
        {
            [XmlElement("Table", typeof(AkcjeNagElem))]
            public List<AkcjeNagElem> GidNazwaLista { get; set; }
        }

        [XmlRoot("ROOT")]
        public class MagazynLista
        {
            [XmlElement("Table", typeof(Magazynn))]
            public ObservableCollection<Magazynn> MagazynList { get; set; }
        }

        [XmlRoot("ROOT")]
        public class TwrLista
        {
            [XmlElement("Table", typeof(RaportListaMM))]
            public List<RaportListaMM> TwrList { get; set; }
        }
    }
}