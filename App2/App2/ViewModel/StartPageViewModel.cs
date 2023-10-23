using App2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class StartPageViewModel : BaseViewModel
    {

        public StartPageViewModel()
        {

            App.SessionManager.OnSessionChanged += SessionChanged;
            UpdateUserName();
        }

        private void SessionChanged(UserSession newSession)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UpdateUserName();
            });
        }

        private string _userName = "Wylogowany";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
        private bool _isButtonsEnabled;
        public bool IsButtonsEnabled
        {
            get { return _isButtonsEnabled; }
            set
            {
                _isButtonsEnabled = value;
                OnPropertyChanged();
            }
        }


        private void UpdateUserName()
        {

            if (App.SessionManager.CurrentSession != null)
            {
                UserName = "Zalogowany: " + App.SessionManager.CurrentSession.UserName;
                IsButtonsEnabled = true;
            }
            else
            {
                UserName = "Nie zalogowany";
                IsButtonsEnabled = false;
            } 
        }
    }
}
