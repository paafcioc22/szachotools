using App2.Model;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
 
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class SettingsPage : TabbedPage
    {

        public static bool IsBuforOff;
        public static bool CzyCenaPierwsza;
        public static SByte SelectedDeviceType;
        public static ISewooXamarinCPCL _cpclPrinter;
        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);
        public static CPCLConst cpclConst;

        private BindableProperty IsSearchingProperty =
            BindableProperty.Create("IsSearching", typeof(bool), typeof(SettingsPage), false);
        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }



        //private string _version;
        public SettingsPage()
        {
            InitializeComponent();
            InitializeSampleUI();

            //  _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            //GetDevices();
            SelectDeviceMetod();

            //  cpclConst = new CPCLConst(); 
 
            var app = Application.Current as App;
            BindingContext = Application.Current;


            if (SprConn())
            {
                GetBaseName();
                GetGidnumer();

                cennikClasses = GetCenniki();
                if (cennikClasses != null)//cennikClasses.Count > 0 || 
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
            catch (Exception)
            {
                await DisplayAlert(null, "Nie można sprawdzić aktualnej wersji", "OK");
            }
        }



        public static ObservableCollection<DrukarkaClass> listaDrukarek;
    
        public static bool CzyDrukarkaOn = false; 


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
                        if ( GetBaseName().Result)
                        {
                            DisplayAlert("Sukces..", "Połączono z bazą "+app.BazaProd, "OK");
                            Application.Current.SavePropertiesAsync();
                            Navigation.PopAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Uwaga", "Nie połączono z bazą - sprawdź urządzenia i spróbuj ponownie..", "OK");
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
                ConnectTimeout = 3
            }.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    //DisplayAlert("Connected", "Połączono z siecia", "OK");
                    return true;
                }
                catch  
                {
                    //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
                    //string aa=x.Message;
                    return false;
                }
            }


        }


        void GetGidnumer()
        {

            var app = Application.Current as App;

            var ConnectionString = "SERVER=" + app.Serwer +
                    ";DATABASE=" + app.BazaProd +
                    ";TRUSTED_CONNECTION=No;UID=" + app.User +
                    ";PWD=" + app.Password;




            var stringquery2 = $@"SELECT  [Mag_GIDNumer]
                                  FROM  [CDN].[Magazyny]
                                  where mag_typ=1 
								  and [Mag_GIDNumer] is not null
								  and mag_nieaktywny=0";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(stringquery2, connection))
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        app.MagGidNumer = System.Convert.ToInt16(reader["Mag_GIDNumer"]);
                    }
                }
            }

        }

        public async Task<bool> GetBaseName()
        {

            var app = Application.Current as App;

            try
            {
                 

                string ConnectionString = "SERVER=" + app.Serwer +
                    ";DATABASE=" + app.BazaConf +
                    ";TRUSTED_CONNECTION=No;UID=" + app.User +
                    ";PWD=" + app.Password;
                 

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                { 
                    connection.Open();

                    string stringquery = $@"SELECT top 1 [Baz_NazwaBazy] nazwaBazy 
                                        FROM {app.BazaConf}.[CDN].[Bazy] order by [Baz_TS_Arch] desc ";

                    using (SqlCommand command = new SqlCommand(stringquery, connection))
                    using (SqlDataReader rs = command.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            string bazaProd = Convert.ToString(rs["nazwaBazy"]);
                            app.BazaProd = bazaProd;
                            BazaProd.Text = bazaProd;
                        }
                    }  
                } 
                 
                 
                return true;
            }
            catch (Exception s)
            {
                await DisplayAlert("Uwaga", "Błąd połączenia..Sprawdź dane i/lub spróbuj ponownie" , "OK");
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

        private void pickerlist_OnChanged(object sender, EventArgs e)
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


        public Picker PickerDrukarki { get { return pickerlist; } }
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
         //   var appp = Application.Current as App;
            //var nazwa = PrinterList.Items[PrinterList.SelectedIndex];
            //var wybrana = listaDrukarek.Single(c => c.NazwaDrukarki == nazwa);

         //   int selectedIndex = PrinterList.SelectedIndex;
            //appp.Drukarka = selectedIndex; <<<<<<<<<<<<<<<<<<<<<<<<<<<smiana aaaaa

         //   var printer = PrinterList.Items[PrinterList.SelectedIndex];
         //   var wybrana = listaDrukarek.Single(c => c.NazwaDrukarki + "\r\n" + c.AdresDrukarki == printer);

            //btnConnectClicked(wybrana);

        }

        private async void Btn_ConToWiFi_Clicked(object sender, EventArgs e)
        {
            IsSearching = true;
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


            string wifi = "Szachownica";//JOART_WiFi

            var version = DependencyService.Get<Model.IWifiConnector>();
            if (version.IsConnectedToWifi(wifi))//Szachownica
            {
                await DisplayAlert("Info", "Połączenie zostało już nawiązane..", "OK");
                return;
            }


            if (version.ConnectToWifi(wifi, "J0@rt11a"))
            {
                await DisplayAlert("Info", "Połączenie z Wifi nawiązane pomyślnie.", "OK");
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie udało się połączyć z Wifi..", "OK");
            }

            

            //if (version.IsConnectedToWifi(wifi))
            //{
            //    DisplayAlert("Info", "Połączenie z Wifi nawiązane pomyślnie.", "OK");

            //}
            //else
            //{

            //    DisplayAlert("Uwaga", "Nie udało się połączyć z Wifi..", "OK");

            //}
            IsSearching = false;

        }

        private void SelectDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

            var appp = Application.Current as App;

            int selectedIndex = SelectDevice.SelectedIndex;
            appp.Skanowanie = (SByte)selectedIndex;


            //SelectedDeviceType = (SByte)SelectDevice.SelectedIndex;
            SelectedDeviceType = appp.Skanowanie;

        }



        // private ISewooXamarinCPCL _cpclPrinter;
        // CPCLConst cpclConst;
        private Picker mediaTypePicker;

        public static Editor editAddress;
        private Button connectButton;
        private Button disconnectButton;
        private Button printText;
        private IEnumerable<object> deviceList;


        //private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);




        private async void InitializeSampleUI()
        {
            //            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);

            var appp = Application.Current as App;

            printText = this.FindByName<Button>("btnPrintText");
            connectButton = this.FindByName<Button>("btnConnect");
            disconnectButton = this.FindByName<Button>("btnDisconnect");
            editAddress = this.FindByName<Editor>("txtAddress");
            //mediaTypePicker = this.FindByName<Picker>("cboMediaType");
            //mediaTypePicker.SelectedIndex = 0; // Gap label default.

            if (!CzyDrukarkaOn)
            {
                connectButton.IsEnabled = true;
                printText.IsEnabled = false;
                disconnectButton.IsEnabled = false;
            }
            else
            {
                connectButton.IsEnabled = false;
                printText.IsEnabled = true;
                disconnectButton.IsEnabled = true;


            }
             
            List<connectableDeviceInfo> listdevice = new List<connectableDeviceInfo>(); 
            //todo : odremuj
            //var blueToothService = DependencyService.Get<Model.IBlueToothService>();
             
            //var deviceList = await _cpclPrinter.connectableDevice(); 

            //if(deviceList !=null)
            //{

            //    listdevice = deviceList.Where(c => c.Name.StartsWith("SW")).ToList();
            //    connectableDeviceView.ItemsSource = listdevice;// deviceList.Where(c => c.Name.StartsWith("SW"));

            //}
          
            if (listdevice == null|| listdevice.Count==0)
            {
                editAddress.IsEnabled = true;
                editAddress.Text = "00:00:00:00:00:00";
                btnConnect.IsEnabled = false;
            }
            else
            {
                editAddress.IsEnabled = false;
                // editAddress.Text = deviceList.Where(c => c.Name.StartsWith("SW")).ElementAt(0).Address;
                // editAddress.Text = deviceList.ElementAt(0).Address;
                if (appp.Drukarka == "00:00:00:00:00:00")
                { 
                    editAddress.Text = "";//deviceList.Where(c => c.Name.StartsWith("SW")).ElementAt(0).Address;
                    btnConnect.IsEnabled = false;
                }

                else
                    editAddress.Text = appp.Drukarka;


            }

            //try
            //{

            //    int iResult;
            //    iResult = await _cpclPrinter.printStatus();
            //    if (iResult == cpclConst.LK_SUCCESS)
            //        btnConnect.IsEnabled = false;
            //}
            //catch (Exception)
            //{
            //    btnConnect.IsEnabled = true;
            //}

           cpclConst = new CPCLConst();



        }


        async void btnConnectClicked(object sender, EventArgs args)
        {
            int iResult;
            await printSemaphore.WaitAsync();
            try
            {
                iResult = await _cpclPrinter.connect(editAddress.Text);
                Debug.WriteLine("connect = " + iResult);
                if (iResult == cpclConst.LK_SUCCESS)
                {
                    connectableDeviceView.IsEnabled = false;
                    connectButton.IsEnabled = false;
                    disconnectButton.IsEnabled = true;
                    printText.IsEnabled = true;
                    CzyDrukarkaOn = true;
                    await DisplayAlert("Sukces..", "Połączenie udane", "OK");

                }
                else
                {
                    await DisplayAlert("Niestety..", "Nie nawiązano połączenia", "OK");
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

        async void btnDisconnectClicked(object sender, EventArgs args)
        {
            await printSemaphore.WaitAsync();
            try
            {
                await _cpclPrinter.disconnect();

                connectableDeviceView.IsEnabled = true;
                connectButton.IsEnabled = true;
                disconnectButton.IsEnabled = false;
                printText.IsEnabled = false;
                CzyDrukarkaOn = false;
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

        async void btnPrintTextClicked(object sender, EventArgs args)
        {
            await printSemaphore.WaitAsync();
            try
            {
                int iResult;

                iResult = await _cpclPrinter.printerCheck();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await _cpclPrinter.printStatus();
                        Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        Debug.WriteLine("printerCheck = " + iResult);
                        break;
                }

                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Printer error", iResult);
                    return;
                }

                // Korean
                // await _cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_KOREAN);
                // Japanese
                // await _cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_KOREAN);

                await _cpclPrinter.setForm(0, 200, 200, 406, 1);
                await _cpclPrinter.setMedia(0);

                // Set the code page of font number(7) 
                await _cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);

                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 0, 1, 1, "FONT-0-0", 0);
                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 1, 100, 50, "Test drukarki ąćżół", 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 1, 1, 50, "FONT-0-1", 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 1, 200, 50, "FONT-7-1", 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 1, 100, "FONT-4-0", 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 1, 1, 150, "FONT-4-1", 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 2, 1, 260, "4-2", 0);

                await _cpclPrinter.printForm();

                iResult = await _cpclPrinter.printResults();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await _cpclPrinter.printStatus();
                        Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        Debug.WriteLine("PrinterResults = " + iResult);
                        break;
                }
                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Printing error", iResult);
                }
                else
                {
                    await DisplayAlert("Drukowanie..", "zakończono sukcesem", "OK");
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

        async void ErrorStatusDisp(string strStatus, int errCode)
        {
            string errMsg = string.Empty;

            if (errCode > 0)
            {
                if ((errCode & cpclConst.LK_STS_CPCL_BATTERY_LOW) > 0)
                    errMsg += "Battery Low\n";
                if ((errCode & cpclConst.LK_STS_CPCL_COVER_OPEN) > 0)
                    errMsg += "Cover Open\n";
                if ((errCode & cpclConst.LK_STS_CPCL_PAPER_EMPTY) > 0)
                    errMsg += "Paper Empty\n";
                if ((errCode & cpclConst.LK_STS_CPCL_POWEROFF) > 0)
                    errMsg += "Power OFF\n";
                if ((errCode & cpclConst.LK_STS_CPCL_TIMEOUT) > 0)
                    errMsg += "Timeout\n";
                await DisplayAlert(strStatus, errMsg, "OK");
            }
            else
            {
                await DisplayAlert(strStatus, "Return value = " + errCode, "OK");
            }
        }


        async void OnConnectableDeviceSelection(object sender, SelectedItemChangedEventArgs e)
        {
            int iResult;
            var appp = Application.Current as App;
            if (e.SelectedItem == null)
                return;

            await printSemaphore.WaitAsync();
            try
            {
                var info = e.SelectedItem as connectableDeviceInfo;

                if (info != null)
                {
                    Debug.WriteLine("Selected device address = " + info.Address.ToString());
                    iResult = await _cpclPrinter.connect(info.Address.ToString());
                    editAddress.Text = info.Address.ToString();
                    if (iResult == cpclConst.LK_SUCCESS)
                    {
                        connectButton.IsEnabled = false;
                        disconnectButton.IsEnabled = true;

                        connectableDeviceView.IsEnabled = false;
                        appp.Drukarka = editAddress.Text;
                        printText.IsEnabled = true;
                        CzyDrukarkaOn = true;
                        await DisplayAlert("Sukces..", "Połączenie udane", "OK");

                    }
                    else
                    {
                        await DisplayAlert("Niestety..", "Nie nawiązano połączenia", "OK");
                        View.SettingsPage.CzyDrukarkaOn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                await DisplayAlert("Exception", ex.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }
        }

        private void rodzajCeny_Toggled(object sender, ToggledEventArgs e)
        {
            CzyCenaPierwsza = rodzajCeny.IsToggled;

            var aaa = Application.Current as App;

            aaa.CzyCena1 = rodzajCeny.IsToggled;
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