using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
     
    public partial class SettingsPage : ContentPage
    {

        //private string _version;
        public SettingsPage()
        {
            InitializeComponent();
            // ZaladujUstawienia();
            var app = Application.Current as App;
            BindingContext = Application.Current;
            if (SprConn())
            {
                GetBaseName();
                cennikClasses = GetCenniki();
                //foreach (var cenniki in _cennikClasses)
                //    pickerlist.Items.Add(cenniki.RodzajCeny);
                if (cennikClasses.Count > 0)
                {
                    pickerlist.ItemsSource = GetCenniki().ToList();
                    pickerlist.SelectedIndex = app.Cennik; 
                }
            }
            sprwersja();
        }

        private async void sprwersja()
        {
            var version = DependencyService.Get<Model.IAppVersionProvider>();
            var versionString = version.AppVersion;
            var bulidVer = version.BuildVersion;
            lbl_appVersion.Text = versionString;
            //_version = bulidVer;

            var AktualnaWersja = await App.TodoManager.GetBuildVer();
            if (bulidVer != AktualnaWersja)
                await DisplayAlert(null, "Używana wersja nie jest aktualna", "OK");
        }
       

        private void SprConn_Clicked(object sender, EventArgs e)
        {
            //GetBaseName();
            // NadajWartosci();
            try
            {
                var app = Application.Current as App;
                var connStr = new SqlConnectionStringBuilder
                {
                    DataSource = app.Serwer,//_serwer,
                    InitialCatalog = app.BazaConf, //_database,
                    UserID = app.User,//_uid,
                    Password = app.Password, //_pwd,
                    ConnectTimeout = 1
                }.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        GetBaseName();
                        // GetCenniki().Clear();
                        //cennikClasses.Clear();
                        //cennikClasses = GetCenniki();
                        //pickerlist.ItemsSource = GetCenniki().ToList();
                        DisplayAlert("Connected", "Połączono z siecia", "OK");
                        Navigation.PopModalAsync();

                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Uwaga", "NIE Połączono z siecia : "+ex.Message, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Uwaga", ex.Message, "OK");
            }
           // ZapiszUstawienia();
        }


        

            //public static string _serwer;
            //public static string _database;
            //public static string _uid;
            //public static string _pwd;

            //private void ZapiszUstawienia()
            //{
            //    Xamarin.Forms.Application.Current.Properties["serwer"] = _serwer;
            //    Xamarin.Forms.Application.Current.Properties["database"] = _database;
            //    Xamarin.Forms.Application.Current.Properties["uid"] = _uid;
            //    Xamarin.Forms.Application.Current.Properties["pwd"] = _pwd;
            //    Xamarin.Forms.Application.Current.SavePropertiesAsync();
            //}

            //public void ZaladujUstawienia()
            //{

            //    if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("serwer"))
            //        Xamarin.Forms.Application.Current.Properties.Add("serwer", _serwer);

            //    if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("database"))
            //        Xamarin.Forms.Application.Current.Properties.Add("database", _database);

            //    if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("uid"))
            //        Xamarin.Forms.Application.Current.Properties.Add("uid", _uid);

            //    if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("pwd"))
            //        Xamarin.Forms.Application.Current.Properties.Add("pwd", _pwd);

            //    _serwer = Xamarin.Forms.Application.Current.Properties["serwer"] as string;
            //    _database = Xamarin.Forms.Application.Current.Properties["database"] as string;
            //    _uid = Xamarin.Forms.Application.Current.Properties["uid"] as string;
            //    _pwd = Xamarin.Forms.Application.Current.Properties["pwd"] as string;

            //    if (_serwer != null && _database != null && _uid != null && _pwd != null)
            //    {
            //        NadajWartosci();
            //    }
            //}
            #region nadanie Wartosci old
            public void NadajWartosci()
        {


            //-------------------------SERWER--------------------------
            //if (Ipserwer.Text != null && _serwer != null)
            //{
            //    _serwer = Ipserwer.Text;
            //}
            //else if (Ipserwer.Text != null && _serwer == null)
            //{
            //    //Ipserwer.Text = _serwer;                
            //    _serwer = Ipserwer.Text;
            //}
            //else if (_serwer != null && Ipserwer.Text == null)
            //{
            //    Ipserwer.Text = _serwer;
            //}

            ////----------------------BAZA-----------------------------
            //if (Instancja.Text != null && _database != null)
            //{
            //    _database = Instancja.Text;
            //}
            //else if (Instancja.Text != null && _database == null)
            //{
            //    _database = Instancja.Text;
            //}
            //else if (Instancja != null && Instancja.Text == null)
            //{
            //    Instancja.Text = _database;
            //}
            ////----------------------LOGIN-----------------------------
            //if (Login.Text != null && _uid != null)
            //{
            //    _uid = Login.Text;
            //}
            //else if (Login.Text != null && _uid == null)
            //{
            //    _uid = Login.Text;
            //}
            //else if (_uid != null && Login.Text == null)
            //{
            //    Login.Text = _uid;
            //}
            ////----------------------PWD-----------------------------
            //if (Haslo.Text != null && _pwd != null)
            //{
            //    _pwd = Haslo.Text;
            //}
            //else if (Haslo.Text != null && _pwd == null)
            //{
            //    _pwd = Haslo.Text;
            //}
            //else if (_pwd != null && Haslo.Text == null)
            //{
            //    Haslo.Text = _pwd;
            //}


            //_database = (Instancja.Text == null) ? "cdnxl_joart" : Instancja.Text;
            //_uid= (Login.Text == null) ? "sa" : Login.Text;  
            //_pwd= (Haslo.Text == null) ? "sqlSQL123#" : Haslo.Text;

        }
        #endregion 
        public static bool SprConn() //Third way, slightly slower than Method 1
        {
            // NadajWartosci();
            var app = Application.Current as App;
            var connStr = new SqlConnectionStringBuilder
            {
                DataSource = app.Serwer,//_serwer,
                InitialCatalog = app.BazaProd, //_database,
                UserID = app.User,//_uid,
                Password = app.Password, //_pwd,
                ConnectTimeout = 1
            }.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    //DisplayAlert("Connected", "Połączono z siecia", "OK");
                    return true;
                }
                catch (Exception )
                {
                    //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
                    return false;
                }
            }

            
        }

        public bool GetBaseName()
        {
           
            var app = Application.Current as App;
             
            try
            {
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = "SERVER=" + app.Serwer +
                    ";DATABASE=" + app.BazaConf +
                    ";TRUSTED_CONNECTION=No;UID=" + app.User +
                    ";PWD=" + app.Password
                };
                connection.Open();
                command.CommandText = "SELECT [Baz_NazwaBazy] nazwaBazy FROM " + app.BazaConf + ".[CDN].[Bazy]";


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                if (rs.Read())
                {
                    string bazaProd = Convert.ToString(rs["nazwaBazy"]);
                    app.BazaProd = bazaProd;
                    BazaProd.Text = bazaProd;
                }

                rs.Close();
                rs.Dispose();
                connection.Close();
            }
            catch
            {
                DisplayAlert("Uwaga", "Błąd połączenia..Sprawdź dane", "OK");
            }
            return true;
        
        }

            List<CennikClass> lista;
        private IList<CennikClass> GetCenniki()
        {
            try
            {
                var app = Application.Current as App;
                string nazwaCennika;
                int IdCennik;
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = "SERVER=" + app.Serwer +
                    ";DATABASE=" + app.BazaProd +
                    ";TRUSTED_CONNECTION=No;UID=" + app.User +
                    ";PWD=" + app.Password
                };
                connection.Open();
                command.CommandText = " SELECT DfC_lp, DfC_Nazwa  FROM [CDN].[DefCeny] where DfC_Nieaktywna = 0";

                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                lista = new List<CennikClass>();
                lista.Clear();
                while (rs.Read())
                {
                    IdCennik = Convert.ToInt32(rs["DfC_lp"]);

                    nazwaCennika = Convert.ToString(rs["DfC_Nazwa"]);
                    lista.Add(new CennikClass { Id = IdCennik, RodzajCeny = nazwaCennika });
                }

                rs.Close();
                rs.Dispose();
                connection.Close();
            }
            catch
            {
                DisplayAlert("Uwaga", "Błąd połączenia..Sprawdź dane", "OK");
            }

            return lista;
            
        }

        public IList<CennikClass> cennikClasses;

        private  void pickerlist_OnChanged(object sender, EventArgs e)
        {
            //pickerlist.ItemsSource = GetCenniki().ToList();
            var app = Application.Current as App;
            
            
                var cennik = pickerlist.Items[pickerlist.SelectedIndex];
                var idceny = GetCenniki().Single(cc => cc.RodzajCeny == cennik);
            
           
                int selectedIndex = pickerlist.SelectedIndex;
                if (selectedIndex != -1)
                {
                    //var app = Application.Current as App;
                   // app.Cennik = idceny.Id;
                }

                var picker = (Picker)sender;
                //int selectedIndex = picker.SelectedIndex;

                if (selectedIndex != -1)
                {
                    app.Cennik = selectedIndex; //idceny.Id;
                }
            

        }

        private void pickerlist_Focused(object sender, FocusEventArgs e)
        {
            if (!SprConn())
            {
                pickerlist.Title = "Brak połączenia z bazą";
            }
            else
            {
                //   cennikClasses = GetCenniki();
                //GetCenniki().Clear();
                //pickerlist.ItemsSource = GetCenniki().ToList();
            }
        }
    }

    public class CennikClass
    {
        public int Id { get; set; }
        public string RodzajCeny { get; set; }
    } 
}