using App2.Model.ApiModel;
using Azure.Core;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.OptimaAPI
{
    public class ServiceDokumentyApi : BaseService
    {
        private RestClient _client;
        public static ObservableCollection<DokNaglowekDto> DokNaglowekDtos { get; set; } =
            new ObservableCollection<DokNaglowekDto>();
       

        public static ObservableCollection<DokElementDto> DokElementsDtos { get; set; } =
            new ObservableCollection<DokElementDto>();

        public ServiceDokumentyApi()
        {
            var app = Application.Current as App;

            _client = new RestClient($"http://{app.Serwer}"); //todo : pobierz adres ip z konfiguracji aplikacji

        }

        public async Task<ApiResponse<DokNaglowekDto>> SaveDokument(CreateDokumentNaglowek dokument)
        {

            var request = new RestRequest("/api/dokument")
                  .AddJsonBody(dokument);

            var response = await _client.ExecutePostAsync<DokNaglowekDto>(request);

            var apiResponse = new ApiResponse<DokNaglowekDto>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {
                var locationHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("Location", StringComparison.OrdinalIgnoreCase));
                var sciezka = locationHeader.Value.ToString();

                var dok = response.Data;

                apiResponse.Data = response.Data;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";

                apiResponse.ErrorMessage = mess;
            }

            return apiResponse;
        }

        public async Task<ApiResponse<List<DokNaglowekDto>>> GetDokAll(GidTyp gidTyp)
        {

            var request = new RestRequest("/api/dokument");
            request.AddParameter("gidtyp", gidTyp);

            var response = await _client.ExecuteGetAsync<List<DokNaglowekDto>>(request);


            var apiResponse = new ApiResponse<List<DokNaglowekDto>>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            { 

                apiResponse.Data = response.Data;

                foreach (var item in response.Data)
                {
                    DokNaglowekDtos.Add(item);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(response.Content) && response.Content.Contains("{"))
                {
                    var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                    var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";

                    apiResponse.ErrorMessage = mess;
                }
                else
                {
                    apiResponse.ErrorMessage = response.Content;
                }
            }

            return apiResponse;

        }

        public async Task<ApiResponse<List<DokWithElementsDto>>> GetDokWithElementsById(int Id)
        {

            var request = new RestRequest($"/api/dokument/{Id}");
            

            var response = await _client.ExecuteGetAsync<DokWithElementsDto>(request);


            var apiResponse = new ApiResponse<DokWithElementsDto>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {

                apiResponse.Data = response.Data;

                foreach (var item in response.Data.DokElements)
                {
                    DokElementsDtos.Add(item);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(response.Content) && response.Content.Contains("{"))
                {
                    var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                    var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";

                    apiResponse.ErrorMessage = mess;
                }
                else
                {
                    apiResponse.ErrorMessage = response.Content;
                }
            }

            return apiResponse;

        }

        public async Task<ApiResponse<bool>> UpdateDokMm(int Id, DokNaglowekDto dokument)
        {
            //var dokument = new DokNaglowekDto
            //{
            //    MagKod = "bol",
            //    GidNumerXl = 111,
            //    IsExport = true,
            //    IsFinish = true,
            //    Opis = "nowy-10022023",
            //    NumerDokumentu = "mm/23213/jgss/2023"
            //};

          

            var request = new RestRequest($"/api/dokument/{Id}")
                 .AddJsonBody(dokument);

            var response = await _client.ExecutePutAsync<DokNaglowekDto>(request);

            var apiResponse = new ApiResponse<bool>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {
                 apiResponse.Data = response.IsSuccessful;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                if (!string.IsNullOrEmpty(response.Content) && response.Content.Contains("{"))
                {
                    var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                    var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";

                    apiResponse.ErrorMessage = mess;
                }
                else
                {
                    apiResponse.ErrorMessage = response.Content;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

                apiResponse.ErrorMessage = response.Content;
            }


            return apiResponse;
        }

        


    }
}
