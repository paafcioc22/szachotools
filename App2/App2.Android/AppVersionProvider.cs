

using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using App2.Droid;
using App2.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(AppVersionProvider))]
namespace App2.Droid
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string AppVersion
        {
            get
            {

                var context = Android.App.Application.Context;
                //var info = context.PackageManager.GetPackageInfo(context.PackageName, PackageManager.PackageInfoFlags.Of(0));
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                //todo : przestarzałe ale nowe nie działą
                return $"{info.VersionName}";
            }
        }

        public long BuildVersion
        {
            get
            {
                var context = Android.App.Application.Context;
                //var info = context.PackageManager.GetPackageInfo(context.PackageName, PackageManager.PackageInfoFlags.Of(0));
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return info.LongVersionCode;
            }
        }
        string _packageName => Android.App.Application.Context.PackageName;

        public Task OpenAppInStore()
        {
            return OpenAppInStore(_packageName);
        }

        public Task OpenAppInStore(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            try
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse($"market://details?id={appName}"));
                intent.SetPackage("com.android.vending");
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id={appName}"));
                Android.App.Application.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }

        public void ShowLong(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShowShort(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }      
}