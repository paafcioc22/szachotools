// Interfaces.cs – kontrakty biblioteki SzachoApi.Client

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SzachoApi.Client.Models;

namespace SzachoApi.Client
{
    public interface ISzachoApiService
    {
        bool IsAuthenticated { get; }

        Task InitializeAsync();
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();

        Task<User> PobierzUzytkownikaAsync(int gidNumer);
        Task<SzachoSettings> SprawdzWersjeAsync();
        Task<List<int>> PobierzMagazynyAsync(int twrNumer);

        Task<List<AkcjeNagElem>> PobierzAkcjeTypyAsync(int magNumer, int dodajDni = 0);
        Task<List<AkcjeNagElem>> PobierzAkcjeAsync(int akNNumer);
        Task<List<AkcjeNagElem>> PobierzZeskanowaneAsync(int aknNumer, int magNumer);

        Task<TwrKarty> PobierzTowarAsync(string ean);

        Task<string> InsertAkcjeSkanAsync(IList<AkcjeNagElem> elementy, short magNumer, string operatorName);
        Task<string> InsertRaportDostawyAsync(List<PMM_RaportElement> elementy);
    }

    public interface ITokenStorage
    {
        /// <summary>Zwraca zapisany token lub null jeśli brak.</summary>
        Task<string> GetTokenAsync();

        /// <summary>Zapisuje token i datę wygaśnięcia.</summary>
        Task SaveTokenAsync(string token, DateTime expiry);

        /// <summary>Usuwa token.</summary>
        Task ClearTokenAsync();

        /// <summary>Czy token istnieje i nie wygasł (z 60s marginesem).</summary>
        bool IsTokenValid();
    }
}
