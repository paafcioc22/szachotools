using App2.Model;
using App2.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private App _app;
        public List<CennikClass> ListaCen { get; set; }

        public SettingsViewModel(App app)
        {
            _app = app;
        }

        public string Serwer
        {
            get => _app.Serwer;
            set
            {
                _app.Serwer = value;
                OnPropertyChanged();
            }
        }
        public string BazaConf
        {
            get => _app.BazaConf;
            set => _app.BazaConf = value;
        }
        public string BazaProd
        {
            get => _app.BazaProd;
            set => _app.BazaProd = value;
        }
        public string Password
        {
            get => _app.Password;
            set => _app.Password = value;
        }
        public string User
        {
            get => _app.User;
            set => _app.User = value;
        }
        public string Cennik
        {
            get => _app.Cennik;
            set => _app.Cennik = value;
        }
        public Int16 MagGidNumer
        {
            get => _app.MagGidNumer;
            set => _app.MagGidNumer = value;
        }
        public string Drukarka
        {
            get => _app.Drukarka;
            set => _app.Drukarka = value;
        }

        public sbyte Skanowanie
        {
            get => _app.Skanowanie;
            set => _app.Skanowanie = value;
        }

        public bool CzyCena1
        {
            get => _app.CzyCena1;
            set => _app.CzyCena1 = value;
        }





    }
}
