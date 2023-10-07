using App2.Model;
using App2.Model.ApiModel;
using Newtonsoft.Json;
using RestSharp;
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
    public partial class List_AkcjeElemView : ContentPage
    {
        public ObservableCollection<NagElem> ListaZFiltrem  { get; set; }
        private static RestClient _client;
        public IList<NagElem> Items2 { get; set; }
        App app;

        private BindableProperty IsSearchingProperty =
           BindableProperty.Create("IsSearching", typeof(bool), typeof(List_AkcjeElemView), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        bool _istapped;
 
        static string Sklep;
        public List_AkcjeElemView( int gidtyp)
        {
            InitializeComponent();
            ListaZFiltrem = new ObservableCollection<NagElem>();
            BindingContext = this;
            GetAkcje( gidtyp);

            app = Application.Current as App;
           

        }


        private async Task<string> GetFiltrSQLstring(int ake_numer)
        {

            var query = $@" cdn.PC_WykonajSelect N'declare @filtrSQL varchar(max), @query nvarchar(max)
            set @filtrSQL = (select(select   Ake_filtrsql + '' or '' from cdn.pc_akcjeelem
            where ake_aknnumer = {ake_numer} For XML PATH('''')))
            select @filtrSQL as FiltrSQL '";

            var filtrLista = await App.TodoManager.PobierzDaneZWeb<FiltrSQLs>(query);

            if (filtrLista.Count > 0)
            {
                return filtrLista[0].FiltrSQL.Substring(0,filtrLista[0].FiltrSQL.Length-3);
            }
            return "";

        }

        private async Task<bool> IsAnyItemFromLocal(string filtrSQL)
        {
            var listFromFiltr= ExtractCode.ExtractCodes(filtrSQL);
            
            int ileKodow = 0;
          

            if (await SettingsPage.SprConn())
            {
                
                try
                {
                    var karta = new TwrKodRequest()
                    {
                        Twrkody = listFromFiltr
                    };

                    var request = new RestRequest("/api/gettowarycnt", Method.Post)
                                        .AddJsonBody(karta);

                    var response = await _client.ExecuteAsync(request);
                    if (response.IsSuccessful)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                        ileKodow = jsonResponse.result;
                        return true;

                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception exception)
                {
                    await DisplayAlert("Uwaga", exception.Message, "OK");
                }
                 
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
            }
            return false;

        }

        private async void GetAkcje(int _gidtyp)
        {
            IsSearching = true;
            IList<NagElem> tmp= new List<NagElem>(); 

            List_AkcjeView akcjeView = new List_AkcjeView();
            var magInfo = await akcjeView.GetMagnumer();
            int magnr= magInfo.Id;
            Sklep = magInfo.MagKod;

            if (magnr != 0)
            {
                 
                try
                {
                    if (StartPage.CheckInternetConnection())
                    {
                        string user = LoginLista._user;
                        int dodajDni = user == "ADM" ? 50 : 0;

                        string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidNumer, AkN_GidTyp  
                        , AkN_GidNazwa ,AkN_NazwaAkcji, AkN_DataStart,AkN_DataKoniec,Ake_FiltrSQL,IsSendData
                        from cdn.pc_akcjeNag 
                        INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                        join [CDN].[PC_AkcjeTyp] on akn_gidtyp = GidTypAkcji
                         where AkN_GidTyp={_gidtyp} and AkN_DataKoniec>=GETDATE() -10 -{dodajDni}
                            and  (AkN_MagDcl like ''%m={magnr},%'' or AkN_MagDcl=''wszystkie'')
                            and getdate() >= AkN_DataStart
                         order by AkN_DataStart desc
                         '";

                        //var AkcjeElemLista = await App.TodoManager.GetGidAkcjeAsync<NagElem>(Webquery2);
                        var AkcjeElemLista = await App.TodoManager.PobierzDaneZWeb<NagElem>(Webquery2);
                        Items2 = AkcjeElemLista;

                        //Items2 = AkcjeElemLista.GroupBy(dd => dd.AkN_GidNumer).Select(a => a.FirstOrDefault()).ToList();
                        //Items2 = AkcjeElemLista.GroupBy(dd => dd.AkN_GidNumer).Select(g => g.OrderBy(x => x.AkN_GidNumer).Where(s=>s.AkN_GidNumer !=0).FirstOrDefault()).ToList();

                        //ListaZFiltrem = AkcjeElemLista;

                        foreach (var item in AkcjeElemLista)
                        {
                            if (item.AkN_GidNazwa != "Foto Relacja")
                            {
                                var have = await IsAnyItemFromLocal(item.Ake_FiltrSQL);
                                if (have)
                                {
                                    tmp.Add(item);
                                }
                            }
                            else 
                            {
                                tmp.Add(item);
                            }
                        }  

                        foreach (var i in tmp.GroupBy(dd => dd.AkN_GidNumer).Select(g => g.OrderBy(x => x.AkN_GidNumer).Where(s => s.AkN_GidNumer != 0).FirstOrDefault()))
                        {
                            ListaZFiltrem.Add(i);
                        }

                        if (ListaZFiltrem.Count == 0)
                        {
                            notFoundFrame.IsVisible = true;
                            lista.IsVisible = false;
                        }

                        //MyListView2.ItemsSource = tmp; 
                    }
                }
                catch (Exception x)
                {
                    await DisplayAlert(null, x.Message, "OK");
                }
            }

            IsSearching = false;
        }

        async void  Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            if (_istapped)
                return;

            _istapped = true;

                // pozycja = e.Item as Model.AkcjeNagElem;
                var pozycja = e.Item as Model.NagElem;

                var nowa =Items2.Where(x => x.AkN_GidNumer == pozycja.AkN_GidNumer).ToList();

            if(pozycja.AkN_GidNazwa!="Foto Relacja")
            {
                await    Navigation.PushAsync(new List_AkcjeTwrList(nowa));
            }
            else
            {
                //await Navigation.PushAsync(new Foto.Foto2(nowa.FirstOrDefault(),Sklep, true));
                await Navigation.PushAsync(new Foto.FotoTest(nowa.FirstOrDefault(),Sklep, true));
            }

            _istapped = false;

            // await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
