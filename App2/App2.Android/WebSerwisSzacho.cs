﻿using App2.Model;
using App2.Model.ApiModel;
using Azure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

//[assembly: Dependency(typeof(App1.Droid.WebSerwisSzacho))]
namespace App2.Droid
{
    public sealed class WebSerwisSzacho : IMagazynSerwis
    {
        public ObservableCollection<Magazynn> Items { get; private set; }
        public List<TwrInfo> TwrkodList { get; private set; }
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
        public async Task<ObservableCollection<Magazynn>> GetAllCustomers(string criteria = null)
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
                    Items.Add(new Magazynn
                    {
                        Id = mag.Id,
                        Magazyn = mag.Magazyn,
                        Region = mag.Region,
                        Ilosc = mag.Ilosc,
                        MagKod = mag.MagKod
                    });
                }
                return Items;
            });
        }

        string odp;
        public async Task<string> InsertDataNiezgodnosci(List<Model.ListDiffrence> polecenie)
        {
            return await Task.Run(() =>
            {
                foreach (var lista in polecenie)
                {
                    var InsertString = $"cdn.PC_InsertRaportDostaw " +
                    $"{lista.XLGidMM}," +
                    $"{lista.GidMagazynu}," +
                    $"'{lista.NrDokumentu}'," +
                    $"'{lista.Operator}'," +
                    $"'{lista.twrkod}'," +
                    $"{lista.IleZMM}," +
                    $"{lista.IleZeSkan}," +
                    $"{lista.ilosc}," +
                    $"'{lista.DataDokumentu}'";

                    var respone = client.ExecuteSQLCommand(InsertString);

                    odp = respone;
                    odp = odp.Replace("<ROOT>\r\n  <Table>\r\n    <statuss>", "").Replace("</statuss>\r\n  </Table>\r\n</ROOT>", "");
                }
                return odp;
            });
        }

        public async Task<string> InsertDataNiezgodnosci(List<PMM_RaportElement> polecenie)
        {
            return await Task.Run(() =>
            {
                foreach (var lista in polecenie)
                {
                    var InsertString = $"cdn.PC_InsertRaportDostaw " +
                    $"{lista.TrnGidNumer}," +
                    $"{lista.MagNumer}," +
                    $"'{lista.TrnDokumentObcy}'," +
                    $"'{lista.Operator}'," +
                    $"'{lista.Twr_Kod}'," +
                    $"{lista.OczekiwanaIlosc}," +
                    $"{lista.RzeczywistaIlosc}," +
                    $"{lista.Difference}," +
                    $"'{lista.DataMM}'";

                    var respone = client.ExecuteSQLCommand(InsertString);

                    odp = respone;
                    odp = odp.Replace("<ROOT>\r\n  <Table>\r\n    <statuss>", "").Replace("</statuss>\r\n  </Table>\r\n</ROOT>", "");
                }
                return odp;
            });
        }


        public async Task<TwrInfo> PobierzTwr(string ean)
        {

            try
            {
                return await Task.Run(() =>
                {
                    var response = client.ExecuteSQLCommand(ean);

                    var twrList = DeserializeFromXml<TwrInfo>(response);

                    // Zakładając, że chcesz zwrócić pierwszy element z listy
                    if (twrList.Any())
                    {
                        return twrList[0];
                    }
                    else
                    {
                        throw new Exception("Lista produktów jest pusta.");
                    }
                });

            }
            catch (Exception ex)
            {
                // Obsługa błędów - możesz zalogować błąd lub zgłosić go wyżej
                throw new Exception("Wystąpił błąd podczas pobierania informacji o produkcie.", ex);
            }

        }

         
        
        public async Task<List<SzachoSettings>> GetSzachoSettings()
        {

            return await Task.Run(() =>
            {
              
                var response = client.ExecuteSQLCommand("cdn.PC_SprawdzWersje");

                var sttings = DeserializeFromXml<SzachoSettings>(response);

                // Zakładając, że chcesz zwrócić pierwszy element z listy
                if (sttings.Any())
                {
                    return sttings.ToList();
                }
                else
                {
                    throw new Exception("nie można sprawidzić wersji");
                } 

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
                        IsSendData = akcje.IsSendData,
                        TwrCena30 = akcje.TwrCena30
                    });
                }

                return AkcjeGidNazwaList;
            });
        }

        //public async Task<string> InsertDataSkan(IList<AkcjeNagElem> polecenie, Int16 magnumer, string ase_operator)
        //{

        //    try
        //    {
        //        return await Task.Run(() =>
        //            {
        //                foreach (var lista in polecenie)
        //                {
        //                    var InsertString = $@"cdn.PC_InsertAkcjeSkan
        //                        {lista.AkN_GidNumer},
        //                        {magnumer},
        //                        '{lista.TwrGrupa}',
        //                        '{lista.TwrDep}',
        //                        {lista.TwrGidNumer},
        //                        {lista.TwrStan},
        //                        {lista.TwrSkan},
        //                        '{ase_operator}'";

        //                    var respone = client.ExecuteSQLCommand(InsertString);

        //                    odp = respone;
        //                    odp = odp.Replace("<ROOT>\r\n  <Table>\r\n    <statuss>", "").Replace("</statuss>\r\n  </Table>\r\n</ROOT>", "");
        //                }
        //                return odp;
        //            });
        //    }
        //    catch (Exception s)
        //    {

        //        odp = s.Message;
        //        return odp;
        //    }
        //}
        public async Task<List<StatusTable>> InsertDataSkan(IList<AkcjeNagElem> polecenie, Int16 magnumer, string ase_operator)
        {
           List<StatusTable> list = new List<StatusTable>();

           return await Task.Run(() =>
           {
                try
                {
                    StringBuilder commandText = new StringBuilder();

                    foreach (var lista in polecenie)
                    {
                        commandText.AppendLine(" cdn.PC_InsertAkcjeSkan2");
                        commandText.AppendLine($"@ASN_AknNumer = {lista.AkN_GidNumer},");
                        commandText.AppendLine($"@ASN_MagNumer = {magnumer},");
                        commandText.AppendLine($"@ASE_Grupa = '{lista.TwrGrupa}',");
                        commandText.AppendLine($"@ASE_TwrDep = '{lista.TwrDep}',");
                        commandText.AppendLine($"@ASE_TwrNumer = {lista.TwrGidNumer},");
                        commandText.AppendLine($"@ASE_TwrStan = {lista.TwrStan},");
                        commandText.AppendLine($"@ASE_TwrSkan = {lista.TwrSkan},");
                        commandText.AppendLine($"@operator = '{ase_operator}';");

                        if (lista != polecenie.Last())
                        {
                            commandText.AppendLine("EXEC");
                        }

                    }
                    var response = client.ExecuteSQLCommand(commandText.ToString());

                    List<StatusTable> odp  = DeserializeFromXmlStatus(response);
                    //List<StatusTable> result = odp.Cast<StatusTable>().ToList();
                    return odp;
                }
                catch (Exception s)
                {
                    odp = s.Message;
                    return null;
                }
            });

        }

        public async Task<ObservableCollection<T>> PobierzDaneZWeb<T>(string query)
        {
            ObservableCollection<T> res = new ObservableCollection<T>();
            return await Task.Run(() =>
            {
                var respone = client.ExecuteSQLCommand(query);

                return DeserializeFromXml<T>(respone);

            });
        }

        public static List<StatusTable> DeserializeFromXmlStatus(string xmlContent)
        {
            XDocument xdoc = XDocument.Parse(xmlContent);
            var tableItems = new List<StatusTable>();

            foreach (var element in xdoc.Root.Elements())
            {
                var tableItem = new StatusTable
                {
                    AknNumer = (int)element.Element("AknNumer"),
                    MagNumer = (int)element.Element("MagNumer"),
                    TwrNumer = (int)element.Element("TwrNumer"),
                    Statuss = (string)element.Element("Statuss")
                };

                tableItems.Add(tableItem);
            }

            return tableItems;
        }

        public static T DeserializeXml<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("ROOT"));
            using (TextReader reader = new StringReader(xmlString))
            {
                return (T)serializer.Deserialize(reader);
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