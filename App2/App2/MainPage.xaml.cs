using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Collections;
using SharpCifs.Smb;

namespace App2
{
//    public partial class MainPage : TabbedPage
//    {
        
//        ZXingScannerPage scanPage;
//        ZXingScannerView zxing;
//        //ZXingBarcodeImageView barcode;
//        ZXing.Mobile.MobileBarcodeScanningOptions opts;

//        ViewModel.DokMMViewModel DokMMViewModel;
//        Model.DokMM DokMM;
//        SqlConnection connection;
//        public MainPage()
//        {
//            BarBackgroundColor = Color.Black;
//            InitializeComponent();
//            //ZaladujUstawienia();
//            if (View.SettingsPage.SprConn())
//            {
//                DokMMViewModel = new ViewModel.DokMMViewModel();
//                DokMM = new Model.DokMM();
//                BindingContext = Model.DokMM.dokMMs; //DokMMViewModel.dokMMs; 
//                ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
//            }
//            else {
//                DisplayAlert(null, "Brak połączenia", "OK");
//            }
//            ButtonScanDefault.Clicked += ButtonScanDefault_Clicked;
//            ButtonScanMM.Clicked += ButtonScanMM_Clicked;
//            StartTimer();

//            var app = Application.Current as App;
//            _serwer = app.Serwer;
//            _database = app.BazaProd;
//            _uid = app.User;
//            _pwd = app.Password;
//            connection = new SqlConnection
//            {

//                ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd

//            };

//        }

        

//        private void StartTimer()
//        {
//            Device.StartTimer(System.TimeSpan.FromSeconds(7), () =>
//            {
//                Device.BeginInvokeOnMainThread(UpdateUserDataAsync);
//                return true;
//            });
//        }

//        private   void UpdateUserDataAsync()
//        {
//            Model.DokMM dokMM = new Model.DokMM();

//            ListaMMek.ItemsSource =   dokMM.getMMki();
//        }


//        public  void OdswiezListe()
//        {
//            ViewModel.DokMMViewModel DokMMViewModel;
//            DokMMViewModel = new ViewModel.DokMMViewModel();
//            ListaMMek.ItemsSource = Model.DokMM.dokMMs;
//        }

//        private void SprConn_Clicked(object sender, EventArgs e)
//        {
             
//            NadajWartosci();
//            try
//            {
//                var connStr = new SqlConnectionStringBuilder
//                {
//                    DataSource = _serwer,
//                    InitialCatalog = _database,
//                    UserID = _uid,
//                    Password = _pwd,
//                    ConnectTimeout = 3
//                }.ConnectionString;
//                using (SqlConnection conn = new SqlConnection(connStr))
//                {
//                    try
//                    {
//                        conn.Open();
//                        DisplayAlert("Connected", "Połączono z siecia", "OK");
//                    }
//                    catch (Exception )
//                    {
//                        DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                DisplayAlert("Uwaga", ex.Message, "OK");
//            }
//            ZapiszUstawienia();
//        }

//        public static string _serwer;
//        public static string _database;
//        public static string _uid;
//        public static string _pwd;

//        private void ZapiszUstawienia()
//        {
//            Xamarin.Forms.Application.Current.Properties["serwer"]  = _serwer;
//            Xamarin.Forms.Application.Current.Properties["database"] = _database ;
//            Xamarin.Forms.Application.Current.Properties["uid"] = _uid;
//            Xamarin.Forms.Application.Current.Properties["pwd"] = _pwd;
//            Xamarin.Forms.Application.Current.SavePropertiesAsync();
//        }

//        private void ZaladujUstawienia()
//        {
            
//            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("serwer"))
//                Xamarin.Forms.Application.Current.Properties.Add("serwer", _serwer);

//            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("database"))
//                Xamarin.Forms.Application.Current.Properties.Add("database", _database) ;

//            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("uid"))
//                Xamarin.Forms.Application.Current.Properties.Add("uid", _uid) ;

//            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("pwd"))
//                Xamarin.Forms.Application.Current.Properties.Add("pwd", _pwd) ;

//            _serwer= Xamarin.Forms.Application.Current.Properties["serwer"] as string ;
//            _database= Xamarin.Forms.Application.Current.Properties["database"] as string;
//            _uid = Xamarin.Forms.Application.Current.Properties["uid"] as string;
//            _pwd= Xamarin.Forms.Application.Current.Properties["pwd"] as string;

//            if (_serwer != null && _database != null && _uid != null && _pwd != null)
//            {
//                NadajWartosci();
//            }
//        }


//        private void NadajWartosci()
//        {


//            //-------------------------SERWER--------------------------
//            if (Ipserwer.Text != null && _serwer != null)
//            {
//                _serwer = Ipserwer.Text;
//            }
//            else if (Ipserwer.Text != null && _serwer == null)
//            {
//                //Ipserwer.Text = _serwer;                
//                _serwer= Ipserwer.Text;
//            }
//            else if (_serwer != null && Ipserwer.Text == null)
//            {
//                Ipserwer.Text = _serwer;
//            }

//            //----------------------BAZA-----------------------------
//            if (Instancja.Text != null && _database != null)
//            {
//                _database = Instancja.Text;
//            }
//            else if (Instancja.Text != null && _database == null)
//            {
//                _database = Instancja.Text;
//            }
//            else if (Instancja != null && Instancja.Text == null)
//            {
//                Instancja.Text = _database;
//            }
//            //----------------------LOGIN-----------------------------
//            if (Login.Text != null && _uid != null)
//            {
//                _uid = Login.Text;
//            }
//            else if (Login.Text != null && _uid == null)
//            {
//                _uid = Login.Text;
//            }
//            else if (_uid != null && Login.Text == null)
//            {
//                Login.Text = _uid;
//            }
//            //----------------------PWD-----------------------------
//            if (Haslo.Text != null && _pwd != null)
//            {
//                _pwd = Haslo.Text;
//            }
//            else if (Haslo.Text != null && _pwd == null)
//            {
//                _pwd = Haslo.Text;
//            }
//            else if (_pwd != null && Haslo.Text == null)
//            {
//                Haslo.Text = _pwd;
//            }
             

//            //_database = (Instancja.Text == null) ? "cdnxl_joart" : Instancja.Text;
//            //_uid= (Login.Text == null) ? "sa" : Login.Text;  
//            //_pwd= (Haslo.Text == null) ? "sqlSQL123#" : Haslo.Text;
             
//        }

//        //class funkcje
//        //{
            
//        //    public static int wersja_nr = 20163;
//        //    public static string serwer = _serwer;
//        //    public static string database = _database;//"cdnxl_joart";// cdnxl_test cdnxl_joart
//        //    public static string sqluser = _uid;
//        //    public static string password = _pwd;
            
//        //}

//        public bool SprConn() //Third way, slightly slower than Method 1
//        {
//            //NadajWartosci();
//            var connStr = new SqlConnectionStringBuilder
//            {
//                DataSource = _serwer,
//                InitialCatalog = _database,
//                UserID = _uid,
//                Password = _pwd,
//                ConnectTimeout = 3
//            }.ConnectionString;
//            using (SqlConnection conn = new SqlConnection(connStr))
//            {
//                try
//                {
//                    conn.Open(); 
//                    //DisplayAlert("Connected", "Połączono z siecia", "OK");
//                    return true;
//                }
//                catch (Exception )
//                {
//                    //DisplayAlert("Uwaga", "NIE Połączono z siecia", "OK");
//                    return false;
//                }
//            }
//        }

      


//        public string twrkod;
//        public string twrnazwa;
//        public string twrsymbol;
//        public string twrcena;

//        public void pobierztwrkod(string _ean)
//        {
//            lbl_twrkod.Text = "";
//            lbl_twrnazwa.Text = "";
//            lbl_twrsymbol.Text = "";
//            lbl_twrcena.Text = "";
//            if (SprConn())
//            {
//                try
//                {
//                    SqlCommand command = new SqlCommand();
//                    SqlConnection connection = new SqlConnection
//                    {
//                        ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd
//                    };
//                    connection.Open();
//                    command.CommandText = "Select twr_kod, twr_nazwa, twr_katalog, cast(twc_wartosc as decimal(5,2))cena from cdn.twrkarty " +
//                        "join cdn.TwrCeny on twr_gidnumer=TwC_TwrNumer and TwC_TwrLp=2" +
//                        "where twr_ean='" + _ean + "'";


//                    SqlCommand query = new SqlCommand(command.CommandText, connection);
//                    SqlDataReader rs;
//                    rs = query.ExecuteReader();
//                    if (rs.Read())
//                    {
//                        twrkod = Convert.ToString(rs["twr_kod"]);
//                        twrnazwa = Convert.ToString(rs["twr_nazwa"]);
//                        twrsymbol = Convert.ToString(rs["twr_katalog"]);
//                        twrcena = Convert.ToString(rs["cena"]);
//                        DisplayAlert("Zeskanowany Kod ", twrkod, "OK");
//                    }
//                    else
//                    {
//                        twrkod = "";
//                        twrnazwa = "";
//                        twrsymbol = "";
//                        twrcena = "";

//                        DisplayAlert("Uwaga", "Kod nie istnieje!", "OK");
//                        lbl_twrkod.Text = "";
//                        lbl_twrnazwa.Text = "";
//                        lbl_twrsymbol.Text = "";
//                        lbl_twrcena.Text = "";

//                    }
//                    rs.Close();
//                    rs.Dispose();
//                    connection.Close();

//                }
//                catch (Exception exception)
//                {
//                    DisplayAlert("Uwaga", exception.Message, "OK");
//                }
//            }
//            else
//            {
//                DisplayAlert("Uwaga", "Nie ma połączenia z serwerem", "OK");
//            }
//            //return twrkod;
//        }


//        public string GetNrDokMmp
//        {
//            get { return nrdokumentuMM; }
//            set { nrdokumentuMM = value; }
//        }

//        string gidnumerMM;
//        private async void ButtonScanMM_Clicked(object sender, EventArgs e)
//        {
//           // pozycje.Clear();!!!!!!!!!!!!!!!
//            opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
//            {
//                AutoRotate = false,
//                PossibleFormats = new List<ZXing.BarcodeFormat>() {

//                ZXing.BarcodeFormat.CODE_128,
//                ZXing.BarcodeFormat.CODABAR,
//                ZXing.BarcodeFormat.CODE_39,

//                }

//            };

//            opts.TryHarder = true;
//            var torch = new Switch
//            {

//            };
//            torch.Toggled += delegate
//            {
//                scanPage.ToggleTorch();
//            };

//            var grid = new Grid
//            {
//                VerticalOptions = LayoutOptions.Center,
//                HorizontalOptions = LayoutOptions.Center,

//            };

//            var Overlay = new ZXingDefaultOverlay
//            {
//                TopText = "Przyłóż telefon do kodu",
//                BottomText = "Skanowanie rozpocznie się automatycznie",
//                ShowFlashButton = true,
//                AutomationId = "zxingDefaultOverlay",
//            };

//            grid.Children.Add(Overlay);
//            Overlay.Children.Add(torch);
//            Overlay.BindingContext = Overlay;

//            scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
//            {
//                DefaultOverlayTopText = "Zeskanuj kod ",
//                //DefaultOverlayBottomText = " Skanuj kod ";
//                DefaultOverlayShowFlashButton = true
//            };

//            scanPage.OnScanResult += (result) =>
//            {
//                scanPage.IsScanning = false;
//                scanPage.AutoFocus();
//                Device.BeginInvokeOnMainThread(() =>
//                {
//                    Navigation.PopModalAsync();
//                    gidnumerMM = result.Text;
//                    GetlistMM(gidnumerMM);
//                    //DisplayAlert("Zeskanowany Kod ", gidnumerMM, "OK");
//                    Navigation.PushModalAsync(new ListaMMP(nrdokumentuMM,0));
//                    //lbl_twrkod.Text = "Kod : " + twrkod;
//                    //lbl_twrnazwa.Text = "Nazwa : " + twrnazwa;
//                    //lbl_twrsymbol.Text = "Symbol : " + twrsymbol;
//                    //lbl_twrcena.Text = "Cena : " + twrcena;
//                });
//            };
//            await Navigation.PushModalAsync(scanPage);
//        }




//        private async void ButtonScanDefault_Clicked(object sender, EventArgs e)
//        {
//            if (SprConn())
//            {
//                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
//                {
//                    AutoRotate = false,
//                    PossibleFormats = new List<ZXing.BarcodeFormat>() {
//                //ZXing.BarcodeFormat.EAN_8,
//                ZXing.BarcodeFormat.EAN_13,
//                //ZXing.BarcodeFormat.CODE_128,
//                //ZXing.BarcodeFormat.CODABAR,
//                //ZXing.BarcodeFormat.CODE_39,

//                }

//                };

//                opts.TryHarder = true;
                 
//                zxing = new ZXingScannerView
//                {
//                    //HorizontalOptions = LayoutOptions.Center,
//                    //VerticalOptions = LayoutOptions.Center,
//                    IsScanning = false,
//                    IsTorchOn = false,
//                    IsAnalyzing = false,
//                    AutomationId = "zxingDefaultOverlay",//zxingScannerView
//                    Opacity = 22,
//                    Options = opts
//                };

//                var torch = new Switch
//                {
//                    //Text = "ON/OFF Latarka"

//                };
//                torch.Toggled += delegate
//                {
//                    scanPage.ToggleTorch();
//                };
//                var grid = new Grid
//                {
//                    VerticalOptions = LayoutOptions.Center,
//                    HorizontalOptions = LayoutOptions.Center,

//                };
//                var Overlay = new ZXingDefaultOverlay
//                {
//                    TopText = "Przyłóż telefon do kodu",
//                    BottomText = "Skanowanie rozpocznie się automatycznie",
//                    ShowFlashButton = true,
//                    AutomationId = "zxingDefaultOverlay",
//                };
//                //customOverlay.Children.Add(torch);
//                grid.Children.Add(Overlay);
//                Overlay.Children.Add(torch);
//                Overlay.BindingContext = Overlay;

//                scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
//                {
//                    DefaultOverlayTopText = "Zeskanuj kod ",
//                    //DefaultOverlayBottomText = " Skanuj kod ";
//                    DefaultOverlayShowFlashButton = true  
//                };


//                scanPage.OnScanResult += (result) =>
//                {
//                    scanPage.IsScanning = false;
//                    scanPage.AutoFocus();
//                    Device.BeginInvokeOnMainThread(() =>
//                    {
//                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
//                        {
//                            if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
//                        });
//                        Navigation.PopModalAsync();
//                        pobierztwrkod(result.Text);
//                        DisplayAlert("Zeskanowany Kod ", result.Text, "OK");
//                        lbl_twrkod.Text = "Kod : " + twrkod;
//                        lbl_twrnazwa.Text = "Nazwa : " + twrnazwa;
//                        lbl_twrsymbol.Text = "Symbol : " + twrsymbol;
//                        lbl_twrcena.Text = "Cena : " + twrcena;
//                    });
//                };
//                await Navigation.PushModalAsync(scanPage);
//            }
//            else
//            {
//                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");
//                lbl_twrkod.Text = "";
//                lbl_twrnazwa.Text = "";
//                lbl_twrsymbol.Text = "";
//                lbl_twrcena.Text = "";
//            }
//        }



//        private void Button_Clicked(object sender, EventArgs e)
//        {


//        }

//        private void Button_Clicked_1(object sender, EventArgs e)
//        {
//            if (SprConn())
//            {
//                pozycje.Clear();
//                gidnumerMM = "0101604002410401032";
//                GetlistMM(gidnumerMM);
//                // DisplayAlert("Zeskanowany Kod ", nrdokumentuMM, "OK");
//                Navigation.PushModalAsync(new ListaMMP(nrdokumentuMM,0));
//                //Navigation.PushModalAsync(new ListaMMP(nrdokumentuMM, pozycje));
//                //MyListView.ItemsSource =  pozycje;
//            }
//            else
//            {
//                DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");
//                pozycje.Clear();
//            }
//        }


//        public string nrdokumentuMM;
//        public string mmtwrkod;
//        public string mmilosc;
//        public string mmurl;
//        public string mmid;
//        public string symbol;
//        //List<lista> listamm = new List<lista>();
//        // List<string> pozycje = new List<string>();

//        //public List<mmka> pozycje = new List<mmka>();
////        mmka listmmka;
//        // List<ViewModel.MmData> pozcje = new List<ViewModel.MmData>();
//        // int MMgidNumer;

//        private void ReturnGidNumer(string BarCodeMM, out int? MMgidNumer)
//        {
//            MMgidNumer = null;
//            SqlCommand command = new SqlCommand();
//            connection.Open();
//            command.CommandText = @"
                                
//                    DECLARE @codeLength AS TINYINT
//                    DECLARE @gidTypLength AS TINYINT
//                    DECLARE @gidTypOffset AS TINYINT
//                    DECLARE @gidNumerLength AS TINYINT
//                    DECLARE @gidNumerOffset AS TINYINT
//                    DECLARE @Code as VARCHAR(19)  

//                    SET @codeLength = 19;
//                    SET @gidTypLength = 5;
//                    SET @gidTypOffset = 3;
//                    SET @gidNumerLength = 10;
//                    SET @gidNumerOffset = 8;
//                    set @Code=@BarCodeString
//                    IF LEN(@Code) <> @codeLength
//                            RETURN 

//                    IF LEFT(@Code,2) <> '01'
//                            RETURN 
 
//                    IF RIGHT('0'+CONVERT(VARCHAR(2),98-(CONVERT(BIGINT,SUBSTRING(@Code,2,@codeLength-3)) % 97)),2) <> RIGHT(@Code,2)
//                            RETURN 

//                    select   CONVERT(INT,SUBSTRING(@Code,@gidTypOffset,@gidTypLength))gidtyp,
//                    CONVERT(INT,SUBSTRING(@Code,@gidNumerOffset,@gidNumerLength)) gidnumer";
//            SqlCommand query = new SqlCommand(command.CommandText, connection);
//            query.Parameters.AddWithValue("@BarCodeString", BarCodeMM);

//            using (SqlDataReader reader = query.ExecuteReader())
//            {
//                if (reader.Read())
//                {
//                    MMgidNumer = (int?)reader["gidnumer"];
//                }
//            }

//            //return MMgidNumer;
//        }

//        public void GetlistMM(string gidnumer)
//        {
//            int? MMgidNumer = null;
//            ReturnGidNumer(gidnumer, out MMgidNumer);
//            var app = Application.Current as App;
//            try
//                {
//                    SqlCommand command = new SqlCommand();
//                SqlConnection connection = new SqlConnection
//                {
//                    ConnectionString = "SERVER=" + app.Serwer + ";DATABASE=" + app.BazaProd + ";TRUSTED_CONNECTION=No;UID=" + app.User + ";PWD=" + app.Password
//                };
//                connection.Open();
//                command.CommandText = @" select 
//                         tre_lp,
//                         TrN_NumerPelny TrN_DokumentObcy,
//                          cast(TrE_ilosc as int) ilosc
//                         ,twr_kod
//                         ,twr_numerkat as symbol
//                         ,case when len(twr_kod) > 5 and len(twr_url)> 5 then
//                         replace(twr_url, substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4),
//                        substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4) + 'Miniatury/') else twr_kod end as urltwr
//                                from cdn.TraNag
//                                join cdn.traelem on TrN_TrNID = TrE_TrNId
//                                join cdn.towary on TrE_TwrId = Twr_TwrId
//                                where trn_gidnumer = @trnGidnumer
//                             order by tre_lp";
        
//                    //sprSymSezon();
//                    SqlCommand query = new SqlCommand(command.CommandText, connection);
//                query.Parameters.AddWithValue(@"trnGidnumer", MMgidNumer);
//                    SqlDataReader rs;
//                    rs = query.ExecuteReader();
//                    while (rs.Read())
//                    {
//                        nrdokumentuMM = Convert.ToString(rs["TrN_DokumentObcy"]);
//                        mmtwrkod = Convert.ToString(rs["twr_kod"]);
//                        mmilosc = Convert.ToString(rs["ilosc"]);
//                        mmurl = Convert.ToString(rs["urltwr"]);
//                        mmid = Convert.ToString(rs["tre_lp"]);
//                        symbol = Convert.ToString(rs["symbol"]);
//                        GetNrDokMmp = nrdokumentuMM;
//                        string pozycja = mmtwrkod;
//                        //pozycje.Add(pozycja); 
//                        //pozycje.Select(x => new { twrkod = mmtwrkod, ilosc = mmilosc }).ToList();
//                        //listmmka =new mmka { id=mmid, twrkod = mmtwrkod, ilosc = mmilosc, url = mmurl };

//                        pozycje.Add(new mmka { id = mmid, twrkod = mmtwrkod, ilosc = mmilosc, url = mmurl,symbol=this.symbol });
//                        //pozcje.Add(new ViewModel.MmData { twrkod = mmtwrkod, ilosc = mmilosc, url = mmurl });

//                    }
//                    rs.Close();
//                    rs.Dispose();
//                    connection.Close();
//                    //DisplayAlert("Nr dokumentu ", nrdokumentuMM, "OK");
//                }
//                catch (Exception ex)
//                {
//                    DisplayAlert("Uwaga", ex.Message, "OK");
//                }
                       
//        }

     



//        public static ObservableCollection<mmka> pozycje= new ObservableCollection<mmka>();

//        public static IEnumerable<mmka> GetMmkas(string searchText = null)
//        {
             
//            if (String.IsNullOrWhiteSpace(searchText))
//                return pozycje;
//            return pozycje.Where(c => c.twrkod.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
//        }

//        public class mmka
//        {
//            public string id { set; get; }
//            public string twrkod { set; get; }
//            public string ilosc { set; get; }
//            public string url { set; get; }
//            public string ilosc_OK { set; get; }
//            public string symbol { set; get; }

//            public static implicit operator ObservableCollection<object>(mmka v)
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public class ShowHidePassEffect : RoutingEffect
//        {
//            public string EntryText { get; set; }
//            public ShowHidePassEffect() : base("Xamarin.ShowHidePassEffect") { }
//        }

//        #region sambaInsert
//        //[STAThread]
//        private async void BtnTworzMM_Clicked(object sender, EventArgs e)
//        {
//            //App2.TworzMM tworzMM=new TworzMM();
//            try
//            {
//                //TworzMM.LogowanieAutomatyczne();
//                //TworzMM.DodanieDOK();
//                var auth1 = new NtlmPasswordAuthentication($"pawel:P@weł19648");
//                //var file = new SmbFile("smb://pawel:P@weł19648@192.168.1.51/Users/pawel/NewFileName.txt");
//                var file = new SmbFile($"smb://192.168.1.51/metki/NewFileName.txt", auth1);
//                //Create file.
//                file.CreateNewFile();

//                //Get writable stream.
//                var writeStream = file.GetOutputStream();

//                //Write bytes.
//                writeStream.Write(Encoding.UTF8.GetBytes("Hello!"));

//                //Dispose writable stream.
//                writeStream.Dispose();
//            }
//            catch (COMException ex)
//            {
//                if (ex.InnerException != null)
//                   await DisplayAlert("Uwaga","Błąd: " + ex.InnerException.Message,"OK");
//                else
//                    await DisplayAlert("Uwaga", "Błąd: " + ex.Message, "OK");
//            }
//            catch (Exception ex)
//            {
//                if (ex.InnerException != null)
//                    await DisplayAlert("Uwaga", "Błąd: " + ex.InnerException.Message, "OK");

//                else
//                    await DisplayAlert("Uwaga", "Błąd: " + ex.Message, "OK");
//            }
//            finally
//            {
//               // TworzMM.Wylogowanie();
//            }
//        }



//        // class TworzMM
//        //{

//        //    protected static CDNBase.ApplicationClass Application2 = null;
//        //    protected static ILogin Login = null;
//        //    private static TabbedPage page;

//        //    // Przyklad 1. - Logowanie do O! bez wyświetlania okienka logowania
//        //    public static void LogowanieAutomatyczne()
//        //    {
                 
//        //        {
//        //            string Operator = "Admin";
//        //            string Haslo = null;    // operator nie ma hasła
//        //            string Firma = "Joart_OPT";    // nazwa firmy Joart_OPT Joart_place

//        //            object[] hPar = new object[] {
//        //                 0,  0,  0,  0,  0,   0,  0,    0,   0,   0,   0,   0,   0,   0,  0,   0, 0 };  // do jakich modułów się logujemy
//        //                                                                                                /* Kolejno: KP, KH, KHP, ST, FA, MAG, PK, PKXL, CRM, ANL, DET, BIU, SRW, ODB, KB, KBP, HAP
//        //                                                                                                 */

//        //            // katalog, gdzie jest zainstalowana Optima (bez ustawienia tej zmiennej nie zadziała, chyba że program odpalimy z katalogu O!)
//        //            //string path= @"\\192.168.1.51\Optima";
//        //            //System.IO.Path.GetDirectoryName = path ;
//        //            // tworzymy nowy obiekt apliakcji
//        //            //System.Environment.CurrentDirectory = @"\\192.168.1.51\Optima";
//        //            var folder = new SmbFile("smb://pawel:P@weł19648@192.168.1.51/Optima/");
//        //            System.IO.Directory.SetCurrentDirectory( @"\\192.168.1.51\Optima");
//        //            Application2 = new CDNBase.ApplicationClass();
//        //            // blokujemy
//        //            Application2.KonfigConnectStr = @"NET:CDN_KNF_Konfiguracja,ADMIN\OPTIMA,NT=0";
//        //            Application2.LockApp(513, 5000, null, null, null, null);
//        //            // logujemy się do podanej Firmy, na danego operatora, do podanych modułów
//        //            //Login = Application.Login(Operator, Haslo, Firma, hPar[0], hPar[1], hPar[2], hPar[3], hPar[4], hPar[5], hPar[6], hPar[7], hPar[8], hPar[9], hPar[10], hPar[11], hPar[12], hPar[13], hPar[14], hPar[15], hPar[16]);

//        //            Login = Application2.Login(Operator, Haslo, Firma, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);


//        //            // tu jesteśmy zalogowani do O!
//        //            page.DisplayAlert("Info", "Jesteśmy zalogowani do O!", "OK");
//        //        }
//        //        //catch (System.IO.IOException x)
//        //        //{
//        //        //    page.DisplayAlert("Info", x.Message, "OK");
//        //        //}

//        //    }
//        //    // wylogowanie z O!
//        //    public  static void Wylogowanie()
//        //    {
//        //        //niszczymy Login
//        //          Login = null;
//        //        // odblokowanie (wylogowanie) O!
//        //        Application2.UnlockApp();
//        //        // niszczymy obiekt Aplikacji
//        //        Application2 = null;

//        //        page.DisplayAlert("Info", "Jesteśmy Wylogowani z O!", "OK");

//        //    }


//            // dla obiektów Clarionowych, których nie można tak łatwo używać w C#
//        //    protected static void InvokeMethod(object o, string Name, object[] Params)
//        //    {
//        //        o.GetType().InvokeMember(Name, System.Reflection.BindingFlags.InvokeMethod, null, o, Params, null, null, null);
//        //    }
//        //    // dla obiektów Clarionowych, których nie można tak łatwo używać w C#
//        //    protected static void SetProperty(object o, string Name, object Value)
//        //    {
//        //        if (o == null)
//        //            return;
//        //        o.GetType().InvokeMember(Name, System.Reflection.BindingFlags.SetProperty, null, o, new object[] { Value });
//        //    }
//        //    // dla obiektów Clarionowych, których nie można tak łatwo używać w C#
//        //    protected static object GetProperty(object o, string Name)
//        //    {
//        //        if (o == null)
//        //            return null;
//        //        return o.GetType().InvokeMember(Name, System.Reflection.BindingFlags.GetProperty, null, o, null);
//        //    }



//        //    static ArrayList towary = new ArrayList();
//        //    static ArrayList ilosci = new ArrayList();
//        //    //ArrayList ilosci;
//        //    public static void DOdajPozycje()
//        //    {

//        //        towary.Add("43ckm0490G");
//        //        towary.Add("43CKW0350LYC");
//        //        towary.Add("43CKW0350LYC");

//        //        //   //ilosci = 
//        //        //ilosci= new ArrayList();
//        //        ilosci.Add(1);
//        //        ilosci.Add(20);
//        //        ilosci.Add(40);

//        //    }

//        //    static public void DodanieDOK()
//        //    {
//        //        CDNBase.AdoSession Sesja = Login.CreateSession();
//        //        DOdajPozycje();
//        //        //var Rezerwacje = (CDNHlmn.DokumentyHaMag)Sesja.CreateObject("CDN.DokumentyHaMag", null);
//        //        //var Rezerwacja = (CDNHlmn.IDokumentHaMag)Rezerwacje.AddNew(null);
//        //        //Tworzenie dokumentu
//        //        CDNHlmn.DokumentyHaMag Rezerwacje = (CDNHlmn.DokumentyHaMag)Sesja.CreateObject("CDN.DokumentyHaMag", null);
//        //        CDNHlmn.IDokumentHaMag Rezerwacja = (CDNHlmn.IDokumentHaMag)Rezerwacje.AddNew(null);//W tym miejscu brakuje obiektu program się zatrzymuje
//        //                                                                                            //Tworzenie Kontrahenta
//        //                                                                                            //CDNBase.ICollection Kontrahenci = (CDNBase.ICollection)(Sesja.CreateObject("CDN.Kontrahenci", null));
//        //                                                                                            //CDNHeal.IKontrahent Kontrahent = (CDNHeal.IKontrahent)Kontrahenci["Knt_Kod='LAS'"];


//        //        //Nagłówek dokumentu

//        //        Rezerwacja.Rodzaj = 312010;
//        //        Rezerwacja.TypDokumentu = 312;
//        //        Rezerwacja.PodID = 1;
//        //        Rezerwacja.PodmiotTyp = 1;
//        //        Rezerwacja.Bufor = 1;
//        //        Rezerwacja.MagazynZrodlowyID = 1;
//        //        Rezerwacja.MagazynDocelowyID = 68;
//        //        Rezerwacja.DataDok = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
//        //        Rezerwacja.Export = 0;
//        //        Rezerwacja.WalutaSymbol = "PLN";

//        //        //Dodajemy pozycje


//        //        CDNBase.ICollection Pozycje = Rezerwacja.Elementy;

//        //        for (int i = 0; i < towary.Count; i++)
//        //        {
//        //            CDNHlmn.IElementHaMag Pozycja = (CDNHlmn.IElementHaMag)Pozycje.AddNew(null);
//        //            Pozycja.TowarKod = (string)towary[i];
//        //            Pozycja.Ilosc = (int)ilosci[i];
//        //        }


//        //        Sesja.Save();

//        //        page.DisplayAlert("Info", "Dodano dokument: " + Rezerwacja.NumerPelny, "OK");


//        //    }
//        //}
//        #endregion

//        private async void Button_Clicked_2(object sender, EventArgs e)
//        {
//            await Navigation.PushModalAsync(new View.AddMMPage());
            
//            DokMMViewModel = new ViewModel.DokMMViewModel();
//            ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
//        }

//        private void Button_Clicked_3(object sender, EventArgs e)
//        {
//            DokMMViewModel = new ViewModel.DokMMViewModel();
//            ListaMMek.ItemsSource = null;
//            ListaMMek.ItemsSource = Model.DokMM.dokMMs;// DokMMViewModel.dokMMs;
//        }

//        private async void ListaMMek_ItemTapped(object sender, ItemTappedEventArgs e)
//        {

//            var mmka = e.Item as Model.DokMM;
//            //await DisplayAlert("Kliknięto :", mmka.gidnumer.ToString(), "OK");
//            ((ListView)sender).SelectedItem = null;
//            Model.DokMM dokMM = new Model.DokMM();
//            dokMM.getElementy(mmka.gidnumer);
//            await Navigation.PushModalAsync(new View.AddElementMMList(mmka));
//        }

//        private async void Delete_Clicked(object sender, EventArgs e)
//        {
//            var action = await DisplayAlert(null, "Czy chcesz usunąć MM z listy?", "Tak", "Nie");
//            if(action)
//            {
//                var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
//                Model.DokMM.dokMMs.Remove(usunMM);
//                Model.DokMM.DeleteMM(usunMM); 
//            }
//        }

//        private void Save_Clicked(object sender, EventArgs e)
//        {
//            var usunMM = (sender as MenuItem).CommandParameter as Model.DokMM;
//            //Model.DokMM dokMM = new Model.DokMM();
//            usunMM.TheColor =Color.Black;

//            //((ListView)sender).SelectedItem = null;
//        }
//    }
}
 