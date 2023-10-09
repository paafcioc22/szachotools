using App2.Model;
using App2.Model.ApiModel;
using App2.OptimaAPI;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List_AkcjeTwrList : ContentPage
    {
        public IList<AkcjeNagElem> TwrListWeb { get; set; }
        public ObservableCollection<AkcjeNagElem> TwrListLocal { get; set; } 
        public IList<AkcjeNagElem> SumaList { get; set; }

        private SQLiteAsyncConnection _connection; 
        private List<NagElem> _nagElem;
        private ObservableCollection<Model.AkcjeNagElem> _fromWeb;
        App app;
        Int16 magnumer;         
        int NrCennika=2;
        bool _istapped;

        private BindableProperty IsSearchingProperty =
           BindableProperty.Create("IsSearching", typeof(bool), typeof(List_AkcjeElemView), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

         

        public List_AkcjeTwrList(List<NagElem> nagElem)
        {
            InitializeComponent();
            BindingContext = this;

            app = Application.Current as App;
            
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            //_connection.DropTableAsync<Model.AkcjeNagElem>();
            _nagElem = nagElem;
            LoadList();
            //GetListFromLocal( nagElem);
            //GetTwrListFromWeb(nagElem[0].AkN_GidNumer);
        }

        private async void LoadList()
        {
            IsSearching = true;

            try
            {
                if (StartPage.CheckInternetConnection())
                {
                    TwrListWeb = await GetTwrListFromWeb(_nagElem[0].AkN_GidNumer);
                    GetListFromLocal(_nagElem);

                    await _connection.CreateTableAsync<AkcjeNagElem>();

                    var SavedList = await _connection.Table<AkcjeNagElem>().ToListAsync(); 

                    SumaList = (
                     from lWeb in TwrListWeb
                     join local in TwrListLocal on lWeb.TwrGidNumer equals local.TwrGidNumer
                     join skan in SavedList on
                               new { lWeb.TwrGidNumer, lWeb.AkN_GidNumer } equals new { skan.TwrGidNumer, skan.AkN_GidNumer } into a
                     from alles in a.DefaultIfEmpty(new AkcjeNagElem { TwrSkan = 0 })
                     select new AkcjeNagElem
                     {
                         AkN_GidNumer = lWeb.AkN_GidNumer,
                         TwrGidNumer = lWeb.TwrGidNumer,
                         TwrKod = lWeb.TwrKod,
                         TwrGrupa = lWeb.TwrGrupa,
                         TwrDep = lWeb.TwrDep,
                         TwrStan = local.TwrStan,
                         TwrSkan = alles.TwrSkan,
                         TwrCena = lWeb.TwrCena,
                         TwrCena1 = lWeb.TwrCena1,
                         TwrEan = lWeb.TwrEan,
                         TwrNazwa = lWeb.TwrNazwa,
                         TwrSymbol = lWeb.TwrSymbol,
                         TwrUrl = lWeb.TwrUrl,
                         IsSendData = lWeb.IsSendData
                     }).ToList();


                    var isSendData = TwrListWeb[0].IsSendData;

                    if (isSendData && View.LoginLista._user != "ADM")
                        //  if (StartPage.CheckInternetConnection())
                        SendDataSkan(SumaList);///wysyłka z listy z grupowania 

                    var nowa = SumaList.GroupBy(g => g.TwrDep).SelectMany(s => s.Select(cs => new Model.AkcjeNagElem
                    {
                        TwrGrupa = cs.TwrGrupa,
                        TwrSkan = s.Sum(cc => cc.TwrSkan),
                        TwrStan = s.Sum(cc => cc.TwrStan),
                        TwrStanVsSKan = cs.TwrStanVsSKan,
                        TwrDep = cs.TwrDep,
                        IsSendData=cs.IsSendData
                    })).GroupBy(p => new { p.TwrDep }).Select(f => f.First()).OrderByDescending(x => x.TwrStan);


                    if (nowa != null)
                    {
                        var sorted = from towar in nowa
                                     orderby towar.TwrDep.Replace(towar.TwrGrupa, "")
                                     group towar by towar.TwrDep.Replace(towar.TwrGrupa, "") into monkeyGroup
                                     select new Model.AkcjeGrupy<string, Model.AkcjeNagElem>(monkeyGroup.Key, monkeyGroup);

                        MyListView3.ItemsSource = sorted;
                    } 
                }


            }
            catch (Exception s)
            {
               await DisplayAlert(null, s.Message, "OK");
            }

            IsSearching = false;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (_istapped)
            {
                return;
            }
            else
            {
                _istapped = true;

                var nazwa = e.Item as Model.AkcjeNagElem;

                var nowa = SumaList.Where(x => x.TwrDep == nazwa.TwrDep).OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();

                await Navigation.PushAsync(new List_AkcjeAfterFiltr(nowa));

            }
            _istapped = false;


            ((ListView)sender).SelectedItem = null;
        }

        private async void GetListFromLocal(List<Model.NagElem> lista)
        {
            TwrListLocal = new ObservableCollection<Model.AkcjeNagElem>();
            //SumaList = new ObservableCollection<Model.AkcjeNagElem>();
            ServicePrzyjmijMM api = new ServicePrzyjmijMM();

            if (await SettingsPage.SprConn())
            {

                foreach (var _lista in lista)
                {
                    try
                    {
                        var twrList = await api.GetTowaryGrupyListAsync(_lista.Ake_FiltrSQL);

                        if(twrList.IsSuccessful)
                        {
                            foreach (var twr in twrList.Data)
                            {
                                TwrListLocal.Add(new Model.AkcjeNagElem()
                                {
                                    TwrKod = twr.Twr_Kod,
                                    TwrGidNumer = twr.Twr_Gidnumer,
                                    TwrNazwa = twr.Twr_Nazwa,
                                    TwrGrupa = twr.Twr_Grupa,
                                    TwrStan = twr.Stan_szt
                                });                                
                            }
                        }
                        else
                        {
                            await DisplayAlert("Uwaga", twrList.ErrorMessage, "OK");
                        }

                    }
                    catch (Exception exception)
                    {
                        await DisplayAlert("Uwaga", exception.Message, "OK");
                    }
                }
             
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
            }

        }

        private async Task<ObservableCollection<Model.AkcjeNagElem>> GetTwrListFromWeb(int _gidNumer)
        {
            var app = Application.Current as App;
            if (!string.IsNullOrEmpty(app.Cennik))
            { 
                if (app.Cennik == "OUTLET")
                    NrCennika = 4;
                else NrCennika = 2; 
               
            } 
                try
                {

                    if (StartPage.CheckInternetConnection())
                    {
                        TwrListWeb = new ObservableCollection<Model.AkcjeNagElem>();

                        string Webquery3 = $@"cdn.PC_WykonajSelect N'declare @filtrSQL varchar(max), @query nvarchar(max)
                    set @filtrSQL =(select (select   Ake_filtrsql+ '' or '' from cdn.pc_akcjeelem 
                    where ake_aknnumer={_gidNumer}  For XML PATH ('''')) )

                    set @query=''select twr_kod TwrKod, twr_ean TwrEan ,twr_katalog TwrSymbol,Twr_GidNumer TwrGidNumer,CDN.PC_GetTwrUrl(twr_kod)
											  as TwrUrl,  twr_nazwa TwrNazwa ,case left(twr_wartosc2,1) when 1 then ''''Damski_'''' 
                    when 2 then ''''Meski_''''  
                    when 3 then ''''Dzieciak_''''
                    when 4 then ''''Dzieciak_''''
                    when 5 then ''''Akcesoria_'''' 
                    when 6 then ''''Bielizna_''''
                    when 7 then ''''Buty_''''
                    end +Twg_kod as TwrDep, twg_kod TwrGrupa
                    ,cast(cd.twc_wartosc as decimal(5,2))TwrCena
                    ,cast(c1.twc_wartosc as decimal(5,2))TwrCena1 ,(select top 1 ake_aknnumer from cdn.pc_akcjeelem where ake_aknnumer={_gidNumer} )AkN_GidNumer 
                    ,(select top 1 [IsSendData]  from [CDN].[PC_AkcjeTyp] where GidTypAkcji={_nagElem[0].AkN_GidTyp} )IsSendData 
                    from cdn.TwrKarty
                    INNER JOIN  CDN.TwrGrupyDom ON Twr_GIDTyp = TGD_GIDTyp AND Twr_GIDNumer = TGD_GIDNumer 
                    INNER JOIN  CDN.TwrGrupy ON TGD_GrOTyp = TwG_GIDTyp AND TGD_GrONumer = TwG_GIDNumer
                    join cdn.TwrCeny cd on Twr_gidnumer = cd.TwC_Twrnumer and cd.TwC_TwrLp =   {NrCennika } -- 2  
                    left join cdn.TwrCeny c1 on Twr_gidnumer = c1.TwC_Twrnumer and c1.TwC_TwrLp = 3  
                    where ''+  left(replace(@filtrSQL,''&#x0D;'',''''),len(replace(@filtrSQL,''&#x0D;'',''''))-3) exec sp_executesql @query'";

                        _fromWeb = await App.TodoManager.GetGidAkcjeAsync(Webquery3);

                    }
                    return _fromWeb;
                }
                catch (Exception s)
                {

                    //throw;
                    await DisplayAlert(null, s.Message, "OK");
                    return null;
                }
                    #region MyRegion
                //var listFromWeb = await App.TodoManager.GetGidAkcjeAsync(Webquery3);

                // TwrListWeb = listFromWeb;//.GroupBy(dd => dd.TwrGrupa).Select(a => a.First()).ToList();

                //await _connection.CreateTableAsync<Model.AkcjeNagElem>();

                //var listFromSkan = await _connection.Table<Model.AkcjeNagElem>().ToListAsync();


                //var nowa2 = TwrListLocal.Concat(twrdane).GroupBy(g => g.TwrGidNumer).SelectMany(c => c.Select(cs => new Model.AkcjeNagElem
                //{
                //    TwrGidNumer = cs.TwrGidNumer,
                //    TwrKod = cs.TwrKod,
                //    TwrStan = cs.TwrStan,
                //    TwrNazwa = cs.TwrNazwa,
                //    TwrGrupa = cs.TwrGrupa,
                //    TwrDep = string.Concat("", cs.TwrDep),
                //    TwrUrl = string.Concat("", cs.TwrUrl),
                //    TwrSymbol = cs.TwrSymbol,
                //    TwrSkan = c.Sum(cc => cc.TwrSkan),
                //    TwrCena = string.Concat("", cs.TwrCena),
                //    TwrCena1 = cs.TwrCena1,
                //    TwrEan = string.Concat("", cs.TwrEan),
                //    AkN_GidNumer = cs.AkN_GidNumer

                //})).GroupBy(p => new { p.TwrGidNumer, p.TwrSkan }).Select(f => f.Last()).Where(x => x.TwrDep != "" && x.TwrStan > 0).ToList();

                //SumaList = (nowa2.Concat(SavedList)).GroupBy(g => g.TwrGidNumer).SelectMany(c => c.Select(cs => new Model.AkcjeNagElem
                //{
                //    TwrGidNumer = cs.TwrGidNumer,
                //    TwrKod = cs.TwrKod,
                //    TwrStan = c.Sum(cc => cc.TwrStan),
                //    TwrNazwa = cs.TwrNazwa,
                //    TwrGrupa = cs.TwrGrupa,
                //    TwrDep = string.Concat("", cs.TwrDep),
                //    TwrUrl = string.Concat("", cs.TwrUrl),
                //    TwrSymbol = cs.TwrSymbol,
                //    TwrSkan = cs.TwrSkan,
                //    TwrCena = string.Concat("", cs.TwrCena),
                //    TwrCena1 = cs.TwrCena1,
                //    TwrEan = string.Concat("", cs.TwrEan),
                //    AkN_GidNumer = cs.AkN_GidNumer
                //})).GroupBy(p => new { p.TwrGidNumer, p.TwrStan }).Select(f => f.Last()).Where(x => x.TwrDep != "" && x.TwrStan > 0).ToList();

                //SumaList = (
                //  from lWeb in listFromWeb
                //  join local in TwrListLocal on lWeb.TwrGidNumer equals local.TwrGidNumer
                //  join skan in listFromSkan on
                //            new { lWeb.TwrGidNumer, lWeb.AkN_GidNumer } equals new { skan.TwrGidNumer, skan.AkN_GidNumer } into a
                //  from alles in a.DefaultIfEmpty(new AkcjeNagElem { TwrSkan = 0 })
                //  select new AkcjeNagElem
                //  {
                //      AkN_GidNumer=lWeb.AkN_GidNumer,
                //      TwrGidNumer=lWeb.TwrGidNumer,
                //      TwrKod=lWeb.TwrKod,
                //      TwrGrupa=lWeb.TwrGrupa,
                //      TwrDep=lWeb.TwrDep,
                //      TwrStan=local.TwrStan,
                //      TwrSkan=alles.TwrSkan,
                //      TwrCena=lWeb.TwrCena,
                //      TwrCena1=lWeb.TwrCena1,
                //      TwrEan=lWeb.TwrEan,
                //      TwrNazwa=lWeb.TwrNazwa,
                //      TwrSymbol=lWeb.TwrSymbol,
                //      TwrUrl=lWeb.TwrUrl,
                //  });


                ////MyListView3.ItemsSource = SumaList.GroupBy(dd => dd.TwrGrupa).Select(a => a.First()).ToList();
                ////MyListView3.ItemsSource = SumaList.GroupBy(dd => dd.TwrGrupa).SelectMany(s => s.Select(cs => new Model.AkcjeNagElem
                ////{
                ////    TwrGrupa = cs.TwrGrupa,
                ////    TwrSkan = s.Sum(cc => cc.TwrSkan),
                ////    TwrStan = s.Sum(cc => cc.TwrStan),
                ////    TwrStanVsSKan = cs.TwrStanVsSKan,

                ////    AkN_GidNumer = cs.AkN_GidNumer
                ////})).GroupBy(p => new { p.TwrGrupa }).Select(f => f.First()).OrderByDescending(x => x.TwrStan).ToList(); 

                //var nowa = SumaList.GroupBy(g => g.TwrDep).SelectMany(s => s.Select(cs => new Model.AkcjeNagElem
                //{
                //    TwrGrupa = cs.TwrGrupa,
                //    TwrSkan = s.Sum(cc => cc.TwrSkan),
                //    TwrStan = s.Sum(cc => cc.TwrStan),
                //    TwrStanVsSKan = cs.TwrStanVsSKan,
                //    TwrDep = cs.TwrDep,
                //    AkN_GidNumer = cs.AkN_GidNumer
                //})).GroupBy(p => new { p.TwrDep }).Select(f => f.First()).OrderByDescending(x => x.TwrStan).ToList();


                //var sorted = from towar in nowa
                //             orderby towar.TwrDep.Replace(towar.TwrGrupa, "")
                //             group towar by towar.TwrDep.Replace(towar.TwrGrupa, "") into monkeyGroup
                //             select new Model.AkcjeGrupy<string, Model.AkcjeNagElem>(monkeyGroup.Key, monkeyGroup);


                // MyListView3.ItemsSource = sorted;
                // SumaList.GroupBy(dd => dd.TwrGrupa).Select(g => new { TwrGrupa = g.TwrGrupa });  
                #endregion


            //});
        }

        public static bool TableExists<T>(SQLiteConnection connection) 
        { 
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?"; 
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name); 
            return cmd.ExecuteScalar<string>() != null; 
        }
        
        protected override async void OnAppearing()
        {

            //TwrListWeb = await GetTwrListFromWeb(_nagElem[0].AkN_GidNumer);
            //GetListFromLocal(_nagElem);

            await _connection.CreateTableAsync<AkcjeNagElem>();

            var SavedList = await _connection.Table<AkcjeNagElem>().ToListAsync();

            var wynik = await _connection.QueryAsync<AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? ", _nagElem[0].AkN_GidNumer);


            //if (TwrListWeb.Count > 0)
            if (TwrListWeb != null )
            {
                 
                if (  TwrListWeb.Count != 0)
                {
                    SumaList = (
                                 from lWeb in TwrListWeb
                                 join local in TwrListLocal on lWeb.TwrGidNumer equals local.TwrGidNumer
                                 join skan in SavedList on
                                           new { lWeb.TwrGidNumer, lWeb.AkN_GidNumer } equals new { skan.TwrGidNumer, skan.AkN_GidNumer } into a
                                 from alles in a.DefaultIfEmpty(new AkcjeNagElem { TwrSkan = 0 })
                                 select new AkcjeNagElem
                                 {
                                     AkN_GidNumer = lWeb.AkN_GidNumer,
                                     TwrGidNumer = lWeb.TwrGidNumer,
                                     TwrKod = lWeb.TwrKod,
                                     TwrGrupa = lWeb.TwrGrupa,
                                     TwrDep = lWeb.TwrDep,
                                     TwrStan = local.TwrStan,
                                     TwrSkan = alles.TwrSkan,
                                     TwrCena = lWeb.TwrCena,
                                     TwrCena1 = lWeb.TwrCena1,
                                     TwrEan = lWeb.TwrEan,
                                     TwrNazwa = lWeb.TwrNazwa,
                                     TwrSymbol = lWeb.TwrSymbol,
                                     TwrUrl = lWeb.TwrUrl,
                                     IsSendData = lWeb.IsSendData,
                                     IsUpdatedData = alles.IsUpdatedData
                                 }).ToList();

                    var isSendData = TwrListWeb[0].IsSendData;

                    var sendOnlyToUpdate = SumaList.Where(c => c.IsUpdatedData == false&& c.TwrSkan>0).ToList();
                    //if (List_AkcjeView.TypAkcji.Contains("Przecena"))
                        if (isSendData && LoginLista._user!="ADM")
                            SendDataSkan(sendOnlyToUpdate);



                    var nowa = SumaList.GroupBy(g => g.TwrDep).SelectMany(s => s.Select(cs => new AkcjeNagElem
                    {
                        TwrGrupa = cs.TwrGrupa,
                        TwrSkan = s.Sum(cc => cc.TwrSkan),
                        TwrStan = s.Sum(cc => cc.TwrStan),
                        TwrStanVsSKan = cs.TwrStanVsSKan,
                        TwrDep = cs.TwrDep,
                        IsSendData = cs.IsSendData
                    })).GroupBy(p => new { p.TwrDep }).Select(f => f.First()).OrderByDescending(x => x.TwrStan);


                    if (nowa != null)
                    {

                        var sorted = from towar in nowa
                                     orderby towar.TwrDep.Replace(towar.TwrGrupa, "")
                                     group towar by towar.TwrDep.Replace(towar.TwrGrupa, "") into monkeyGroup
                                     select new AkcjeGrupy<string, AkcjeNagElem>(monkeyGroup.Key, monkeyGroup);



                        MyListView3.ItemsSource = sorted;

                    } 
                }

            }
            else {
                await DisplayAlert(null, "Błąd pobierania danych", "OK");
            }



            base.OnAppearing();

        }

        private async void SendDataSkan(IList<AkcjeNagElem> sumaList)
        {

            try
            {
                if (await SettingsPage.SprConn())
                {
                    var magGidnumer = (Application.Current as App).MagGidNumer;

                    if (magGidnumer == 0)
                    {
                        ServicePrzyjmijMM api = new ServicePrzyjmijMM();
                        var magazyn = await api.GetSklepMagNumer();
                        magGidnumer = (short)magazyn.Id;
                    }


                    string ase_operator = View.LoginLista._user + " " + View.LoginLista._nazwisko;
                    var odp = await App.TodoManager.InsertDataSkan(sumaList, magGidnumer, ase_operator);
                    if (odp != "OK")
                        await DisplayAlert(null, odp, "OK");
                }
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // var sss = sender.GetType();
            var stack = (AbsoluteLayout)sender;
            var label = (Label)stack.Children[0];

            string dep = label.Text;

            if (_istapped)
                return;

            _istapped = true;

            var nowa = SumaList.Where(x => x.TwrDep.Replace(x.TwrGrupa, "") == dep).OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();

            await Navigation.PushAsync(new List_AkcjeAfterFiltr(nowa));

            _istapped = false;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await _connection.DropTableAsync<Model.AkcjeNagElem>();
        }
        private void ImportSkanFromOther_Clicked(object sender, EventArgs e)
        {
            var magGidnumer = (Application.Current as App).MagGidNumer;
            if (magGidnumer > 0)
            {
                ImportZeskanowychZCentrali(_nagElem[0].AkN_GidNumer, magGidnumer);
            }
        }
        

        private async void ImportZeskanowychZCentrali(int asn_gidnumer, int magnumer)
        {
            var query = @$"cdn.PC_WykonajSelect N'
                    select ASE_AknNumer as AkN_GidNumer, ASE_AknMagNumer as Ake_ElemLp 
                    ,ASE_Grupa  as TwrGrupa                        
                    ,ASE_TwrDep as TwrDep
                    ,ASE_TwrNumer as  TwrGidNumer
                    ,ASE_TwrStan as TwrStan ,ASE_TwrSkan as TwrSkan 
                    FROM CDN.PC_AkcjeSkanElem
              where ASE_AknNumer = {asn_gidnumer} and ASE_AknMagNumer = {magnumer} and ASE_TwrSkan> 0'";

            var zbazy= await App.TodoManager.PobierzDaneZWeb<AkcjeNagElem> (query);


            var test= await _connection.InsertAllAsync(zbazy, true);


            var wynik = await _connection.QueryAsync<AkcjeNagElem>("select * from AkcjeNagElem where AkN_GidNumer = ? ", _nagElem[0].AkN_GidNumer);

        }
    }

}
