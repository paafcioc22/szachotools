using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public interface IAppVersionProvider
    {
        string AppVersion { get; }
        long BuildVersion { get; }
        Task OpenAppInStore();
        void ShowLong(string message);
        void ShowShort(string message);
    }
}
