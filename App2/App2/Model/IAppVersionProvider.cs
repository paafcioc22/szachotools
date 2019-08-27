using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public interface IAppVersionProvider
    {
        string AppVersion { get; }
        int BuildVersion { get; }
        Task OpenAppInStore();
    }
}
