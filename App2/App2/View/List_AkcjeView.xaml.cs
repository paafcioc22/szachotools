using App2.Model;
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
    public partial class List_AkcjeView : ContentPage
    {
        public IList<Model.AkcjeNagElem> Items { get; set; }
 
        public static string TypAkcji; 
        bool _istapped;
        private string connectionstring;

        public List_AkcjeView()
        {
            InitializeComponent();

            var app = Application.Current as App;
            connectionstring = $@"SERVER= {app.Serwer}
                ;DATABASE={app.BazaProd}
                ;TRUSTED_CONNECTION=No;UID={ app.User}
                ;PWD={app.Password}";
            GetAkcje();

            //StworzListe();
        }
        

        public Magazynn GetMagnumer()
        {
            Magazynn mag= new Magazynn(); 
            try
            {

                if (SettingsPage.SprConn())
                {

                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        string querystring = $@"SELECT  [Mag_GIDNumer], Mag_Symbol
                                  FROM  [CDN].[Magazyny]
                                  where mag_typ=1 
								  and [Mag_GIDNumer] is not null
								  and mag_nieaktywny=0";

                        using (SqlCommand command2 = new SqlCommand(querystring, connection))
                        {
                            using (SqlDataReader rs = command2.ExecuteReader())
                            {
                                while (rs.Read())
                                {
                                    mag= new Magazynn
                                    {
                                        Id = System.Convert.ToInt16(rs["Mag_GIDNumer"]),
                                        MagKod = rs["Mag_Symbol"].ToString(),
                                    };
                                    
                                }
                            }
                        }
                    }

                }
                return mag;
            }
            catch (Exception)
            {

                DisplayAlert(null, "Błąd pobierania- wejdź w ustawienia\n kliknij zapisz sprawdź połączenie", "OK");
                return mag;

            }
        }

        private async void GetAkcje()
        {
            string user = LoginLista._user;
            int dodajDni = user == "ADM" ? 50 : 0;
            int magnr = GetMagnumer().Id; 

            try
            {
                if(magnr==0)
                    throw new InvalidOperationException("Błąd pobierania danych");

                if (StartPage.CheckInternetConnection())
                {
                    string Webquery = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidTyp  
                    ,AkN_GidNazwa +'' (''+cast( count(distinct Ake_AknNumer)as varchar)+'')'' AkN_GidNazwa ,   max(AkN_DataStart) as ddd 
                    from cdn.pc_akcjeNag INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                    where (AkN_GidNazwa=''przecena'' and AkN_DataKoniec>=GETDATE() -10-{dodajDni} ) or
					(AkN_GidNazwa<>''przecena'' and AkN_DataKoniec>=GETDATE() -5-{dodajDni} )
                    and  (AkN_MagDcl like ''%m={magnr},%'' or AkN_MagDcl=''wszystkie'')
                    and getdate() >= AkN_DataStart
                    group by AkN_GidTyp  , AkN_GidNazwa
                    order by ddd desc
                    '";
                    var twrdane = await App.TodoManager.GetGidAkcjeAsync(Webquery);
                    if (twrdane != null) 
                    { 
                    
                        Items = twrdane;
                        MyListView.ItemsSource = twrdane;
                    }
                    
                }
                else {
                    MyListView.IsVisible = false;
                    notFoundFrame.IsVisible = !MyListView.IsVisible;
                }

            }
            catch (Exception x)
            {
                MyListView.IsVisible =false;
                notFoundFrame.IsVisible = !MyListView.IsVisible;
            
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;


            if (_istapped)
                return;

            _istapped = true;

            var pozycja = e.Item as Model.AkcjeNagElem;
       
                await Navigation.PushAsync(new View.List_AkcjeElemView(pozycja.AkN_GidTyp));

                TypAkcji = pozycja.AkN_GidNazwa;

            _istapped = false;


            ((ListView)sender).SelectedItem = null;
        }

        
    }
}
