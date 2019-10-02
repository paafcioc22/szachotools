
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
 
 
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace App2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginLista : ContentPage
    {
        public   ObservableCollection<Pracownik> ListaLogin { get; set; }
        SqlConnection connection;
        string konfiguracyjna;

        static string haslo;
        static public string _user;
        static public string _nazwisko;
        static public int _opeGid;
        static string haslo_chk;
         

        public ListView ListViewLogin { get { return MyListView; } }

        public LoginLista()
        {
            InitializeComponent();
            var app = Application.Current as App;
            connection = new SqlConnection
            {

                ConnectionString = "SERVER=" + app.Serwer + ";" +
                "DATABASE=" + app.BazaProd + ";" +
                "TRUSTED_CONNECTION=No" +
                ";UID=" + app.User + ";" +
                "PWD=" + app.Password
            };
            konfiguracyjna = app.BazaConf;
            GetLogins();
             
             
        }
 
        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;

        public async void SkanujIdetyfikator()
        {
            if (SettingsPage.SprConn())
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>()
                    { 
                        //ZXing.BarcodeFormat.EAN_13,
                        //ZXing.BarcodeFormat.CODE_128,
                        ZXing.BarcodeFormat.EAN_8,
                        ZXing.BarcodeFormat.CODE_39,
                    }, 

                };

                opts.TryHarder = true;

                zxing = new ZXingScannerView
                {

                    IsScanning = false,
                    IsTorchOn = false,
                    IsAnalyzing = false,
                    AutomationId = "zxingDefaultOverlay",//zxingScannerView
                    Opacity = 22,
                    Options = opts
                };

                var torch = new Switch
                {
                };

                torch.Toggled += delegate
                {
                    scanPage.ToggleTorch();
                };

                var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };

                var Overlay = new ZXingDefaultOverlay
                {
                    TopText = "Włącz latarkę",
                    BottomText = "Skanowanie rozpocznie się automatycznie",
                    ShowFlashButton = true,
                    AutomationId = "zxingDefaultOverlay",

                };
                //customOverlay.Children.Add(torch);


                var customOverlay = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };
                //customOverlay.Children.Add(btn_Manual);


                //  grid.Children.Add(customOverlay); //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                grid.Children.Add(Overlay);
                Overlay.Children.Add(torch);
                Overlay.BindingContext = Overlay;

                scanPage = new ZXingScannerPage(opts, customOverlay: Overlay)
                {
                    DefaultOverlayTopText = "Zeskanuj kod ",
                    //DefaultOverlayBottomText = " Skanuj kod ";
                    DefaultOverlayShowFlashButton = true

                };
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;
                    scanPage.AutoFocus();
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
                        {
                            if (scanPage.IsScanning) scanPage.AutoFocus(); return true;
                        });
                        Navigation.PopModalAsync();
                        entry_haslo.Text = (result.Text);
                        entry_haslo.Focus();
                    });
                };

                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var prac = e.Item as Pracownik;
            haslo = prac.opehaslo;
            haslo_chk = prac.opechk;
            _opeGid = prac.opegidnumer;
            _user = prac.opekod; //"Zalogowany : "
            _nazwisko = prac.openazwa;
            

           // SkanujIdetyfikator();
             
 
        }

         

        private void GetLogins()
        {
            ListaLogin = new ObservableCollection<Pracownik>();
            SqlCommand command = new SqlCommand();
         
            connection.Open();
            command.CommandText = "select ope_gidnumer, ope_kod, Ope_Nazwisko,Ope_Haslo, Ope_HasloChk " +
                                  "from "+ konfiguracyjna+".cdn.operatorzy  "+
                                   " where Ope_Nieaktywny=0 and ope_administrator=0";


            SqlCommand query = new SqlCommand(command.CommandText, connection);
            //query.Parameters.AddWithValue("@konf", konfiguracyjna);
            SqlDataReader rs;
            rs = query.ExecuteReader();

            while (rs.Read())
            {
                ListaLogin.Add(new Pracownik
                {
                    opekod = Convert.ToString(rs["ope_kod"]),
                    openazwa = Convert.ToString(rs["Ope_Nazwisko"]),
                    opehaslo = Convert.ToString(rs["Ope_Haslo"]),
                    opechk = Convert.ToString(rs["Ope_HasloChk"]),
                    opegidnumer = Convert.ToInt32(rs["ope_gidnumer"])
                    

                 }); 
            }
            rs.Close();
            rs.Dispose();
            connection.Close();

            MyListView.ItemsSource = ListaLogin;
        }

        public bool IsPassCorrect()
        {

            var isNumeric = int.TryParse(entry_haslo.Text, out int n);

            if (isNumeric)
            {
                int znak1 = Convert.ToInt32(entry_haslo.Text.Substring(0, 1));
                int znak24 = Convert.ToInt32(entry_haslo.Text.Substring(1, 3));
                if (znak1 == 0 && entry_haslo.Text.Length == 8)
                {
                    if (znak24 == _opeGid)
                    {
                        return true;
                    }
                    else { return false; }

                }
                else if (entry_haslo.Text.Length != 6)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
             
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (entry_haslo.Text != null) //entry_haslo.Text!=""
            {


                  
                if (IsPassCorrect())
                {
                     

                    View.StartPage.user = _user;  
                    
                   
                    View.StartPage startPage = new StartPage();
                    startPage.OdblokujPrzyciski();
                     
                    Navigation.PopModalAsync();
                }
                else
                {
                    DisplayAlert(null, "Złe haslo", "OK");

                } 
            }
        }

        private static readonly  byte[] salt = Encoding.ASCII.GetBytes("Xamarin.iOS Version: 7.0.6.168");

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }


        //public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        //{
        //    try
        //    {
        //        // Create a MemoryStream.
        //        MemoryStream mStream = new MemoryStream();

        //        // Create a new DES object.
        //        DES DESalg = DES.Create();

        //        // Create a CryptoStream using the MemoryStream 
        //        // and the passed key and initialization vector (IV).
        //        CryptoStream cStream = new CryptoStream(mStream,
        //            DESalg.CreateEncryptor(Key, IV),
        //            CryptoStreamMode.Write);

        //        // Convert the passed string to a byte array.
        //        byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

        //        // Write the byte array to the crypto stream and flush it.
        //        cStream.Write(toEncrypt, 0, toEncrypt.Length);
        //        cStream.FlushFinalBlock();

        //        // Get an array of bytes from the 
        //        // MemoryStream that holds the 
        //        // encrypted data.
        //        byte[] ret = mStream.ToArray();

        //        // Close the streams.
        //        cStream.Close();
        //        mStream.Close();

        //        // Return the encrypted buffer.
        //        return ret;
        //    }
        //    catch (CryptographicException e)
        //    {
        //        Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
        //        return null;
        //    }

        //}



        private void entry_haslo_Completed(object sender, EventArgs e)
        {


             
            if (IsPassCorrect())
            {


                View.StartPage.user = _user;
                 
                View.StartPage startPage = new StartPage();
                startPage.OdblokujPrzyciski();
                 

                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert(null, "Złe haslo", "OK");

            }
        }
         
    }

    public  class Pracownik
    {
        public string opekod { get; set; }
        public string openazwa { get; set; }
        public string opehaslo { get; set; }
        public string opechk { get; set; }
        public int  opegidnumer { get; set; }
        
    }
}
