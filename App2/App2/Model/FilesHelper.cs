using App2.Model.ApiModel;
using App2.Services;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiLib.Serwis;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.Model
{
    public static class FilesHelper
    {
        public static IszachoApi Api => DependencyService.Get<IszachoApi>();
        public static async Task<string> SaveToCacheAsync(Stream data, string fileName)
        {
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await using var stream = File.Create(filePath);
            await data.CopyToAsync(stream);
            stream.Close();

            return filePath;
        }

        public static string ConvertUrlToOtherSize(string url, string twrkod, OtherSize otherSize, bool onlyFromPresta = false)
        {
            string rtrnUrl = "";
            if (!string.IsNullOrEmpty(url))
            {
                if (url.Contains("://szachownica.com"))
                {
                    rtrnUrl = url
                        .Replace("large", otherSize.ToString())
                        .Replace("home", otherSize.ToString())
                        .Replace("small", otherSize.ToString());
                }
                else
                {
                    //replace(twr_url,replace(twr_kod,'/','-')+'.JPG','')+'Miniatury/'+replace(twr_kod,'/','-')+'.JPG' as Twr_Url_Small
                    if (onlyFromPresta)
                    {
                        return url;
                    }
                    else
                    {
                        switch (otherSize)
                        {
                            case OtherSize.large:
                                rtrnUrl = GetOriginalUrl(url);
                                break;
                            case OtherSize.home:
                                rtrnUrl = GetOriginalUrl(url);
                                break;
                            case OtherSize.small:
                                 rtrnUrl = GetThumbnailUrl(url, twrkod);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }

            return rtrnUrl;
        }
        static string GetThumbnailUrl(string originalUrl, string productCode)
        {
            try
            {
                // Parsowanie oryginalnego URL
                var uri = new Uri(originalUrl);

                // Tworzenie segmentów ścieżki
                var segments = uri.Segments;
                if (segments.Length < 2) return originalUrl; // Nieprawidłowy format URL

                // Dodanie 'Miniatury' do ścieżki
                var newPath = Path.Combine(uri.Scheme + "://", uri.Host, segments[1].TrimEnd('/'), "Miniatury", WebUtility.UrlEncode(productCode) + ".JPG");

                return newPath;
            }
            catch
            {
                // W przypadku błędu, zwróć oryginalny URL
                return originalUrl;
            }
        }

        static string GetOriginalUrl(string thumbnailUrl)
        {
            try
            {
                 var newPath = thumbnailUrl.Replace("Miniatury/","");

                return newPath;
            }
            catch
            {
                // W przypadku błędu, zwróć URL miniatury
                return thumbnailUrl;
            }
        }

        public static async Task<TwrInfo> GetCombinedTwrInfo(string _ean, int NrCennika, RestRequest request, IRestClient client)
        {
            // Pobieranie danych z pierwszego serwisu
            TwrInfo twrInfoFromOptima = null;
            var response = await client.ExecutePostAsync<List<TwrInfo>>(request);

            var apiResponse = new ApiResponse<TwrInfo>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {
                apiResponse.Data = response.Data?.FirstOrDefault();
                twrInfoFromOptima = apiResponse.Data;
            }
            else
            {
                
            }

            // Pobieranie danych z drugiego serwisu
            string Webquery = $"cdn.pc_pobierztwr '{_ean}', {NrCennika}";
            var produkt = await Api.ExecSQLCommandAsync<TwrInfo>(Webquery);
            //var daneFromCentrala = await App.TodoManager.PobierzTwrAsync(Webquery);

            if (produkt.Any())
            {
                var daneFromCentrala = produkt.First();

                if (twrInfoFromOptima != null && daneFromCentrala != null)
                {
                    // Tworzenie nowego obiektu TwrInfo, łączącego dane z obu źródeł
                    return new TwrInfo
                    {
                        Twr_Kod = twrInfoFromOptima.Twr_Kod ?? daneFromCentrala.Twr_Kod,
                        Twr_Gidnumer = twrInfoFromOptima.Twr_Gidnumer != 0 ? twrInfoFromOptima.Twr_Gidnumer : daneFromCentrala.Twr_Gidnumer,
                        Stan_szt = twrInfoFromOptima.Stan_szt,  // tutaj przyjmuję, że pierwszy serwis ma bardziej aktualne dane
                        Twr_Url = daneFromCentrala.Twr_Url ?? twrInfoFromOptima.Twr_Url,
                        Twr_Nazwa = daneFromCentrala.Twr_Nazwa,
                        Twr_Symbol =  daneFromCentrala.Twr_Symbol,
                        Twr_Ean = twrInfoFromOptima.Twr_Ean ?? daneFromCentrala.Twr_Ean,
                        Twr_Cena = daneFromCentrala.Twr_Cena,
                        Twr_Cena1 = daneFromCentrala.Twr_Cena1,  // przyjmuję, że drugi serwis dostarcza tę wartość
                        Twr_Cena30= daneFromCentrala.Twr_Cena30
                    };
                }
                else if (twrInfoFromOptima == null)
                {
                    DependencyService
                                        .Get<IAppVersionProvider>()
                                        .ShowShort("Brak stanów lub towar nie znaleziony. Dane pobrane z centrali");
                    return daneFromCentrala;  // zwracanie danych z drugiego serwisu, jeśli pierwszy zwrócił null
                }
                else
                {
                    // Jeśli daneFromCentrala == null, ale response.Data != null
                    // Możesz dostosować ten fragment kodu zgodnie z potrzebami, tutaj zwracam dane z response.Data
                    return twrInfoFromOptima;  // zakładam, że response.Data to lista
                }

            }
            return twrInfoFromOptima;  // zakładam, że response.Data to lista

            // Sprawdzanie, czy oba źródła danych zwróciły wartość
        }
        public enum OtherSize
        {
            large,
            home,
            small
        }
    }
}
