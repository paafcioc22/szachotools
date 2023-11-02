using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using App2.Droid;
using App2.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseExporter))]
namespace App2.Droid
{
    public class DatabaseExporter : IDatabaseExporter
    {
        public async Task ExportDatabaseAsync()
        {
            // Wstaw tutaj kod metody ExportDatabaseAsync, który podałem wcześniej.
            try
            {
                // Ścieżka źródłowa (gdzie jest Twoja baza danych)
                var sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MySQLite.db3");

                // Ścieżka docelowa (gdzie chcesz skopiować bazę danych)
                var destinationPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads, "MySQLite_exported.db3");

                // Kopiowanie bazy danych
                using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                using (var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    await source.CopyToAsync(destination);
                }

                Console.WriteLine($"Baza danych skopiowana do: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas eksportowania bazy danych: {ex.Message}");
            }
        }

        public void RequestStoragePermission()
        {
            var context = Android.App.Application.Context;
            var activity = MainActivity.MainActivityInstance;
            // Wstaw tutaj kod metody RequestStoragePermission, który podałem wcześniej.
            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }
        }

        
    }
}