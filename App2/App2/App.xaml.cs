using App2.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Microsoft.AppCenter;
//using Microsoft.AppCenter.Push;
 
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App2
{
    public partial class App : Application
    {
        public static WebMenager TodoManager { get; set; }

        private const string bazaConf = "bazaConf";
        private const string serwer = "serwer!";
        private const string user = "user";
        private const string password = "password";
        private const string bazaProd = "bazaProd";
        private const string cennik = "cennik";
        private const string drukarka = "drukarka";
        private const string skanowanie = "skanowanie";

        public App()
        {
            InitializeComponent();
            //AppCenter.Start("e5e8c9f5-7520-4f24-937e-3c0d00a929b0", typeof(Push));
            //AppCenter.Start("e5e8c9f5-7520-4f24-937e-3c0d00a929b0", typeof(Push));
            MainPage = new NavigationPage( new View.SplashPage());
        }


        protected override void OnStart()
        {
             
        }

        protected override void OnSleep()
        {



            var pages = Application.Current.MainPage.Navigation.ModalStack;
            if (pages.Count > 0)
            {
                var nazwa = pages[pages.Count - 1].GetType().Name;

                if (nazwa == "RaportLista_AddTwrKod" || nazwa == "WeryfikatorCenPage")
                    return;

                   View.StartPage.user = "Wylogowany";

                //if(nazwa != "WeryfikatorCenPage")
                //    View.StartPage.user = "Wylogowany";

            }
            else {
                View.StartPage.user = "Wylogowany";

            }


            
        }

        protected override void OnResume()
        {
           
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
                return "192.168.1.2\\optima";
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

        public int Cennik
        {
            get
            {
                if (Properties.ContainsKey(cennik))
                    return (Int32)Properties[cennik];
                return 0;
            }
            set
            {
                Properties[cennik] = value;
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

    }
}
