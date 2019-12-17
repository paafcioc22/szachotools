using App2.Model;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class SettingsPage : ContentPage
    {
       
        public static bool IsBuforOff;
        public static SByte SelectedDeviceType;
        public static ISewooXamarinCPCL _cpclPrinter;
        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);
        CPCLConst cpclConst;


        //private string _version;
        public SettingsPage()
        {
            InitializeComponent();
            // ZaladujUstawienia();

            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            GetDevices();
            SelectDeviceMetod();

            cpclConst = new CPCLConst();
            var  app = Application.Current as App;
            BindingContext = Application.Current;
            if (SprConn())
            {
                GetBaseName();
                 

                cennikClasses = GetCenniki();
                if (cennikClasses.Count > 0)
                {
                    pickerlist.ItemsSource = GetCenniki().ToList();
                    pickerlist.SelectedIndex = app.Cennik;
                }

            }
  

            sprwersja();
            SwitchStatus.IsToggled = IsBuforOff;
        }

        private void SelectDeviceMetod()
        {
             
            var app = Application.Current as App;
            var lista = new List<MetodaSkanowania>()
            {
                new MetodaSkanowania{Id=1,TypeDevice="Skaner"},
                new MetodaSkanowania{Id=2,TypeDevice="Aparat"}
            };

            foreach (var ss in lista)
            {
                SelectDevice.Items.Add(ss.TypeDevice);
            }

            try
            {
                //PrinterList.ItemsSource = listaDrukarek;

                //SelectDevice.SelectedIndex = SelectedDeviceType;
                SelectDevice.SelectedIndex = app.Skanowanie;
            }
            catch
            {
                SelectDevice.SelectedIndex = -1;
            }

        }

      

        private async void sprwersja()
        {
            try
            {
                var version = DependencyService.Get<Model.IAppVersionProvider>();
                var versionString = version.AppVersion;
                var bulidVer = version.BuildVersion;
                lbl_appVersion.Text = "Wersja " + versionString;
                //_version = bulidVer;

                var AktualnaWersja = await App.TodoManager.GetBuildVer();
                if (bulidVer < Convert.ToInt16(AktualnaWersja))
                    await DisplayAlert(null, "Używana wersja nie jest aktualna", "OK");
            }
            catch (Exception )
            {
                await DisplayAlert(null, "Nie można sprawdzić aktualnej wersji", "OK");
            }
        }

 

        public static ObservableCollection<DrukarkaClass> listaDrukarek;
        async void GetDevices()
        {

            var app = Application.Current as App;
            BindingContext = Application.Current;
            try
            {

                listaDrukarek = new ObservableCollection<DrukarkaClass>();
                listaDrukarek.Clear();
                var list = await _cpclPrinter.connectableDevice();

                if (list.Count > 0)
                {

                    for (int i = 0; i <= list.Count()-1; i++)
                    {
                        listaDrukarek.Add(new DrukarkaClass { Id = i, NazwaDrukarki = list[i].Name, AdresDrukarki = list[i].Address });
                        PrinterList.Items.Add($"{list[i].Name}\r\n{list[i].Address}");
                    }

                    try
                    {
                        //PrinterList.ItemsSource = listaDrukarek;

                        PrinterList.SelectedIndex = app.Drukarka;
                    }
                    catch
                    {
                        PrinterList.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ss)
            {

                System.Diagnostics.Debug.WriteLine(ss.Message);
            }
        }
        public static bool CzyDrukarkaOn= false;
        async void btnConnectClicked(DrukarkaClass drukarkaClass)
        {
            int iResult;
            await printSemaphore.WaitAsync();
            try
            {
                iResult = await _cpclPrinter.connect(drukarkaClass.AdresDrukarki);
                 
                if (iResult == cpclConst.LK_SUCCESS)
                {
                    await DisplayAlert("Connection", "Połaczono z drukarką", "OK");
                    CzyDrukarkaOn = true;

                }
                else
                {
                    await DisplayAlert("Connection", "Błąd połączenia z drukarką", "OK");
                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }

        }

        //private IList<string> _deviceList;
        //public IList<string> DeviceList;


       

        private void SprConn_Clicked(object sender, EventArgs e)
        {
               
         
            try
            {
                 var app = Application.Current as App;
                var connStr = new SqlConnectionStringBuilder
                {
                    DataSource = app.Serwer,//_serwer,
                    InitialCatalog = app.BazaConf, //_database,
                    UserID = app.User,//_uid,
                    Password = app.Password, //_pwd,
                    ConnectTimeout = 2
                }.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        if (GetBaseName())
                        {
                            DisplayAlert("Connected", "Połączono z siecia", "OK");
                            Navigation.PopModalAsync(); 
                        } 

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
                ConnectTimeout =3
            }.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    //DisplayAlert("Connected", "Połączono z siecia", "OK");
                    return true;
                }
                catch (Exception x )
                {
                    //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
                    //string aa=x.Message;
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
                command.CommandText = $@"SELECT top 1 [Baz_NazwaBazy] nazwaBazy 
                                        FROM {app.BazaConf}.[CDN].[Bazy] order by [Baz_TS_Arch] desc ";


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {
                    string bazaProd = Convert.ToString(rs["nazwaBazy"]);
                    app.BazaProd = bazaProd;
                    BazaProd.Text = bazaProd;
                }

                rs.Close();
                rs.Dispose();
                connection.Close();
            return true;
            }
            catch (Exception s)
            {
                DisplayAlert("Uwaga", "Błąd połączenia..Sprawdź dane"+s.Message, "OK");
                return false;
            }
        
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


        public  Picker PickerDrukarki { get { return pickerlist; } }
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

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            IsBuforOff = e.Value;
        }
        
        private void PrinterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var appp = Application.Current as App;
            //var nazwa = PrinterList.Items[PrinterList.SelectedIndex];
            //var wybrana = listaDrukarek.Single(c => c.NazwaDrukarki == nazwa);
             
             int selectedIndex = PrinterList.SelectedIndex;
            appp.Drukarka = selectedIndex;

            var printer = PrinterList.Items[PrinterList.SelectedIndex];
            var wybrana = listaDrukarek.Single(c => c.NazwaDrukarki+ "\r\n"+c.AdresDrukarki == printer);

            //btnConnectClicked(wybrana);

        }

        private  void Btn_ConToWiFi_Clicked(object sender, EventArgs e)
        {

            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi))
            {
                System.Diagnostics.Debug.WriteLine("połączenie wigi");
            }

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("połączenie dostępne");
            }


            string wifi= "Szachownica";//JOART_WiFi

            var version = DependencyService.Get<Model.IWifiConnector>();
            if (version.IsConnectedToWifi(wifi))//Szachownica
            {
                 DisplayAlert("Info", "Połączenie zostało już nawiązane..","OK");
                return;
            } 


            version.ConnectToWifi(wifi, "J0@rt11a");

             Task.Delay(2000);

            if (version.IsConnectedToWifi(wifi))
            {
                 DisplayAlert("Info", "Połączenie z Wifi nawiązane pomyślnie.", "OK");

            }
            else 
            {

                 DisplayAlert("Uwaga", "Nie udało się połączyć z Wifi..", "OK");

            }
             
        }

        private void SelectDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

            var appp = Application.Current as App; 

            int selectedIndex = SelectDevice.SelectedIndex;
            appp.Skanowanie = (SByte)selectedIndex;


            //SelectedDeviceType = (SByte)SelectDevice.SelectedIndex;
            SelectedDeviceType = appp.Skanowanie;

        }
    }

    public class CennikClass
    {
        public int Id { get; set; }
        public string RodzajCeny { get; set; }
    }

    public class DrukarkaClass
    {
        public int Id { get; set; }
        public string NazwaDrukarki { get; set; }
        public string AdresDrukarki { get; set; }
    }

    class MetodaSkanowania
    {
        public int Id { get; set; }
        public string TypeDevice { get; set; }
    }
}