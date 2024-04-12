using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WebApiLib;
using WebApiLib.Serwis;
using App2.View;
using App2.Services;
using System.IO;
using Plugin.Media;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App2
{
    public partial class App : Application
    {
        public static WebMenager TodoManager { get; set; }
        public static SessionManager SessionManager { get; set; }= new SessionManager();

        NetworkMonitor networkMonitor;

        static SQLiteRepository database;

        public static SQLiteRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLiteRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InventoriedItems.db3"));
                }
                return database;
            }
        }


        private const string bazaConf = "bazaConf";
        private const string serwer = "serwer!";
        private const string user = "user";
        private const string password = "password";
        private const string bazaProd = "bazaProd";
        private const string cennik = "cennik";
        private const string drukarka = "drukarka";
        private const string skanowanie = "skanowanie";
        private const string magGidnumer = "magGidnumer";
        private const string czycena1 = "czycena1";

      

        public App() 
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1JpRGRGfV5yd0VCalhQTnNXUj0eQnxTdEZiWH5ecXJRR2VUU0ZxXQ==");
            InitializeComponent();
            DependencyService.Register<IszachoApi, szachoApi>();
            //MainPage = new NavigationPage( new View.SplashPage());
            networkMonitor = new NetworkMonitor();
            Plugin.Media.CrossMedia.Current.Initialize();
            MainPage = new NavigationPage( new View.StartPage());
        }


        protected override void OnStart()
        {
            AppCenter.Start("android=81c9a3a4-22c2-495f-8d63-c07f259b834a;" +
                 "uwp={Your UWP App secret here};" +
                 "ios={Your iOS App secret here};" +
                 "macos={Your macOS App secret here};",
                 typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            SessionManager.UpdateSleepTime();
            networkMonitor.StopMonitoring();
            //var pages = Application.Current.MainPage.Navigation.ModalStack;
            //if (pages.Count > 0)
            //{
            //    var nazwa = pages[pages.Count - 1].GetType().Name;
            //    List<string> listaOkienNoLogOff = new List<string>()
            //    {
            //        "RaportLista_AddTwrKod",
            //        "WeryfikatorCenPage",
            //        "AddTwrPage",
            //        "List_ScanPage",

            //    };

            //    if(listaOkienNoLogOff.Where(c=>c.Contains(nazwa)).Any())                
            //        return;

            // View.StartPage.user = "Wylogowany";               

            //}
            //else 
            //{
            //    View.StartPage.user = "Wylogowany";
            //}


        }

        protected override void OnResume()
        {
            networkMonitor.StartMonitoring();

            if (!SessionManager.IsValidSession())
            {
                SessionManager.EndSession();
                MainPage = new NavigationPage(new View.StartPage()); // Przenieś użytkownika do ekranu logowania
            }
        }

        public string BazaConf
        {
            get
            {
                if (Properties.ContainsKey(bazaConf))
                    return Properties[bazaConf].ToString();
                return "CDN_KNF_Konfiguracja";
            }
            set
            {
                Properties[bazaConf] = value;
            }
            
        }
        public string Serwer
        {
            get
            {
                if (Properties.ContainsKey(serwer))
                    return Properties[serwer].ToString();
                return "192.168.1.2:8081"; 
            
            }
            set
            {
                Properties[serwer] = value;
                
            } 
        }

        public string User
        {
            get
            {
                if (Properties.ContainsKey(user))
                    return Properties[user].ToString();
                return "szachownica";
            }
            set
            {
                Properties[user] = value;
            } 
        }

        public string Password
        {
            get
            {
                if (Properties.ContainsKey(password))
                    return Properties[password].ToString();
                return "6@#RE2us";
            }
            set
            {
                Properties[password] = value;
            }

        }

        public string BazaProd
        {
            get
            {
                if (Properties.ContainsKey(bazaProd))
                    return Properties[bazaProd].ToString();
                return "";
            }
            set
            {
                Properties[bazaProd] = value;
            }

        }

        public string Cennik
        {
            get
            {
                if (Properties.ContainsKey(cennik))
                    return  Properties[cennik].ToString();
                return "BRUTTO";
            }
            set
            {
                Properties[cennik] = value;
            }

        }

        public Int16 MagGidNumer
        {
            get
            {
                if (Properties.ContainsKey(magGidnumer))
                    return (Int16)Properties[magGidnumer];
                return 0;
            }
            set
            {
                Properties[magGidnumer] = value;
            }

        }

        public string Drukarka
        {
            get
            {
                if (Properties.ContainsKey(drukarka))
                    return (string)Properties[drukarka];
                return "00:00:00:00:00:00";
            }
            set
            {
                Properties[drukarka] = value;
            }

        }

        public sbyte Skanowanie
        {
            get
            {
                if (Properties.ContainsKey(skanowanie))
                    return (sbyte)Properties[skanowanie];
                return 0;
            }
            set
            {
                Properties[skanowanie] = value;
            }

        }

         

        public bool CzyCena1 
        {
            get
            {
                if (Properties.ContainsKey(czycena1))
                    return (bool)Properties[czycena1];
                return false;
            }
            set
            {
                Properties[czycena1] = value;
            }
        }


    }
}
