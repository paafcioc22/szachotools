using BCrypt.Net;
using CryptSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public ObservableCollection<pracownik> Items { get; set; }
        SqlConnection connection;
        string konfiguracyjna;

        static string haslo;
        static string _user;
        static public string _nazwisko;
        static string haslo_chk;

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

        //async void nowawersja()
        //{
        //    try
        //    {
        //        var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();

        //        if (!isLatest)
        //        {
        //            var update = await DisplayAlert("New Version", "There is a new version of this app available. Would you like to update now?", "Yes", "No");

        //            if (update)
        //            {
        //                await CrossLatestVersion.Current.OpenAppInStore();
        //            }
        //        }
        //    }
        //    catch (Exception s)
        //    {

        //        await DisplayAlert(null, s.Message, "OK");
        //    }
        //}

        ZXing.Mobile.MobileBarcodeScanningOptions opts;
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var prac = e.Item as pracownik;
            haslo = prac.opehaslo;
            haslo_chk = prac.opechk;
            _user = "Zalogowany : " +prac.opekod;
            _nazwisko = prac.openazwa;
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");


            if (SettingsPage.SprConn())
            {
                opts = new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    AutoRotate = false,
                    PossibleFormats = new List<ZXing.BarcodeFormat>() {
               
                ZXing.BarcodeFormat.EAN_13,
                ZXing.BarcodeFormat.CODE_128,
                ZXing.BarcodeFormat.CODABAR,
                ZXing.BarcodeFormat.CODE_39,
                },
                    //CameraResolutionSelector = availableResolutions => {

                    //    foreach (var ar in availableResolutions)
                    //    {
                    //        Console.WriteLine("Resolution: " + ar.Width + "x" + ar.Height);
                    //    }
                    //    return availableResolutions[0];
                    //},
                    

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
                        entry_haslo.Text= (result.Text);
                        entry_haslo.Focus();
                    });
                };
                 
                await Navigation.PushModalAsync(scanPage);
            }
            else
            {
                await DisplayAlert("Uwaga", "Nie połączono z serwerem", "OK");

            }


            //Deselect Item
          //  ((ListView)sender).SelectedItem = null;
        }



        private void GetLogins()
        {
            Items = new ObservableCollection<pracownik>();
            SqlCommand command = new SqlCommand();
         
            connection.Open();
            command.CommandText = "select ope_kod, Ope_Nazwisko,Ope_Haslo, Ope_HasloChk "+
                                  "from "+ konfiguracyjna+".cdn.operatorzy  "+
                                   " where Ope_Nieaktywny=0 and ope_administrator=0";


            SqlCommand query = new SqlCommand(command.CommandText, connection);
            //query.Parameters.AddWithValue("@konf", konfiguracyjna);
            SqlDataReader rs;
            rs = query.ExecuteReader();

            while (rs.Read())
            {
                 Items.Add(new pracownik
                {
                    opekod = Convert.ToString(rs["ope_kod"]),
                    openazwa = Convert.ToString(rs["Ope_Nazwisko"]),
                    opehaslo = Convert.ToString(rs["Ope_Haslo"]),
                    opechk = Convert.ToString(rs["Ope_HasloChk"]),
                     
                }); 
            }
            rs.Close();
            rs.Dispose();
            connection.Close();

            MyListView.ItemsSource = Items;
        }
        
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (entry_haslo.Text != null) //entry_haslo.Text!=""
            {
                string hasloAll = haslo_chk + haslo;

                //string passwordHash = BCrypt.Net.BCrypt.HashPassword(entry_haslo.Text);
                //var ssss= BCrypt.Net.BCrypt.Verify(entry_haslo.Text, passwordHash);

                var key = Encoding.UTF8.GetBytes(entry_haslo.Text);

                DES DESalg = DES.Create();
                string sData = entry_haslo.Text;

                // Encrypt the string to an in-memory buffer.
                byte[] Data = EncryptTextToMemory(sData, DESalg.Key, DESalg.IV);
                var haaslo = Convert.ToBase64String(Data);
                // Decrypt the buffer back to a string.
                string Final = DecryptTextFromMemory(Data, DESalg.Key, DESalg.IV);

                //var bytes = Encoding.UTF8.GetBytes(entry_haslo.Text); 
                //var salt= Crypter.Blowfish.GenerateSalt();
                //var hhaaaa = Crypter.Blowfish.Crypt(bytes, salt);
                //var hsh = EncryptData(entry_haslo.Text, haslo_chk);
                //var dec = DecryptData(hsh, haslo_chk);

                //var haslohas = Encrypt(entry_haslo.Text, haslo_chk);   //te jest ok
                //var dehaslo = Decrypt(haslohas, haslo_chk);

                // string zahaslowane = Crypter.Blowfish.Crypt(key, haslo_chk);

                bool check = Crypter.SafeEquals(entry_haslo.Text, Final.Replace("\0", ""));
                if (check)
                {
                    // DisplayAlert(null, "Brawo- OK", "OK");
                    View.StartPage.user = _user;

                    //View.StartPage.CzyPrzyciskiWlaczone = true;
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

        internal static string Encrypt(string textToEncrypt, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            //Anything to process?
            if (textToEncrypt == null || textToEncrypt == "") return "";

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        internal static string Decrypt(string encryptedText, string encryptionPassword)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            //Anything to process?
            if (encryptedText == null || encryptedText == "") return "";

            byte[] descryptedBytes;
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
            }
            return Encoding.UTF8.GetString(descryptedBytes);
        }


        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }

        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }

       



        public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    DESalg.CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Convert the passed string to a byte array.
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }

        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(Data);

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    DESalg.CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create buffer to hold the decrypted data.
                byte[] fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it.
                //return new Encoding.UTF8.GetString(fromEncrypt);
                return Encoding.UTF8.GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        private void entry_haslo_Completed(object sender, EventArgs e)
        {
            var key = Encoding.UTF8.GetBytes(entry_haslo.Text);

            DES DESalg = DES.Create();
            string sData = entry_haslo.Text;
             
            byte[] Data = EncryptTextToMemory(sData, DESalg.Key, DESalg.IV);
            var haaslo = Convert.ToBase64String(Data);

            string Final = DecryptTextFromMemory(Data, DESalg.Key, DESalg.IV); 

            bool check = Crypter.SafeEquals(entry_haslo.Text, Final.Replace("\0", ""));
            if (check)
            {
                // DisplayAlert(null, "Brawo- OK", "OK");
                View.StartPage.user = _user;

                //View.StartPage.CzyPrzyciskiWlaczone = true;
                View.StartPage startPage = new StartPage();
                startPage.OdblokujPrzyciski();

                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert(null, "Złe haslo", "OK");

            }
        }

        //public string EncryptData(string strData, string strKey)
        //{
        //    byte[] key = { }; //Encryption Key   
        //    byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        //    byte[] inputByteArray;



        //    try
        //    {
        //        key = Encoding.UTF8.GetBytes(strKey);
        //        // DESCryptoServiceProvider is a cryptography class defind in c#.  
        //        DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();

        //        inputByteArray = Encoding.UTF8.GetBytes(strData);
        //        MemoryStream Objmst = new MemoryStream();
        //        CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateEncryptor(null,null), CryptoStreamMode.Write);
        //        Objcs.Write(inputByteArray, 0, inputByteArray.Length);
        //        Objcs.FlushFinalBlock();

        //        return Convert.ToBase64String(Objmst.ToArray());//encrypted string  
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public string DecryptData(string strData, string strKey)
        //{
        //    byte[] key = { };// Key   
        //    byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        //    byte[] inputByteArray = new byte[strData.Length];

        //    try
        //    {
        //        key = Encoding.UTF8.GetBytes(strKey);
        //        DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
        //        inputByteArray = Convert.FromBase64String(strData);

        //        MemoryStream Objmst = new MemoryStream();
        //        CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(null, null), CryptoStreamMode.Write);
        //        Objcs.Write(inputByteArray, 0, inputByteArray.Length);
        //        Objcs.FlushFinalBlock();

        //        Encoding encoding = Encoding.UTF8;
        //        return encoding.GetString(Objmst.ToArray());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    public class pracownik
    {
        public string opekod { get; set; }
        public string openazwa { get; set; }
        public string opehaslo { get; set; }
        public string opechk { get; set; }
    }
}
