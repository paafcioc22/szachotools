using App2.Model;
using App2.Model.ApiModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.QrCode.Internal;

namespace App2.OptimaAPI
{
    public class ServicePrzyjmijMM
    {
        private RestClient _client;
        public ServicePrzyjmijMM()
        {
            var app = Application.Current as App;

            _client = new RestClient($"http://{app.Serwer}");
        }

        public async Task<int> GetGidNumerFromEANMM(string BarCodeMM)
        {
            int gidNumer = 0;
            try
            {
                var request = new RestRequest($"/api/przyjmijMM/getGidnumerMMbyEan/{BarCodeMM}", Method.Get);

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    if (int.TryParse(response.Content, out int odp))
                    {
                        gidNumer = odp;
                        return gidNumer;
                    }
                    else
                    {
                        return gidNumer;
                    }
                }
                else
                {
                    return gidNumer;
                }


            }
            catch (HttpRequestException a)
            {

                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return gidNumer;
            }
        }

        public async Task<DokumentMM> GetDokMMWithElements(int trnId, int trnGidnumer=0)
        {
            try
            {
                var request = new RestRequest($"/api/przyjmijMM/GetMmElements", Method.Get);
                request.AddQueryParameter("trnId", trnId);
                request.AddQueryParameter("gidNumer", trnGidnumer);

                var response = await _client.ExecuteGetAsync<DokumentMM>(request); 

                if (response.IsSuccessful)
                {
                    var dok = response.Data;
                    return dok;
                }
                else
                {
                    return null;
                } 

            }
            catch (HttpRequestException a)
            {
                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return null;
            }
        }

        public async Task<ApiResponse<List<DokumentMM>>> GetDokMmList(bool zatwierdzoneTez, int pasDays = 100)
        {
            try
            {
                var request = new RestRequest($"/api/przyjmijMM/GetMmListNag", Method.Get);
                request.AddQueryParameter("CzyZatwierdzoneTez", zatwierdzoneTez);
                request.AddQueryParameter("pastDays", pasDays);


                var response = await _client.ExecuteGetAsync<List<DokumentMM>>(request);

                var apiResponse = new ApiResponse<List<DokumentMM>>
                {
                    IsSuccessful = response.IsSuccessful
                };

                if (response.IsSuccessful)
                {

                    var lstaMM = response.Data;

                    if (lstaMM.Any())
                    {   
                        apiResponse.Data = lstaMM;
                    }

                }
                else
                {
                    apiResponse.ErrorMessage = response.ErrorMessage;
                }

                return apiResponse;

            }
            catch (HttpRequestException a)
            {
                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return null;
            }
        }

        public async Task<ApiResponse<TwrInfo>> GetTwrFromOptima(string twrKod)
        {             

            var karta = new TwrKodRequest()
            {
                Twrcenaid = 3,
                Twrkod= twrKod
            };


            try
            {
                var request = new RestRequest("/api/gettowar")
                  .AddJsonBody(karta);

                var response = await _client.ExecutePostAsync<List<TwrInfo>>(request);


                var apiResponse = new ApiResponse<TwrInfo>
                {
                    IsSuccessful = response.IsSuccessful
                };

                if (response.IsSuccessful)
                {

                    var produkty = response.Data;

                    if (produkty.Any())
                    {                        
                        var produkt = produkty.First();
                        produkt.Twr_UrlImage = new UriImageSource { Uri = new Uri(produkt.Twr_Url.Replace("large", "small")) };                                                
                        apiResponse.Data = produkt;
                    }

                }
                else
                {
                    apiResponse.ErrorMessage = response.ErrorMessage;
                }

                return apiResponse;
            }
            catch (HttpRequestException a)
            {
                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return null;
            }



        }

        public async Task<Magazynn> GetSklepMagNumer()
        {
            try
            {
                var request = new RestRequest("/api/getMagazynInfo");

                var response = await _client.ExecuteGetAsync<Magazynn>(request);


                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                    return null;

            }
            catch (HttpRequestException a)
            {

                var sdas = a.InnerException;
                Console.WriteLine(sdas);
                return null;
            }

        }

        public async Task<ApiResponse<List<TwrInfo>>> GetTowaryGrupyListAsync(string filtrSql)
        {
            var listFromFiltr = ExtractCode.ExtractCodes(filtrSql);

            var karta = new TwrKodRequest()
            {
                Twrkody = listFromFiltr
            };

            var request = new RestRequest("/api/gettowaryGrupa", Method.Post)
                  .AddJsonBody(karta);

            var response = await _client.ExecutePostAsync<List<TwrInfo>>(request);

            var apiResponse = new ApiResponse<List<TwrInfo>>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {

                var lstaMM = response.Data;

                if (lstaMM.Any())
                {
                    apiResponse.Data = lstaMM;
                }

            }
            else
            {
                apiResponse.ErrorMessage = response.ErrorMessage;
            }

            return apiResponse;


        }

        public async Task<List<MmZCentrali>> GetRaportyZCentrali(int magNumer)
        {
            string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct RrD_MMGidNumer 
                                    from cdn.PC_RaportRoznicowyDostaw
                                    where RrD_MagGidNumer={magNumer}
                                    and RrD_DataDokumentu>=GETDATE()-30
                         '";


            var items = await App.TodoManager.PobierzDaneZWeb<MmZCentrali>(Webquery2);

            return items.ToList();
        }

    }
}
