using App2.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        public App()
        {
            InitializeComponent(); 
            MainPage = new NavigationPage( new View.StartPage());
        }


        protected override void OnStart()
        {
             
        }

        protected override void OnSleep()
        {
            View.StartPage.user = "Wylogowany";
            //View.StartPage.CzyPrzyciskiWlaczone = false;

            //Model.Analyze.DataPresent = false;
            //View.StartPage.blokujPrzyciski();

            //View.StartPage startPage = new View.StartPage();
            //startPage.blokujPrzyciski();
        }

        protected override void OnResume()
        {
          //  View.StartPage.user = "";
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


    }
}
