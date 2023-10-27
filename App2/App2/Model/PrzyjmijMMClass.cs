using App2.Model.ApiModel;
using App2.OptimaAPI;
using Microsoft.AppCenter.Crashes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.Model
{


    public class PrzyjmijMMClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string id { set; get; }
        public string twrkod { set; get; }
        public int ilosc { set; get; }
        public string url { set; get; }
        public string symbol { set; get; }
        public string nazwa { set; get; }
        public string ean { set; get; }
        public string cena { set; get; }
        public string cena1 { set; get; }
        public string nrdokumentuMM { set; get; }
        public string DatadokumentuMM { set; get; }
        public string OpisdokumentuMM { set; get; }
        public string Operator { set; get; }
        public int GIDdokumentuMM { set; get; }
        public int GIDMagazynuMM { set; get; }
        public int XLGIDMM { set; get; }
        public bool StatusMM { set; get; }


        public string GetNrDokMmp
        {
            get { return nrdokumentuMM; }
            set { nrdokumentuMM = value; }
        }

        private int _iloscOK;
        public int ilosc_OK
        {
            get { return _iloscOK; }
            set
            {
                if (_iloscOK == value)
                    return;
                _iloscOK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ilosc_OK)));
            }
        }




        ServicePrzyjmijMM serviceApi;
        public PrzyjmijMMClass()
        {
            var app = Application.Current as App;
            serviceApi = new ServicePrzyjmijMM();

        }


        private SQLiteAsyncConnection _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();

        public static ObservableCollection<PrzyjmijMMClass> ListOfTwrOnMM = new ObservableCollection<PrzyjmijMMClass>();
        public static ObservableCollection<PrzyjmijMMClass> ListaMMDoPrzyjcia = new ObservableCollection<PrzyjmijMMClass>();



        public async Task getListMM(bool CzyZatwierdzone, int pastDays = 100)
        {
            PrzyjmijMMClass przyjmijMM;

            var listaMM = await serviceApi.GetDokMmList(CzyZatwierdzone, pastDays);

            try
            {
                ListaMMDoPrzyjcia.Clear();

                if (listaMM.IsSuccessful)
                {
                    if (listaMM.Data != null)
                    {
                        foreach (var item in listaMM.Data.OrderByDescending(s => s.DataMM))
                        {
                            przyjmijMM = new PrzyjmijMMClass()
                            {
                                GIDdokumentuMM = item.Trn_Trnid,
                                DatadokumentuMM = item.DataMM,
                                nrdokumentuMM = item.TrN_DokumentObcy,
                                OpisdokumentuMM = item.TrN_Opis,
                                GIDMagazynuMM = item.Trn_MagZrdId,
                                XLGIDMM = item.Trn_Gidnumer,

                            };

                            var tmpMM = await PobierzSatus(przyjmijMM);

                            ListaMMDoPrzyjcia.Add(tmpMM);
                        }
                    }

                }

            }
            catch (Exception s)
            {
                var app = Application.Current as App;
                var properties = new Dictionary<string, string>
                {
                    { "conn", "/api/przyjmijMM/GetMmListNag" },
                    { "query", listaMM.ErrorMessage},
                    {"magkod",app.BazaProd },
                    {"user", App.SessionManager.CurrentSession.UserName }
                };

                Crashes.TrackError(s, properties);
                throw;
            }

        }

        public async Task<PrzyjmijMMClass> PobierzSatus(PrzyjmijMMClass mMClass)
        {
            try
            {
                await _connection.CreateTableAsync<Model.RaportListaMM>();


                var wynik = await _connection.QueryAsync<Model.RaportListaMM>("select * from RaportListaMM where XLGIDMM = ? ", mMClass.XLGIDMM);

                if (wynik != null)
                {
                    if (wynik.Count > 0)
                    {
                        var wpis = wynik[0].Sended;

                        mMClass.StatusMM = wpis;
                        return mMClass;
                    }
                    return mMClass;

                }
                else
                {
                    return mMClass;
                }

            }
            catch (Exception)
            {
                return mMClass;
            }
        }


        public static IEnumerable<PrzyjmijMMClass> GetMmkas(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return ListOfTwrOnMM;
            return ListOfTwrOnMM.Where(c => c.twrkod.Contains(searchText.ToUpper()));//, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<int> ReturnGidNumerFromEANMM(string BarCodeMM)
        {
            ServicePrzyjmijMM serviceApi = new ServicePrzyjmijMM();
            return await serviceApi.GetGidNumerFromEANMM(BarCodeMM);
        }


        public async Task GetlistMMElementsAsync(int trnId = 0)
        {
            //if (!string.IsNullOrEmpty(KodEanMM))
            //    trnId = await ReturnGidNumerFromEANMM(KodEanMM);
            try
            {
                var dokumentWithEle = await serviceApi.GetDokMMWithElements(trnId);


                foreach (var ele in dokumentWithEle.Elementy)
                {

                    ListOfTwrOnMM.Add(new PrzyjmijMMClass
                    {
                        id = ele.Tre_Lp.ToString(),
                        twrkod = ele.Twr_Kod,
                        ilosc = ele.Stan_szt,
                        url = FilesHelper.ConvertUrlToOtherSize(ele.Twr_Url, ele.Twr_Kod, FilesHelper.OtherSize.small),
                        nazwa = ele.Twr_Nazwa,
                        symbol = ele.Twr_Symbol,
                        GIDdokumentuMM = dokumentWithEle.Trn_Trnid,
                        nrdokumentuMM = dokumentWithEle.TrN_DokumentObcy,
                        GIDMagazynuMM = dokumentWithEle.Trn_MagZrdId,
                        DatadokumentuMM = dokumentWithEle.DataMM,
                        XLGIDMM = dokumentWithEle.Trn_Gidnumer,
                        cena = ele.Twr_Cena.ToString(),
                        ean = ele.Twr_Ean.ToString()
                    });
                }

            }
            catch (Exception)
            {
                return;
            }
        }



        public async Task<ApiResponse<TwrInfo>> PobierzDaneZKarty(string _twrkod)
        {
            return await serviceApi.GetTwrFromOptima(_twrkod);

        }


    }
}
