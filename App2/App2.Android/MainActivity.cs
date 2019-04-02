﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing.Net.Mobile;
using Acr.UserDialogs;
using System.Net;
using Android.Content;
using Android.Util;
using Xamarin.Forms;

namespace App2.Droid
{
    [Activity(Label = "SzachoTools", Icon = "@drawable/NewSzacho", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
           // UserDialogs.Init(this);
            UserDialogs.Init(() => this);
            LoadApplication(new App());
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
            App.TodoManager = new Model.WebMenager(new WebSerwisSzacho());
             
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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