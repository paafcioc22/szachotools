// SzachoApiService.cs – biblioteka netstandard2.1
// Auto re-login gdy token wygaśnie – użytkownik nic nie odczuwa

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SzachoApi.Client.Models;

namespace SzachoApi.Client
{
    public class SzachoApiService : ISzachoApiService
    {
        private readonly HttpClient _http;
        private readonly ITokenStorage _tokenStorage;
        private readonly string _username;
        private readonly string _password;

        // Zapobiega równoległym re-loginom gdy wiele wątków trafi na wygasły token
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public SzachoApiService(
            string baseUrl,
            string username,
            string password,
            ITokenStorage tokenStorage)
        {
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
            _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));

            _http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(20)
            };
        }

        // ─── Auth ─────────────────────────────────────────────────────────────

        public bool IsAuthenticated => _tokenStorage.IsTokenValid();

        /// <summary>
        /// Wywołaj raz na starcie apki – odtwarza token z SecureStorage.
        /// Jeśli token wygasł, od razu loguje się ponownie w tle.
        /// </summary>
        public async Task InitializeAsync()
        {
            var token = await _tokenStorage.GetTokenAsync().ConfigureAwait(false);
            if (!string.IsNullOrEmpty(token) && _tokenStorage.IsTokenValid())
            {
                SetAuthHeader(token);
                return;
            }

            // Token wygasł lub brak – zaloguj od razu żeby apka była gotowa
            await LoginAsync(_username, _password).ConfigureAwait(false);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var response = await PostRawAsync(
                "/api/auth/login",
                new { username, password },
                skipAuth: true).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<LoginResponse>(json, JsonOpts);

            if (result == null)
                return false;

            await _tokenStorage.SaveTokenAsync(result.Token, result.ExpiresAt).ConfigureAwait(false);
            SetAuthHeader(result.Token);
            return true;
        }

        public async Task LogoutAsync()
        {
            _http.DefaultRequestHeaders.Authorization = null;
            await _tokenStorage.ClearTokenAsync().ConfigureAwait(false);
        }

        // ─── Użytkownik ───────────────────────────────────────────────────────

        public async Task<User> PobierzUzytkownikaAsync(int gidNumer)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            var list = await GetListAsync<User>(
                $"/api/Sql/uzytkownik?gidNumer={gidNumer}").ConfigureAwait(false);
            return list.FirstOrDefault();
        }

        // ─── Ustawienia / wersja ──────────────────────────────────────────────

        public async Task<SzachoSettings> SprawdzWersjeAsync()
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            var list = await GetListAsync<SzachoSettings>("/api/Sql/wersja").ConfigureAwait(false);
            return list.FirstOrDefault();
        }

        // ─── Magazyny ─────────────────────────────────────────────────────────

        public async Task<List<int>> PobierzMagazynyAsync(int twrNumer)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            var list = await GetListAsync<MagazynInfo>(
                $"/api/Sql/magazyny/{twrNumer}").ConfigureAwait(false);
            return list.Select(m => m.Id).ToList();
        }

        // ─── Akcje ────────────────────────────────────────────────────────────

        public async Task<List<AkcjeNagElem>> PobierzAkcjeTypyAsync(int magNumer, int dodajDni = 0)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            return await GetListAsync<AkcjeNagElem>(
                $"/api/Sql/akcje-typy?magNumer={magNumer}&dodajDni={dodajDni}").ConfigureAwait(false);
        }

        public async Task<List<AkcjeNagElem>> PobierzAkcjeAsync(int akNNumer)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            return await GetListAsync<AkcjeNagElem>(
                $"/api/Sql/akcje/{akNNumer}").ConfigureAwait(false);
        }

        public async Task<List<AkcjeNagElem>> PobierzZeskanowaneAsync(int aknNumer, int magNumer)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            return await GetListAsync<AkcjeNagElem>(
                $"/api/Sql/zeskanowane?aknNumer={aknNumer}&magNumer={magNumer}").ConfigureAwait(false);
        }

        // ─── Towar ────────────────────────────────────────────────────────────

        public async Task<TwrKarty> PobierzTowarAsync(string ean)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);
            var list = await GetListAsync<TwrKarty>(
                $"/api/Sql/towar/{Uri.EscapeDataString(ean)}").ConfigureAwait(false);
            return list.FirstOrDefault();
        }

        // ─── Insert skan akcji ────────────────────────────────────────────────

        public async Task<string> InsertAkcjeSkanAsync(
            IList<AkcjeNagElem> elementy, short magNumer, string operatorName)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);

            var tasks = elementy.Select(e => PostRawAsync("/api/Sql/akcje-skan", new
            {
                aknNumer = e.AkN_GidNumer,
                magNumer = magNumer,
                grupa = e.TwrGrupa,
                twrDep = e.TwrDep,
                twrNumer = e.TwrGidNumer,
                twrStan = e.TwrStan,
                twrSkan = e.TwrSkan,
                operatorName = operatorName
            })).ToList();

            var responses = await Task.WhenAll(tasks).ConfigureAwait(false);
            return await CollectErrorsAsync(responses).ConfigureAwait(false);
        }

        // ─── Insert raport dostawy ────────────────────────────────────────────

        public async Task<string> InsertRaportDostawyAsync(List<PMM_RaportElement> elementy)
        {
            await EnsureValidTokenAsync().ConfigureAwait(false);

            var tasks = elementy.Select(e => PostRawAsync("/api/Sql/raport-dostawy", new
            {
                xLGidMM = e.TrnGidNumer,
                gidMagazynu = e.MagNumer,
                nrDokumentu = e.TrnDokumentObcy,
                operatorName = e.Operator,
                twrKod = e.Twr_Kod,
                ileZMM = (decimal)e.OczekiwanaIlosc,
                ileZeSkan = (decimal)e.RzeczywistaIlosc,
                ilosc = (decimal)e.Difference,
                dataDokumentu = e.DataMM
            })).ToList();

            var responses = await Task.WhenAll(tasks).ConfigureAwait(false);
            return await CollectErrorsAsync(responses).ConfigureAwait(false);
        }

        // ─── Token refresh ────────────────────────────────────────────────────

        private async Task EnsureValidTokenAsync()
        {
            if (IsAuthenticated)
                return;

            await _refreshLock.WaitAsync().ConfigureAwait(false);
            try
            {
                // Double-check po wejściu do locka –
                // może inny wątek już zdążył odświeżyć token
                if (IsAuthenticated)
                    return;

                var success = await LoginAsync(_username, _password).ConfigureAwait(false);
                if (!success)
                    throw new InvalidOperationException(
                        "Nie można odświeżyć sesji – sprawdź połączenie z serwerem.");
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        // ─── Helpers ──────────────────────────────────────────────────────────

        private void SetAuthHeader(string token) =>
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        private async Task<List<T>> GetListAsync<T>(string url)
        {
            var response = await _http.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<ApiResponse<List<T>>>(json, JsonOpts);
            return result?.Data ?? new List<T>();
        }

        private Task<HttpResponseMessage> PostRawAsync(
            string url, object body, bool skipAuth = false)
        {
            var json = JsonSerializer.Serialize(body, JsonOpts);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return _http.PostAsync(url, content);
        }

        private static async Task<string> CollectErrorsAsync(
            IEnumerable<HttpResponseMessage> responses)
        {
            var errors = new List<string>();
            foreach (var r in responses)
            {
                if (!r.IsSuccessStatusCode)
                    errors.Add(await r.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return errors.Count == 0 ? "OK" : string.Join("\n", errors);
        }
    }
}
