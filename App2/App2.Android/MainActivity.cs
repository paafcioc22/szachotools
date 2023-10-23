
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using App2.Services;
using FFImageLoading.Forms.Platform;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.Droid
{//, WindowSoftInputMode = Android.Views.SoftInput.AdjustResize
    [Activity(Label = "SzachoTools", Icon = "@drawable/NewSzacho", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation|ConfigChanges.KeyboardHidden|ConfigChanges.Keyboard)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static MainActivity MainActivityInstance { get; private set; }
        private const int yourRequestCode = 1001;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            base.OnCreate(savedInstanceState);
            NativeMedia.Platform.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);  // dodane do essential
            //Forms.SetFlags("SwipeView_Experimental");
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);

            Rg.Plugins.Popup.Popup.Init( this);
            // UserDialogs.Init(this);
            //UserDialogs.Init(() => this);

            //global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //CachedImageRenderer.Init(true); 

            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();

          

            //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S) // Android 12 
            //{
            //    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) != Permission.Granted ||
            //        ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothScan) != Permission.Granted)
            //    {

            //        ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.BluetoothConnect, Manifest.Permission.BluetoothScan }, yourRequestCode);
            //    }
            //}

            LoadApplication(new App());
           // Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
            App.TodoManager = new WebMenager(new WebSerwisSzacho());
           // App.SessionManager = new Model.SessionManager();
            // Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
            MainActivityInstance = this;
        }
         
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
             
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            //dodane do essential

            //if (requestCode == yourRequestCode) // yourRequestCode to wartość int, którą przekazałeś w powyższym kodzie
            //{
            //    if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            //    {
            //        // Uprawnienie zostało przyznane, możesz kontynuować operacje związane z Bluetooth
            //    }
            //    else
            //    {
            //        // Uprawnienie nie zostało przyznane. Informuj użytkownika, że niektóre funkcje mogą nie działać poprawnie.
            //    }
            //}

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if (NativeMedia.Platform.CheckCanProcessResult(requestCode, resultCode, intent))
                NativeMedia.Platform.OnActivityResult(requestCode, resultCode, intent);

            base.OnActivityResult(requestCode, resultCode, intent);
        }


        //private void CheckUpdate(Action doIfUpToDate)
        //{
        //    if (NeedUpdate())
        //    {
        //        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
        //        alert.SetTitle("New Update");
        //        alert.SetMessage("You must download the newest version of this to play multiplayer.  Would you like to now?");
        //        alert.SetCancelable(false);
        //        alert.SetPositiveButton("Yes", new EventHandler<DialogClickEventArgs>((object sender, DialogClickEventArgs e) => GetUpdate()));
        //        alert.SetNegativeButton("No", delegate { });
        //        alert.Show();
        //    }
        //    else
        //    {
        //        doIfUpToDate.Invoke();
        //    }
        //}

        //private bool NeedUpdate()
        //{
        //    try
        //    {
        //        var curVersion = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
        //        var newVersion = curVersion;

        //        string htmlCode;
        //        //probably better to do in a background thread
        //        using (WebClient client = new WebClient())
        //        {
        //            htmlCode = client.DownloadString("https://play.google.com/store/apps/details?id=" + PackageName + "&hl=en");
        //        }

        //        HtmlWebViewSource doc = new HtmlWebViewSource();
        //        doc.(htmlCode);

        //        newVersion = doc.DocumentNode.SelectNodes("//div[@itemprop='softwareVersion']")
        //                          .Select(p => p.InnerText)
        //                          .ToList()
        //                          .First()
        //                          .Trim();

        //        return String.Compare(curVersion, newVersion) < 0;
        //    }
        //    catch (Exception e)
        //    {
        //        //Log.Error(Tag, e.Message);
        //        Toast.MakeText(this, "Trouble validating app version for multiplayer gameplay.. Check your internet connection", ToastLength.Long).Show();
        //        return true;
        //    }
        //}

        //private void GetUpdate()
        //{
        //    try
        //    {
        //        StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + PackageName)));
        //    }
        //    catch (ActivityNotFoundException e)
        //    {
        //        //Default to the the actual web page in case google play store app is not installed
        //        StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + PackageName)));
        //    }
        //}
    }



}