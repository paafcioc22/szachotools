using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List_AkcjeTwrList : ContentPage
    {
        public IEnumerable<Model.AkcjeNagElem> TwrListWeb { get; set; }
        public ObservableCollection<Model.AkcjeNagElem> TwrListLocal { get; set; }
        //public ObservableCollection<Model.AkcjeGrupy> GroupLista { get; set; }
        public IEnumerable<Model.AkcjeNagElem> SumaList { get; set; }
        SqlConnection connection;
        private SQLiteAsyncConnection _connection;

        public List_AkcjeTwrList( List<Model.AkcjeNagElem> nagElem)
        {
            InitializeComponent();
            BindingContext = this;

            var app = Application.Current as App;
            connection = new SqlConnection
            {
                ConnectionString = "SERVER=" + app.Serwer +
                ";DATABASE=" + app.BazaProd +
                ";TRUSTED_CONNECTION=No;UID=" + app.User +
                ";PWD=" + app.Password
            };
            _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
            //_connection.DropTableAsync<Model.AkcjeNagElem>();
            GetListFromLocal( nagElem);
            GetTwrListFromWeb(nagElem[0].AkN_GidNumer);
        }


        bool _istapped;
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

                await Navigation.PushModalAsync(new List_AkcjeAfterFiltr(nowa));

            }
                _istapped = false;
          
             
            ((ListView)sender).SelectedItem = null;
        }


        private async void GetListFromLocal(List<Model.AkcjeNagElem> lista)
        {
            TwrListLocal = new ObservableCollection<Model.AkcjeNagElem>();
            SumaList = new ObservableCollection<Model.AkcjeNagElem>();
            SqlCommand command = new SqlCommand();

            connection.Open();
            
            if (SettingsPage.SprConn())
            {
                foreach (var _lista in lista)
                {
                    try
                    {
                        command.CommandText = $@"Select twr_gidnumer,twr_kod, twr_nazwa
                    ,cast(sum(TwZ_Ilosc) as int)ilosc,   g.twg_kod grupa
                    from cdn.towary 
                    join cdn.TwrGrupy t on twr_gidnumer = t.TwG_GIDNumer
                    join cdn.TwrGrupy g on t.TwG_GrONumer = g.TwG_GIDNumer and g.twg_gidtyp=-16 
                    join cdn.TwrZasoby on Twr_twrid = TwZ_TwrId 
                    where {_lista.Ake_FiltrSQL} and g.twg_gronumer=0
                    group by twr_gidnumer,twr_kod, twr_nazwa, g.twg_kod";


                        SqlCommand query = new SqlCommand(command.CommandText, connection);
                        SqlDataReader rs;
                        rs = query.ExecuteReader();
                        while (rs.Read())
                        {
                            TwrListLocal.Add(new Model.AkcjeNagElem()
                            {
                                TwrKod = Convert.ToString(rs["twr_kod"]),
                                TwrGidNumer = Convert.ToInt32(rs["twr_gidnumer"]),
                                TwrNazwa = Convert.ToString(rs["twr_nazwa"]),
                                TwrGrupa = Convert.ToString(rs["grupa"]),
                                TwrStan = Convert.ToInt32(rs["ilosc"]) 
                            });

                            // DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
                        }

                        rs.Close();
                        rs.Dispose();
                        

                    }
                    catch (Exception exception)
                    {
                        await  DisplayAlert("Uwaga", exception.Message, "OK");
                    }
                }
                connection.Close();
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
            }

        }

        private async void GetTwrListFromWeb(int _gidNumer)
        {
            try
            {
                string Webquery3 = $@"cdn.PC_WykonajSelect N'declare @filtrSQL varchar(max), @query nvarchar(max)
set @filtrSQL =(select (select   Ake_filtrsql+ '' or '' from cdn.pc_akcjeelem 
where ake_aknnumer={_gidNumer}  For XML PATH ('''')) )

set @query=''select twr_kod TwrKod, twr_ean TwrEan ,twr_katalog TwrSymbol,Twr_GidNumer TwrGidNumer,case when len(twr_kod) > 5 and len(twr_url)> 5 then
                         replace(twr_url, substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4),
                        substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4) + ''''Miniatury/'''') else twr_kod end as  TwrUrl,  twr_nazwa TwrNazwa ,case left(twr_wartosc2,1) when 1 then ''''Damski_'''' 
when 2 then ''''Meski_''''  
when 3 then ''''Dzieciak_''''
when 4 then ''''Dzieciak_''''
when 5 then ''''Akcesoria_'''' 
when 6 then ''''Bielizna_''''end +Twg_kod as TwrDep, twg_kod TwrGrupa
,cast(cd.twc_wartosc as decimal(5,2))TwrCena
,cast(c1.twc_wartosc as decimal(5,2))TwrCena1 ,(select top 1 ake_aknnumer from cdn.pc_akcjeelem where ake_aknnumer=25  )AkN_GidNumer 
from cdn.TwrKarty
 INNER JOIN  CDN.TwrGrupyDom ON Twr_GIDTyp = TGD_GIDTyp AND Twr_GIDNumer = TGD_GIDNumer 
INNER JOIN  CDN.TwrGrupy ON TGD_GrOTyp = TwG_GIDTyp AND TGD_GrONumer = TwG_GIDNumer
join cdn.TwrCeny cd on Twr_gidnumer = cd.TwC_Twrnumer and cd.TwC_TwrLp = 2  
left join cdn.TwrCeny c1 on Twr_gidnumer = c1.TwC_Twrnumer and c1.TwC_TwrLp = 3  
where ''+  left(replace(@filtrSQL,''&#x0D;'',''''),len(replace(@filtrSQL,''&#x0D;'',''''))-3) exec sp_executesql @query'";

                var twrdane = await App.TodoManager.GetGidAkcjeAsync(Webquery3);

                TwrListWeb = twrdane.GroupBy(dd => dd.TwrGrupa).Select(a => a.First()).ToList();

                SumaList = (TwrListLocal.Concat(twrdane)).GroupBy(g => g.TwrGidNumer).SelectMany(c => c.Select(cs => new Model.AkcjeNagElem
                {
                    TwrGidNumer = cs.TwrGidNumer,
                    TwrKod = cs.TwrKod,
                    TwrStan = c.Sum(cc => cc.TwrStan),
                    TwrNazwa = cs.TwrNazwa,
                    TwrGrupa = cs.TwrGrupa,
                    TwrDep = string.Concat("", cs.TwrDep),
                    TwrUrl = string.Concat("", cs.TwrUrl),
                    TwrSymbol = cs.TwrSymbol,
                    TwrSkan = cs.TwrSkan,
                    TwrCena = string.Concat("", cs.TwrCena),
                    TwrCena1 = cs.TwrCena1,
                    TwrEan = string.Concat("", cs.TwrEan),
                    AkN_GidNumer = cs.AkN_GidNumer
                })).GroupBy(p => new { p.TwrGidNumer, p.TwrStan }).Select(f => f.Last()).Where(x => x.TwrDep != "" && x.TwrStan > 0).ToList();






                MyListView3.ItemsSource = SumaList.GroupBy(dd => dd.TwrGrupa).Select(a => a.First()).ToList();
                var nowa = SumaList.GroupBy(g => g.TwrDep).SelectMany(s => s.Select(cs => new Model.AkcjeNagElem
                {
                    TwrGrupa = cs.TwrGrupa,
                    TwrSkan = s.Sum(cc => cc.TwrSkan),
                    TwrStan = s.Sum(cc => cc.TwrStan),
                    TwrStanVsSKan = cs.TwrStanVsSKan,
                    TwrDep = cs.TwrDep,
                    AkN_GidNumer = cs.AkN_GidNumer
                })).GroupBy(p => new { p.TwrDep }).Select(f => f.First()).OrderByDescending(x => x.TwrStan).ToList();


                var sorted = from towar in nowa
                             orderby towar.TwrDep.Replace(towar.TwrGrupa, "")
                             group towar by towar.TwrDep.Replace(towar.TwrGrupa, "") into monkeyGroup
                             select new Model.AkcjeGrupy<string, Model.AkcjeNagElem>(monkeyGroup.Key, monkeyGroup);


                MyListView3.ItemsSource = sorted;
                // SumaList.GroupBy(dd => dd.TwrGrupa).Select(g => new { TwrGrupa = g.TwrGrupa }); 
            }
            catch (Exception x)
            {

                await DisplayAlert(null, x.Message, "OK");
            }
        }

       

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Model.AkcjeNagElem>();

            var SavedList = await _connection.Table<Model.AkcjeNagElem>().ToListAsync();

            var nowa = SumaList.GroupBy(g => g.TwrDep).SelectMany(s => s.Select(cs => new Model.AkcjeNagElem
            {
                TwrGrupa = cs.TwrGrupa,
                TwrSkan = s.Sum(cc => cc.TwrSkan),
                TwrStan = s.Sum(cc => cc.TwrStan),
                TwrStanVsSKan = cs.TwrStanVsSKan,
                TwrDep = cs.TwrDep
            })).GroupBy(p => new { p.TwrDep }).Select(f => f.First()).OrderByDescending(x => x.TwrStan);

            var sorted = from monkey in nowa
                         orderby monkey.TwrDep.Replace(monkey.TwrGrupa, "")
                         group monkey by monkey.TwrDep.Replace(monkey.TwrGrupa, "") into monkeyGroup
                         select new Model.AkcjeGrupy<string, Model.AkcjeNagElem>(monkeyGroup.Key, monkeyGroup);


            MyListView3.ItemsSource = sorted;
            base.OnAppearing();



        }

        

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           // var sss = sender.GetType();
            var stack = (StackLayout)sender;
            var label = (Label)stack.Children[0];

             string dep = label.Text;

            if (_istapped)
                return;

            _istapped = true;

                var nowa = SumaList.Where(x => x.TwrDep.Replace(x.TwrGrupa, "") == dep).OrderByDescending(x => x.TwrStan - x.TwrSkan).ToList();

                await Navigation.PushModalAsync(new List_AkcjeAfterFiltr(nowa));

            _istapped = false;

        }

         
    }
}
