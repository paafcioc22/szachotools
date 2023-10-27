using System;
using System.Collections.Generic;
using System.Text;
using App2.Model;

namespace App2.Services
{
    public class SessionManager
    {
        public delegate void SessionChangedEventHandler(UserSession newSession);
        public event SessionChangedEventHandler OnSessionChanged;

        private const int SessionTimeoutMinutes = 2;
        public UserSession CurrentSession { get; set; }

        public void CreateSession(string username)
        {
            CurrentSession = new UserSession
            {
                UserName = username,
                LastActivity = DateTime.Now
            };

            OnSessionChanged?.Invoke(CurrentSession);
        }


        public void UpdateSleepTime()
        {
            if (CurrentSession != null)
            {
                CurrentSession.LastActivity = DateTime.Now;
            }
        }

        public bool IsValidSession()
        {
            if (CurrentSession == null || CurrentSession.LastActivity == null)
                return true;//todo : zmiana

            return (DateTime.Now - CurrentSession.LastActivity.Value).TotalMinutes <= SessionTimeoutMinutes;
        }

        public void EndSession()
        {
            CurrentSession = null;
            OnSessionChanged?.Invoke(CurrentSession);
        }
    }

}
