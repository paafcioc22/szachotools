using App2.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaportListaRoznice : ContentPage
    {

        Model.RaportRoznicGrupa braki_grupa;
        Model.RaportRoznicGrupa nadstany_grupa;

        SQLiteAsyncConnection _connection;
        // List<Model.RaportRoznicGrupa> listDiffrence = new List<Model.RaportRoznicGrupa>();
        ObservableCollection<Model.RaportRoznicGrupa> listDiffrence2;
        static ObservableCollection<Model.ListDiffrence> lista;
        //List<ListDiffrence> listaRoznic;  


         

        static private int _gidnumer;
        static private int _XLgidnumer;
        static private int _MagGidnumer;
        static private string  _DataDokumentu;

        private string _nrdokumentu;

        public RaportListaRoznice(int gidnumer)
        {
            InitializeComponent();
            _gidnumer = gidnumer;
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection(); 

            //  _connection.CreateTableAsync<Model.RaportListaMM>();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

          //  await _connection.CreateTableAsync<Model.RaportListaMM>();
            MyListView.ItemsSource= await OdswiezListe();
        }
        #region Przelicz-Niepotrzebne

        async void przelicz()
        {
            Model.ListDiffrence.listDiffrences.Clear();


            var listaZMM = Model.PrzyjmijMMLista.przyjmijMMListas.Where(n => n.GIDdokumentuMM == _gidnumer);

            foreach (var ww in listaZMM)
            {
                Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
                {
                    twrkod = ww.twrkod,
                    ilosc = ww.ilosc * -1,
                    typ = "MM",
                    IleZMM = ww.ilosc,
                    NrDokumentu = ww.nrdokumentuMM,
                    GidMagazynu = ww.GIDMagazynuMM,
                    DataDokumentu = ww.DatadokumentuMM,
                    TwrNazwa = ww.nazwa,
                    XLGidMM = ww.XLGIDMM
                });
                _nrdokumentu = ww.nrdokumentuMM;
            }

            var listaZskanowania = await _connection.QueryAsync<Model.RaportListaMM>(@"select RaportListaMM.twrkod, RaportListaMM.nazwa,
                RaportListaMM.ilosc_OK , RaportListaMM.nrdokumentuMM, RaportListaMM.DatadokumentuMM
                from RaportListaMM 
                where RaportListaMM.GIDdokumentuMM = ? ", _gidnumer);

            foreach (var ww in listaZskanowania)
            {
                Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
                {
                    twrkod = ww.twrkod,
                    ilosc = ww.ilosc_OK,
                    typ = "Skan",
                    IleZeSkan = ww.ilosc_OK,
                    NrDokumentu = ww.nrdokumentuMM,
                    DataDokumentu = ww.DatadokumentuMM,
                    GidMagazynu = ww.GIDMagazynuMM,
                    TwrNazwa = ww.nazwa
                });
            }

            var lista = Model.ListDiffrence.listDiffrences;


            var nowa2 = lista.GroupBy(g => g.twrkod).
                SelectMany(c => c.Select(
                    csline => new Model.ListDiffrence
                    {
                        twrkod = csline.twrkod,
                        IleZeSkan = c.Sum(x => x.IleZeSkan),
                        IleZMM = csline.IleZMM,
                        ilosc = c.Sum(cc => cc.ilosc),
                        NrDokumentu = csline.NrDokumentu,
                        GidMagazynu = csline.GidMagazynu,
                        DataDokumentu = csline.DataDokumentu,
                        TwrNazwa = csline.TwrNazwa
                    })).GroupBy(p => new
                    {
                        p.twrkod,
                        p.ilosc // modyfikacja
                                //p.IleZMM,
                                //p.IleZeSkan
                    }).Select(g => g.First()).Where(f => f.ilosc != 0).OrderBy(x => x.twrkod);


            //OdswiezListe(nowa2, _nrdokumentu);
            // OdswiezListe(nowa2, _nrdokumentu);

        } 
        #endregion

        private async Task<ObservableCollection<RaportRoznicGrupa>> OdswiezListe()
        {
            lista = new ObservableCollection<ListDiffrence>();
            lista.Clear();
            Model.ListDiffrence.listDiffrences.Clear();
            var listaZMM = Model.PrzyjmijMMLista.przyjmijMMListas.Where(n => n.GIDdokumentuMM == _gidnumer);

            foreach (var ww in listaZMM)
            {
                Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
                {
                    twrkod = ww.twrkod,
                    ilosc = ww.ilosc * -1,
                    typ = "MM",
                    IleZMM = ww.ilosc,
                    NrDokumentu = ww.nrdokumentuMM,
                    GidMagazynu = ww.GIDMagazynuMM,
                    DataDokumentu = ww.DatadokumentuMM,
                    TwrNazwa = ww.nazwa,
                    GidDokumentu= ww.GIDdokumentuMM,
                    Operator = View.LoginLista._nazwisko,
                    XLGidMM = ww.XLGIDMM
                });
                _nrdokumentu = ww.nrdokumentuMM;
                _MagGidnumer = ww.GIDMagazynuMM;
                _DataDokumentu = ww.DatadokumentuMM;
                _XLgidnumer = ww.XLGIDMM;
            }

            var listaZskanowania = await _connection.QueryAsync<Model.RaportListaMM>(@"select RaportListaMM.twrkod, RaportListaMM.nazwa,
                RaportListaMM.ilosc_OK , RaportListaMM.nrdokumentuMM, RaportListaMM.DatadokumentuMM
                from RaportListaMM 
                where RaportListaMM.GIDdokumentuMM = ? ", _gidnumer);

            foreach (var ww in listaZskanowania)
            {
                Model.ListDiffrence.listDiffrences.Add(new Model.ListDiffrence
                {
                    twrkod = ww.twrkod,
                    ilosc = ww.ilosc_OK,
                    typ = "Skan",
                    IleZeSkan = ww.ilosc_OK,
                    NrDokumentu = ww.nrdokumentuMM,
                    DataDokumentu = ww.DatadokumentuMM,
                    GidMagazynu = _MagGidnumer,
                    TwrNazwa = ww.nazwa,
                    GidDokumentu = _gidnumer,
                    Operator = View.LoginLista._nazwisko,
                    XLGidMM = _XLgidnumer
                });
            }

            var lista2 = Model.ListDiffrence.listDiffrences;


            var nowa2 = lista2.GroupBy(g => g.twrkod).
                SelectMany(c => c.Select(
                    csline => new Model.ListDiffrence
                    {
                        twrkod = csline.twrkod,
                        IleZeSkan = c.Sum(x => x.IleZeSkan),
                        IleZMM = csline.IleZMM,
                        ilosc = c.Sum(cc => cc.ilosc),
                        NrDokumentu = csline.NrDokumentu,
                        GidMagazynu = csline.GidMagazynu,
                        DataDokumentu = csline.DataDokumentu,
                        TwrNazwa = csline.TwrNazwa,
                        Operator = csline.Operator,
                        XLGidMM = csline.XLGidMM,
                         
                    })).GroupBy(p => new
                    {
                        p.twrkod,
                        p.ilosc // modyfikacja
                                //p.IleZMM,
                                //p.IleZeSkan
                    }).Select(g => g.First()).Where(f => f.ilosc != 0).OrderBy(x => x.twrkod);


            listDiffrence2 =new ObservableCollection<Model.RaportRoznicGrupa>();
            listDiffrence2.Clear();//

            lista = Convert(nowa2.OrderByDescending(x => x.ilosc));

          
            lbl_NrDokumentu.Text = "Raport różnic :" + _nrdokumentu;

            var ile_brak = lista.Where(x => x.ilosc < 0).Count();
            var ile_nadstan = lista.Where(x => x.ilosc > 0).Count();

            if (ile_brak > 0)
            {
                braki_grupa = new Model.RaportRoznicGrupa("BRAKI", Color.Red);
                listDiffrence2.Add(braki_grupa);//
            }
            if (ile_nadstan > 0)
            {
                nadstany_grupa = new Model.RaportRoznicGrupa("NADSTANY", Color.Green);
                listDiffrence2.Add(nadstany_grupa);//
            }
             

            if (ile_brak > 0 || ile_nadstan > 0)
            {
                foreach (var el in lista)
                {
                    if (el.ilosc < 0)
                    {
                        Model.ListDiffrence element = new Model.ListDiffrence();
                        element.twrkod = el.twrkod;
                        element.NrDokumentu = el.NrDokumentu;
                        element.ilosc = el.ilosc;
                        element.GidMagazynu = el.GidMagazynu;
                        element.DataDokumentu = el.DataDokumentu;
                        element.IleZeSkan = el.IleZeSkan;
                        element.IleZMM = el.IleZMM;
                        element.TwrNazwa = el.TwrNazwa;
                        // element.typ = el.IleZMM.ToString() + " vs " + el.IleZeSkan.ToString();
                        braki_grupa.Add(element);
                    }
                    else
                    {
                        Model.ListDiffrence element = new Model.ListDiffrence();
                        element.twrkod = el.twrkod;
                        element.NrDokumentu = el.NrDokumentu;
                        element.GidMagazynu = el.GidMagazynu;
                        element.DataDokumentu = el.DataDokumentu;
                        element.ilosc = el.ilosc;
                        element.IleZeSkan = el.IleZeSkan;
                        element.IleZMM = el.IleZMM;
                        element.TwrNazwa = el.TwrNazwa;

                        //element.typ = el.IleZMM.ToString() + " vs " + el.IleZeSkan.ToString();
                        nadstany_grupa.Add(element);
                    }
                }

                listDiffrence2.OrderBy(n => n.Select(x => x.ilosc));//

            }


            if (ile_brak == 0 && ile_nadstan == 0)
            {
                //lbl_komunikat.Text = "Pełna zgodność dostawy- brawo";
                nadstany_grupa = new Model.RaportRoznicGrupa("Pełna zgodność dostawy", Color.LightGreen);
                listDiffrence2.Add(nadstany_grupa);//
                //Model.ListDiffrence element = new Model.ListDiffrence();
                //element.twrkod = "Brawo";
                //nadstany_grupa.Add(element);
           //     MyListView.ItemsSource = listDiffrence2;//lista; }//

            }
            else
            {
               // MyListView.ItemsSource = listDiffrence2;//lista; }//
            }


            return listDiffrence2;
        }

        

        public ObservableCollection<T> Convert<T>(IOrderedEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var dane = e.Item as Model.ListDiffrence;
                //Model.ListDiffrence;
            //var dane = lista.Where(x => x.twrkod == kod.twrkod) as Model.ListDiffrence;
            //await DisplayAlert("Item Tapped",$"kod:{dane.twrkod}, MM:{dane.IleZMM}, skan:{dane.IleZeSkan}" , "OK");
            await Navigation.PushModalAsync(new View.RaportListaDetails(dane));
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void Btn_SendRaport_Clicked(object sender, EventArgs e)
        {




            var action = await DisplayAlert(null, "Czy chcesz wysłać raport?", "Tak", "Nie");
            if (action)
            {
                if (lista.Count != 0)
                {
                    var Maglista = await App.TodoManager.InsertDataNiezgodnosci(lista);
                    if (Maglista.ToString() == "OK")
                    {
                        await DisplayAlert("Info", "Raport został wysłany pomyślnie.", "OK");

                        var wynik = await _connection.QueryAsync<Model.RaportListaMM>("select * from RaportListaMM where XLGIDMM = ? ", _XLgidnumer);

                        //var wpis = wynik[0];

                        foreach (var wpis in wynik)
                        {
                            wpis.Sended = true;
                            await _connection.UpdateAsync(wpis);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", "Raport został już wysłany.", "OK");
                        //await Navigation.PopToRootAsync();

                    }
                }
                else
                {
                    lista.Add(new Model.ListDiffrence
                    {
                        twrkod = "Lista zgodna",
                        IleZeSkan = 0,
                        IleZMM = 0,
                        ilosc = 0,
                        NrDokumentu = _nrdokumentu,
                        GidMagazynu = _MagGidnumer,
                        DataDokumentu = _DataDokumentu,
                        Operator = View.LoginLista._nazwisko,
                        XLGidMM = _XLgidnumer
                    });

                    var Maglista = await App.TodoManager.InsertDataNiezgodnosci(lista);
                    if (Maglista.ToString() == "OK")
                    {
                        await DisplayAlert("Info", "Raport został wysłany pomyślnie.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Uwaga", "Raport został już wysłany.", "OK");
                    }
                }
            }
        }
    }
}
