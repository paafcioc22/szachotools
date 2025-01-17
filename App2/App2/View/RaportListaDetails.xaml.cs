﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaportListaDetails : ContentPage
    {
        SQLiteAsyncConnection _connection;
        Model.ListDiffrence _towar;

        private int _gidnumer;

        public  RaportListaDetails(Model.ListDiffrence dane)
        {
            InitializeComponent();
            _towar = dane;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            _connection.CreateTableAsync<Model.RaportListaMM>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await pobierzdane();
        }

        private async Task pobierzdane()
        {
            try
            {
                Model.PrzyjmijMMClass daneZkartylista = new Model.PrzyjmijMMClass();

                if (_towar.twrkod != null)
                {
                    var daneZkarty = await daneZkartylista.PobierzDaneZKarty(_towar.twrkod);

                    if (daneZkarty.IsSuccessful)
                    {
                        img_foto.Source = daneZkarty.Data.Twr_Url;
                        lbl_twrcena.Text = daneZkarty.Data.Twr_Cena.ToString();
                        lbl_twrnazwa.Text = daneZkarty.Data.Twr_Nazwa;
                        lbl_twrsymbol.Text = daneZkarty.Data.Twr_Symbol;
                        lbl_twrean.Text = daneZkarty.Data.Twr_Ean;
                    }
                    else
                    {
                        string Webquery = "cdn.pc_pobierztwr '" + _towar.twrkod + "'";
                        var twrdane = await App.TodoManager.PobierzTwrAsync(Webquery);

                        if (twrdane != null)
                        {
                            img_foto.Source = twrdane.Twr_Url;
                            lbl_twrcena.Text = twrdane.Twr_Cena.ToString();
                            lbl_twrnazwa.Text = twrdane.Twr_Nazwa;
                            lbl_twrsymbol.Text = twrdane.Twr_Symbol;
                            lbl_twrean.Text = twrdane.Twr_Ean;
                        }

                    }

                    lbl_twr_kod.Text = _towar.twrkod;

                    lbl_ileZMM.Text = _towar.IleZMM.ToString();
                    entry_ileZeSkan.Text = _towar.IleZeSkan.ToString();
                }
            }
            catch (Exception s)
            {
                await DisplayAlert("Błąd", s.Message, "OK");
            }

           
        }

        private async void Btn_save_Clicked(object sender, EventArgs e)
        {
            Model.ListDiffrence.listDiffrences.Clear();
            var wynik = await _connection.QueryAsync<Model.RaportListaMM>("select * from RaportListaMM where nrdokumentuMM = ? and twrkod=?", _towar.NrDokumentu, _towar.twrkod);
            int ilosc;
            if (wynik.Count > 0)
            {
                var wpis = wynik[0];

                var tryConv = Int32.TryParse(entry_ileZeSkan.Text, out ilosc);
                if (tryConv)
                {
                    wpis.ilosc_OK = ilosc;
                    _gidnumer = wynik[0].GIDdokumentuMM;
                    await _connection.UpdateAsync(wpis);

                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert(null, "Wpisana ilość nie jest liczbą lub przekracza zakres", "OK");
                }
               
            }
           

        }

        #region niekatywne przlicz
        //async void przelicz()
        //{
        //    Model.ListDiffrence.listDiffrences.Clear();
        //    var listaZMM = Model.PrzyjmijMMLista.przyjmijMMListas.Where(n => n.GIDdokumentuMM == _gidnumer);

        //    foreach (var ww in listaZMM)
        //    {
        //        Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
        //        {
        //            twrkod = ww.twrkod,
        //            ilosc = ww.ilosc * -1,
        //            typ = "MM",
        //            IleZMM = ww.ilosc,
        //            NrDokumentu = ww.nrdokumentuMM,
        //            GidMagazynu = ww.GIDMagazynuMM,
        //            DataDokumentu = ww.DatadokumentuMM,
        //            TwrNazwa = ww.nazwa
        //        });
        //    }

        //    var listaZskanowania = await _connection.QueryAsync<Model.RaportListaMM>(@"select RaportListaMM.twrkod, RaportListaMM.nazwa,
        //        RaportListaMM.ilosc_OK , RaportListaMM.nrdokumentuMM, RaportListaMM.DatadokumentuMM
        //        from RaportListaMM 
        //        where RaportListaMM.GIDdokumentuMM = ? ", _gidnumer);

        //    foreach (var ww in listaZskanowania)
        //    {
        //        Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
        //        {
        //            twrkod = ww.twrkod,
        //            ilosc = ww.ilosc_OK,
        //            typ = "Skan",
        //            IleZeSkan = ww.ilosc_OK,
        //            NrDokumentu = ww.nrdokumentuMM,
        //            DataDokumentu = ww.DatadokumentuMM,
        //            GidMagazynu = ww.GIDMagazynuMM,
        //            TwrNazwa = ww.nazwa
        //        });
        //    }

        //    var lista = Model.ListDiffrence.listDiffrences;

        //    var nowa2 = lista.GroupBy(g => g.twrkod).
        //        SelectMany(c => c.Select(
        //            csline => new Model.ListDiffrence
        //            {
        //                twrkod = csline.twrkod,
        //                IleZeSkan = c.Sum(x => x.IleZeSkan),
        //                IleZMM = csline.IleZMM,
        //                ilosc = c.Sum(cc => cc.ilosc),
        //                NrDokumentu = csline.NrDokumentu,
        //                GidMagazynu = csline.GidMagazynu,
        //                DataDokumentu = csline.DataDokumentu,
        //                TwrNazwa = csline.TwrNazwa
        //            })).ToList().GroupBy(p => new
        //            {
        //                p.twrkod,
        //                p.ilosc // modyfikacja
        //                        //p.IleZMM,
        //                        //p.IleZeSkan
        //            }).Select(g => g.First()).Where(f => f.ilosc != 0).OrderBy(x => x.twrkod);

        //    // nowa2.Where(n => n.ilosc != 0).ToList();
        //    await Navigation.PushModalAsync(new View.RaportListaRoznice(nowa2, listaZMM));

        //    //Model.PrzyjmijMMLista.przyjmijMMListas.Clear();
        //    Model.ListDiffrence.listDiffrences.Clear();
        //} 
        #endregion

        private void entry_ileZeSkan_Focused(object sender, FocusEventArgs e)
        {
            if (_towar.IleZeSkan == 0)
            {
                DisplayAlert("Uwaga", "Zeskanuj model lub pozostaw 0 jeśli brak", "OK");
                entry_ileZeSkan.IsEnabled = false;
                btn_save.IsEnabled = false;
               // Navigation.PopModalAsync();
            } 

        }
    }
}