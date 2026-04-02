using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApiXL
{
    public interface ITokenStorage
    {
        Task<string> GetTokenAsync();
        Task SaveTokenAsync(string token, DateTime expiry);
        Task ClearTokenAsync();
        bool IsTokenValid();
    }
}
