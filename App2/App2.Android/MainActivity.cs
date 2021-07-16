using System;
 
using Android.App;
using Android.Net;
 
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
 
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
 
using ZXing.Net.Mobile; 
using System.Net;
using Android.Content;
using Android.Util;
using Xamarin.Forms;
using FFImageLoading.Forms.Platform;
using Android.Net.Wifi;
using App2.Model;


//using Microsoft.AppCenter.Push;
//using Microsoft.AppCenter;


namespace App2.Droid
{//, WindowSoftInputMode = Android.Views.SoftInput.AdjustResize
    [Activity(Label = "SzachoTools", Icon = "@drawable/NewSzacho", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation|ConfigChanges.KeyboardHidden|ConfigChanges.Keyboard)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static MainActivity MainActivityInstance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;


            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);  // dodane do essential

            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            // UserDialogs.Init(this);
            //UserDialogs.Init(() => this);
        
            //global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //CachedImageRenderer.Init(true); 

            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();
 
            //Push.SetSenderId("466514621461");

            LoadApplication(new App());
           // Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
            App.TodoManager = new Model.WebMenager(new WebSerwisSzacho());
            // Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
            MainActivityInstance = this;
        }
         

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            //dodane do essential
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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