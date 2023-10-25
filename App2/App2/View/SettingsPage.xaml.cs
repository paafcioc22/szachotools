using App2.Model;
using App2.OptimaAPI;
using App2.Services;
using App2.ViewModel;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        public static bool OnAlfaNumeric;
        public static SByte SelectedDeviceType;
        public static ISewooXamarinCPCL _cpclPrinter;
        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);
        public static CPCLConst cpclConst;
        private const string serverIp = "192.168.1.155:5063";
        private static RestClient _client;
        public static ObservableCollection<DrukarkaClass> listaDrukarek;
        //public static List<CennikClass> ListaCen { get; set; }


        public static bool CzyDrukarkaOn = false; 
     
        SettingsViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            InitializeSampleUI();

            //  _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            //GetDevices();
            SelectDeviceMetod();

            var app = Application.Current as App;

            this.BindingContext = viewModel = new SettingsViewModel(app);
            //BindingContext = Application.Current; 


            if (!app.Serwer.Contains("optima"))
            {
                var options = new RestClientOptions($"http://{app.Serwer}")
                {
                    MaxTimeout = 10000 // 10 sekund
                };

                _client = new RestClient(options);
            }
            else
            {
                app.Serwer = app.Serwer.Replace(@"\optima", ":8081");
                viewModel.Serwer = app.Serwer;
            }

            SwitchStatus.IsToggled = IsBuforOff;
            SwitchKlawiatura.IsToggled = OnAlfaNumeric;
        }



        protected async override void OnAppearing()
        {
            var app = Application.Current as App;

            base.OnAppearing();
            isPageActive = true;
            await sprwersja();
            await InicjalizujAsync(app);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isPageActive = false;
        }

        private bool isPageActive = false;
        private async Task InicjalizujAsync(App app)
        {
            try
            {
                bool wynik = await SprConn();

                if (wynik)
                {
                    await GetBaseName(app);
                    await GetGidnumer();

                    viewModel.ListaCen = (await GetCenniki());
                    if (viewModel.ListaCen != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            pickerlist.ItemsSource = viewModel.ListaCen;
                            if (app.Cennik != "0")
                            {
                                var selected = viewModel.ListaCen.FirstOrDefault(s => s.RodzajCeny == app.Cennik);
                                pickerlist.SelectedItem = selected;
                            }
                        });
                    }

                }
            }
            catch (Exception )
            {
                if (isPageActive)
                {
                    DependencyService
                            .Get<IAppVersionProvider>()
                            .ShowShort("Brak połączenia");
                }
            }

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



        private async Task sprwersja()
        {
            var version = DependencyService.Get<IAppVersionProvider>();

            try
            {
                var versionString = version.AppVersion;
                var bulidVer = version.BuildVersion;
                lbl_appVersion.Text = "Wersja " + versionString;
                //_version = bulidVer;

                var AktualnaWersja = await App.TodoManager.GetBuildVer();
                if (bulidVer < Convert.ToInt16(AktualnaWersja))
                    //await DisplayAlert(null, "Używana wersja nie jest aktualna", "OK");
                    version.ShowShort("Używana wersja nie jest aktualna");
            }
            catch (Exception)
            {
                version.ShowShort("Błąd pobierania wersji aplikacji");

            }
        }




        async static Task<bool> SprawdzPolaczenieApi(App app)
        {
         
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(20); // Ustawienie czasu wygaśnięcia na 10 sekund

                var response = await httpClient.GetAsync($"http://{app.Serwer}/api/test");
                var isOk = response.EnsureSuccessStatusCode(); // Rzuci wyjątek, jeśli status code to błąd
                return isOk.IsSuccessStatusCode;
            }
            catch (OperationCanceledException ex)
            {
                // Przechwyć wyjątek, gdy żądanie jest anulowane z powodu przekroczenia czasu
                Debug.WriteLine("Timeout: " + ex.Message);
                throw new Exception("Timeout: upłynął limit czasu żądania");

            }
            catch (HttpRequestException ex)
            {
                // Przechwyć wyjątek, gdy wystąpi problem z żądaniem HTTP
                Debug.WriteLine("Problem z żądaniem HTTP: " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                // Przechwyć inne wyjątki
                Debug.WriteLine("Wystąpił inny błąd: " + ex.Message);
                string newMessage = ex.Message + "\nspróbuj ponownie za chwilę";
                throw new Exception(newMessage, ex);

            }
        }

        private async void SprConn_Clicked(object sender, EventArgs e)
        {

            try
            {
                viewModel.IsBusy = true;
                var app = Application.Current as App;

                Regex regex = new Regex(@"[^0-9.:]");
  
                if (regex.IsMatch(app.Serwer))
                {
                    // Wyświetl komunikat o błędzie, jeśli wprowadzony tekst zawiera niedozwolone znaki
                    throw new Exception("Wprowadzono nieprawidłowy adres IP. Proszę używać tylko cyfr, kropek i dwukropków.");

                }
                else
                {
                                    
                    if (await SprawdzPolaczenieApi(app))
                    {                     

                        var options = new RestClientOptions($"http://{app.Serwer}")
                        {
                            MaxTimeout = 10000 // 10 sekund
                        };

                        _client = new RestClient(options);  // re-inicjalizacja klienta


                        var request = new RestRequest("/api/test")
                        {
                            Timeout = 10000
                        };

                        var response = await _client.GetAsync(request);
                        if (response.IsSuccessful)
                        {
                            var dbNameHeader = response.Headers.FirstOrDefault(h => h.Name == "X-Database-Name");
                            if (dbNameHeader != null)
                            {
                                Console.WriteLine($"X-Database-Name: {dbNameHeader.Value}");
                            }

                            var connOk = response.IsSuccessStatusCode;
                            if (connOk)
                            {

                                var magInfo = await GetGidnumer();
                                app.MagGidNumer = (short)magInfo.Id;

                                app.BazaProd = dbNameHeader.Value.ToString();
                                BazaProd.Text = dbNameHeader.Value.ToString();
                                await DisplayAlert("Sukces..", $"Połączono z bazą {dbNameHeader.Value}", "OK");
                                await UzupełnijCennik();
                                viewModel.IsBusy = false;
                                await Application.Current.SavePropertiesAsync();
                                if (pickerlist.SelectedItem != null)
                                    await Navigation.PopAsync();
                                else
                                    await DisplayAlert(null, "Nie uzupełniono cennika", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Uwaga", "Nie połączono z bazą - sprawdź urządzenia i spróbuj ponownie..", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Uwaga", "Nie połączono z bazą - sprawdź urządzenia i spróbuj ponownie..", "OK");

                        }
                        viewModel.IsBusy = false;
                    }
                } 

            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("Żądanie zostało anulowane: " + ex.Message);
                //await DisplayAlert("Upłynął limit czasu połączenia", ex.Message, "OK");
                

                viewModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Uwaga", ex.Message, "OK");
                viewModel.IsBusy = false;
            }

        }

        async Task UzupełnijCennik()
        {
            try
            {
                var app = Application.Current as App;
                viewModel.ListaCen = (await GetCenniki());
                if (viewModel.ListaCen != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        pickerlist.ItemsSource = viewModel.ListaCen;
                        if (app.Cennik != "0")
                        {
                            var selected = viewModel.ListaCen.FirstOrDefault(s => s.RodzajCeny == app.Cennik);
                            pickerlist.SelectedItem = selected;
                        }
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<bool> SprConn() //Third way, slightly slower than Method 1
        {
            try
            {

                Regex regex = new Regex(@"[^0-9.:]");

                var app = Application.Current as App;
            
                if (regex.IsMatch(app.Serwer))
                {
                    // Wyświetl komunikat o błędzie, jeśli wprowadzony tekst zawiera niedozwolone znaki
                    throw new Exception("Wprowadzono nieprawidłowy adres IP. Proszę używać tylko cyfr, kropek i dwukropków.");
             
                }
                else
                {
                    if (await SprawdzPolaczenieApi(app))
                    {
                        //var options = new RestClientOptions($"http://{app.Serwer}")
                        //{
                        //    MaxTimeout = 10000 // 10 sekund
                        //};
                        //_client = new RestClient(options);  // re-inicjalizacja klienta

                        //var request = new RestRequest("/api/test");
                        //var response = await _client.GetAsync(request);

                        //return response.IsSuccessStatusCode;
                        return true;

                    }
                }                
                return false;

            }
            catch (HttpRequestException a)
            {
                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return false;
            }

        }


        public static async Task<Magazynn> GetGidnumer()
        {
            ServicePrzyjmijMM serviceApi = new ServicePrzyjmijMM();
            return await serviceApi.GetSklepMagNumer();

        }

        public async Task<bool> GetBaseName(App app)
        {

            try
            {
                var request = new RestRequest("/api/test");

                var response = await _client.GetAsync(request);

                var dbNameHeader = response.Headers.FirstOrDefault(h => h.Name == "X-Database-Name");
                if (dbNameHeader != null)
                {
                    Console.WriteLine($"X-Database-Name: {dbNameHeader.Value}");
                    app.BazaProd = dbNameHeader.Value.ToString();
                    return true;
                }
                return false;


            }
            catch (Exception )
            {
                await DisplayAlert("Uwaga", "Błąd połączenia..Sprawdź dane i/lub spróbuj ponownie", "OK");
                return false;
            }

        }


        private async Task<List<CennikClass>> GetCenniki()
        {
            try
            {
                var request = new RestRequest("/api/getCenniki");

                var response = await _client.ExecuteGetAsync<List<CennikClass>>(request);

                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                    return null;

            }
            catch (HttpRequestException a)
            {
                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return null;
            }

        }



        private async void pickerlist_OnChanged(object sender, EventArgs e)
        {
            //pickerlist.ItemsSource = GetCenniki().ToList();
            var app = Application.Current as App;

            var picker = (Picker)sender;




            //var cennik = pickerlist.Items[pickerlist.SelectedIndex];
            //var idceny = (await GetCenniki()).Single(cc => cc.RodzajCeny == cennik);


            int selectedIndex = pickerlist.SelectedIndex;
            //if (selectedIndex != -1)
            //{
            //    //var app = Application.Current as App;
            //    // app.Cennik = idceny.Id;
            //}


            //int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                var selectedValue = picker.SelectedItem as CennikClass;
                app.Cennik = selectedValue.RodzajCeny; //idceny.Id;
                await Application.Current.SavePropertiesAsync();
            }


        }


        public Picker PickerDrukarki { get { return pickerlist; } }
        //private async void pickerlist_Focused(object sender, FocusEventArgs e)
        //{
        //    if (!await SprConn())
        //    {
        //        pickerlist.Title = "Brak połączenia z bazą";
        //    }
        //    else
        //    {

        //    }
        //}

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

            IsBusy = true;
            string wifi = "Szachownica";//JOART_WiFi Szachownica
            int versionA = DeviceInfo.Version.Major;

            if (versionA <= 9)
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



                var version = DependencyService.Get<IWifiConnector>();
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
            }
            else
            {
                IWifiConn10 wifiConn = DependencyService.Get<IWifiConn10>(DependencyFetchTarget.GlobalInstance);

                var odp = wifiConn.SuggestNetwork(wifi, "J0@rt11a");
                if (!string.IsNullOrEmpty(odp))
                    if (odp == "Sieć została już dodana")
                    {
                        var czyRemoveNet = await DisplayAlert("Info", "Sieć została już dodana\n Czy chcesz usunąć sugerowane sieci?", "Tak", "NIE");
                        if (czyRemoveNet)
                            wifiConn.RemoveSuggestNetwork(wifi, "J0@rt11a");
                    }
                    else
                    {
                        await DisplayAlert("Info", odp, "OK");
                        if (odp == "Sieć dodano - zatwierdź w ustawieniach")
                        {
                            wifiConn.OpenSettings();
                        }
                    }

            }



            //Device.BeginInvokeOnMainThread(async () =>
            // {
            //    IWifiConn10  wifiConn= DependencyService.Get<Model.IWifiConn10>(DependencyFetchTarget.GlobalInstance);
            ////wifiConn.RequestNetwork(wifi, "J0@rt11a");
            //if(!string.IsNullOrEmpty(wifiConn.SuggestNetwork(wifi, "J0@rt11a")))
            //await DisplayAlert("Info", wifiConn.SuggestNetwork(wifi, "J0@rt11a"), "OK"); 

            //});
            IsBusy = false;

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
        //private Picker mediaTypePicker;

        public static Editor editAddress;
        private Button connectButton;
        private Button disconnectButton;
        private Button printText;
        //private IEnumerable<object> deviceList;


        //private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);




        private async void InitializeSampleUI()
        {



            var bluetoothPermissionService = DependencyService.Get<IBluetoothPermissionService>();
            var status = await bluetoothPermissionService.CheckAndRequestBluetoothPermissionAsync();

            if (!status)
            {
                await DisplayAlert("info", "Nie przyznano odpowiednich uprawnień BT", "OK");
            }


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


            var deviceList = await _cpclPrinter.connectableDevice();

            if (deviceList != null)
            {

                listdevice = deviceList.Where(c => c.Name.StartsWith("SW")).ToList();
                connectableDeviceView.ItemsSource = listdevice;// deviceList.Where(c => c.Name.StartsWith("SW"));

            }

            if (listdevice == null || listdevice.Count == 0)
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

        private void SwitchKlawiatura_Toggled(object sender, ToggledEventArgs e)
        {
            OnAlfaNumeric = SwitchKlawiatura.IsToggled;
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