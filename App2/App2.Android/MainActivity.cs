
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
     
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);

            Rg.Plugins.Popup.Popup.Init( this);
      

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
             
            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            for (int i = 0; i < permissions.Length; i++)
            {
                if (permissions[i].Equals("android.permission.CAMERA") && grantResults[i] == Permission.Granted)
                    global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            } 

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if (NativeMedia.Platform.CheckCanProcessResult(requestCode, resultCode, intent))
                NativeMedia.Platform.OnActivityResult(requestCode, resultCode, intent);

            base.OnActivityResult(requestCode, resultCode, intent);
        } 

         
    }



}