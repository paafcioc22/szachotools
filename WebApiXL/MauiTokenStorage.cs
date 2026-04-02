//// MauiTokenStorage.cs – implementacja ITokenStorage dla MAUI
//// Identyczna logika co XamarinTokenStorage – jedyna różnica to using

//using System;
//using System.Threading.Tasks;
//using SzachoApi.Client;
//using Microsoft.Maui.Storage;  // ← jedyna różnica względem Xamarin

//namespace SzachoMaui.Services
//{
//    public class MauiTokenStorage : ITokenStorage
//    {
//        private const string TokenKey       = "auth_token";
//        private const string TokenExpiryKey = "auth_token_expiry";

//        private string   _cachedToken;
//        private DateTime _cachedExpiry;

//        public async Task<string> GetTokenAsync()
//        {
//            if (!string.IsNullOrEmpty(_cachedToken))
//                return _cachedToken;

//            try
//            {
//                var token  = await SecureStorage.GetAsync(TokenKey).ConfigureAwait(false);
//                var expiry = await SecureStorage.GetAsync(TokenExpiryKey).ConfigureAwait(false);

//                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiry))
//                    return null;

//                if (!DateTime.TryParse(expiry, out var expiryDate))
//                    return null;

//                _cachedToken  = token;
//                _cachedExpiry = expiryDate;
//                return token;
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        public async Task SaveTokenAsync(string token, DateTime expiry)
//        {
//            _cachedToken  = token;
//            _cachedExpiry = expiry;

//            try
//            {
//                await SecureStorage.SetAsync(TokenKey, token).ConfigureAwait(false);
//                await SecureStorage.SetAsync(TokenExpiryKey, expiry.ToString("O")).ConfigureAwait(false);
//            }
//            catch { }
//        }

//        public async Task ClearTokenAsync()
//        {
//            _cachedToken  = null;
//            _cachedExpiry = DateTime.MinValue;

//            try
//            {
//                SecureStorage.Remove(TokenKey);
//                SecureStorage.Remove(TokenExpiryKey);
//            }
//            catch { }

//            await Task.CompletedTask.ConfigureAwait(false);
//        }

//        public bool IsTokenValid() =>
//            !string.IsNullOrEmpty(_cachedToken) &&
//            DateTime.UtcNow < _cachedExpiry.AddSeconds(-60);
//    }
//}
