using App2.Model;
using App2.Model.ApiModel;
using Azure.Core;
using Azure.Storage.Blobs.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.OptimaAPI
{
    public class ServiceDokumentyApi : BaseService
    {
        private RestClient _client;
        private bool _finishedToo;
        public ObservableRangeCollection<DokNaglowekDto> DokNaglowekDtos { get; set; } =
            new ObservableRangeCollection<DokNaglowekDto>();

        public ICommand LoadMMCommand {  get; set; }
        public ICommand SwitchToggledCommand { get; private set; }
        public bool FinishedToo
        {
            get => _finishedToo;
            //set => _finishedToo = SetProperty(ref _finishedToo, value);
            set
            {
                if (SetProperty(ref _finishedToo, value))
                {
                    // Komenda jest wywoływana tylko wtedy, gdy wartość się zmienia.
                    LoadMMCommand.Execute(null);
                }
            }
        }

        public static ObservableCollection<DokElementDto> DokElementsDtos { get; set; } =
            new ObservableCollection<DokElementDto>();

        public ServiceDokumentyApi()
        {
            var app = Application.Current as App;

            _client = new RestClient($"http://{app.Serwer}");
            LoadMMCommand = new Command(async ()=> await LoadMMExec(FinishedToo) );
            //SwitchToggledCommand = new Command<bool>(ExecuteSwitchToggled);
        }

        private async void ExecuteSwitchToggled(bool obj)
        {
            await LoadMMExec(obj);
        }

        //public async Task LoadMMExec(bool finishedToo)
        //{
        //    IsBusy = true;

        //    DokNaglowekDtos.Clear();

        //    if(finishedToo)
        //        await GetDokAll(GidTyp.Mm, true);

        //    await GetDokAll(GidTyp.Mm, false);

        //    IsBusy = false;
        //}

        public async Task LoadMMExec(bool finishedToo)
        {
            IsBusy = true;

            try
            {
                var noweDane = new List<DokNaglowekDto>();
                if (finishedToo)
                {
                    var response = await GetDokAll(GidTyp.Mm, true);
                    if (response.IsSuccessful)
                        noweDane.AddRange(response.Data);
                }

                var response2 = await GetDokAll(GidTyp.Mm, false);
                if (response2.IsSuccessful)
                    noweDane.AddRange(response2.Data);
                //Debug.WriteLine("odczyt danych z bazy iteracja po isexport");
                var idsToDelete = noweDane
                    .Where(item => !string.IsNullOrEmpty(item.NumerDokumentu) && item.CreateDokDate < DateTime.Now.AddDays(-2))
                    .Select(item => item.Id)
                    .ToList();

                // Usuwanie starych dokumentów z bazy danych
                foreach (var id in idsToDelete)
                {
                    await DeleteDokument(id);
                }

                noweDane = noweDane.Where(item => !idsToDelete.Contains(item.Id)).ToList();

                foreach (var nowyItem in noweDane)
                {
                    var istniejacyItem = DokNaglowekDtos.FirstOrDefault(d => d.Id == nowyItem.Id);
                    if (istniejacyItem != null)
                    {
                        // Aktualizuj tylko potrzebne pola
                        // Debug.WriteLine("zmieniamy tylko odpowiednia pola");
                        istniejacyItem.NumerDokumentu = nowyItem.NumerDokumentu;
                        istniejacyItem.IsFinish = nowyItem.IsFinish;
                        istniejacyItem.IsExport = nowyItem.IsExport;
                        // Aktualizuj inne pola w razie potrzeby
                    }
                    else
                    {
                        // Jeśli element nie istnieje, dodaj go do kolekcji                         
                        DokNaglowekDtos.Add(nowyItem);
                    }
                }

                for (int i = DokNaglowekDtos.Count - 1; i >= 0; i--)
                {
                    var item = DokNaglowekDtos[i];
                    if (!noweDane.Any(nd => nd.Id == item.Id))
                    {
                        DokNaglowekDtos.Remove(item);
                    }
                }
            }
            catch (Exception s)
            {
                await Application.Current.MainPage.DisplayAlert("błąd", s.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
            IsBusy = false;
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

        public async Task<ApiResponse<List<DokNaglowekDto>>> GetDokAll(GidTyp gidTyp, bool isFnished)
        {

            var request = new RestRequest("/api/dokument");
            request.AddParameter("gidtyp", gidTyp);
            request.AddParameter("isFinished", isFnished);

            var response = await _client.ExecuteGetAsync<List<DokNaglowekDto>>(request);


            var apiResponse = new ApiResponse<List<DokNaglowekDto>>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {
                apiResponse.Data = response.Data;
                var itemsToAdd = new List<DokNaglowekDto>();
                var idsToDelete = new List<int>();

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

        public async Task<ApiResponse<DokWithElementsDto>> GetDokWithElementsById(int Id)
        {
            DokElementsDtos.Clear();
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

        public async Task<ApiResponse<DokElementDto>> UpadteElement(int ilosc, int naglowekId, int elementId)
        {

            var newElement = new DokElementDto
            {
                TwrIlosc = ilosc
            };


            var request = new RestRequest($"/api/dokument/{naglowekId}/element/{elementId}")
                 .AddJsonBody(newElement);

            var response = await _client.ExecutePutAsync<DokElementDto>(request);



            var apiResponse = new ApiResponse<DokElementDto>
            {
                IsSuccessful = response.IsSuccessful
            };


            if (response.IsSuccessful)
            {

            }
            else
            {
                var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";

                apiResponse.ErrorMessage = mess;
            }

            return apiResponse;

        }

        public async Task<ApiResponse<List<DokNaglowekSmallDto>>> GetDokWithElementByTwrkod(string twrkod, bool isFinsished = false)
        {

            var request = new RestRequest("/api/dokument/byTwrKod", Method.Get);

            request.AddParameter("twrkod", twrkod);
            request.AddParameter("isFinished", isFinsished);

            var response = await _client.ExecuteGetAsync<List<DokNaglowekSmallDto>>(request);

            var apiResponse = new ApiResponse<List<DokNaglowekSmallDto>>
            {
                IsSuccessful = response.IsSuccessful,
                HttpStatusCode = response.StatusCode
            };

            if (response.IsSuccessful)
            {
                apiResponse.Data = response.Data;
                //int totalTwrIlosc = response.Data
                //     .Where(naglowek => naglowek.DokElements != null) // filtruj obiekty, które mają listę DokElements
                //     .SelectMany(naglowek => naglowek.DokElements)     // połącz wszystkie listy DokElements w jedną
                //     .Where(element => element.TwrIlosc.HasValue)     // filtruj elementy, które mają wartość TwrIlosc
                //     .Sum(element => element.TwrIlosc.Value);         // oblicz sumę wartości TwrIlosc

            }
            else
            {
                apiResponse.Data = new List<DokNaglowekSmallDto>();
                apiResponse.ErrorMessage = response.Content;
            }

            return apiResponse;


        }

        public async Task<bool> ExistsOnOtherDocs(string twrkod, int dokumentId)
        {
            bool isFound = false;
            var response = await GetDokWithElementByTwrkod(twrkod);
            if (response.IsSuccessful)
            {
                isFound = response.Data.Any(s => s.Id != dokumentId && string.IsNullOrEmpty(s.NumerDokumentu));
            }
            return isFound;
        }


        public List<string> ListaMMBufor(ApiResponse<List<DokNaglowekDto>> listmmBufor)
        {
            List<string> result = new List<string>();
            if (listmmBufor.IsSuccessful)
            {
                foreach (var mm in listmmBufor.Data)
                {
                    result.Add(mm.Id + ")" + mm.MagKod + " " + mm.Opis.Replace("Pakował(a):", ""));
                }
            }
            return result;
        }

        public int TotalTwrIloscFromAllDoks(List<DokNaglowekSmallDto> smallDto)
        {
            int total = 0;
            if (smallDto != null)
            {
                total = smallDto
                       // .Where(nn=> string.IsNullOrEmpty(nn.NumerDokumentu))
                        .SelectMany(naglowek => naglowek.DokElements)     // połącz wszystkie listy DokElements w jedną
                        .Where(element => element.TwrIlosc.HasValue)
                        .Sum(element => element.TwrIlosc.Value);
            }
            return total;
        }

        public async Task<ApiResponse<CreateDokElementDto>> SaveElement(CreateDokElementDto elementDto, int dokumentId)
        {

            var request = new RestRequest($"/api/dokument/{dokumentId}/element")
                 .AddJsonBody(elementDto);

            var response = await _client.ExecutePostAsync<CreateDokElementDto>(request);

            var apiResponse = new ApiResponse<CreateDokElementDto>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {
                var locationHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("Location", StringComparison.OrdinalIgnoreCase));
                var sciezka = locationHeader.Value.ToString();

                apiResponse.Data = response.Data;
                apiResponse.Path = sciezka;

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                    var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage}";

                    apiResponse.ErrorMessage = mess;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    var body = JsonConvert.DeserializeObject<ApiResponseConflict<DokElementDto>>(response.Content);

                    apiResponse.ConflictInformation = new ConflictInformation
                    {
                        TwrKod = body.Element.TwrKod,
                        ExistingQuantity = (int)body.Element.TwrIlosc,
                        AttemptedToAddQuantity = elementDto.TwrIlosc,
                        IdElement = body.Element.Id
                    };
                }
                else
                {
                    apiResponse.ErrorMessage = response.Content;
                }
            }
            return apiResponse;

        }

        public async Task<bool> DeleteElement(DokElementDto elementDto)
        {
            bool apiResponse = false;
            var request = new RestRequest($"/api/dokument/{elementDto.DokNaglowekId}/element/{elementDto.Id}", Method.Delete);

            var response = await _client.ExecuteAsync<DokElementDto>(request);

            if (response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    apiResponse = true;
                }

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                if (response.ContentType?.StartsWith("application/json") == true)
                {
                    var error = JsonConvert.DeserializeObject<List<ErrorApi>>(response.Content);

                    var mess = $"{(HttpStatusCode)response.StatusCode} : {error[0].PropertyName}-{error[0].ErrorMessage} ";
                }
                else
                {
                    var error = response.Content;
                }

            }
            return apiResponse;

        }

        public async Task<bool> DeleteDokument(int dokumentId)
        {
            bool apiResponse = false;
            var request = new RestRequest($"/api/dokument/{dokumentId}", Method.Delete);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // Kod 204 lub 200 wskazuje na pomyślne usunięcie
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    apiResponse = true;
                }
                else
                {
                    Console.WriteLine($"Nieoczekiwany kod odpowiedzi: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine($"Błąd podczas usuwania dokumentu: {response.ErrorMessage}");
            }
            return apiResponse;

        }

        public async Task<List<MagazynInfo>> GetSklepyInfo()
        {
            List<MagazynInfo> sklepy = new List<MagazynInfo>();
            var request = new RestRequest("/api/Getsklepyinfo");

            var response = await _client.ExecuteGetAsync<List<MagazynInfo>>(request);

            if (response.IsSuccessful)
            {
                sklepy = response.Data;
            }

            return sklepy;

        }

        public async Task<List<ViewUser>> GetTestViewUsersAsync()
        {
            return await Task.FromResult(new List<ViewUser>
            {
                new ViewUser { OpeGidnumer = 1, OpeKod = "KOD001", OpeNazwa = "Jan Kowalski" },
                new ViewUser { OpeGidnumer = 2, OpeKod = "KOD002", OpeNazwa = "Anna Nowak" },
                new ViewUser { OpeGidnumer = 3, OpeKod = "KOD003", OpeNazwa = "Paweł Wiśniewski" },
                new ViewUser { OpeGidnumer = 605, OpeKod = "S3_JAK", OpeNazwa = "Ewa Dąbrowska" },
                new ViewUser { OpeGidnumer = 5, OpeKod = "KOD005", OpeNazwa = "Tomasz Żak" },
            });
        }


        public async Task<List<ViewUser>> GetViewUsersAsync()
        {
            var request = new RestRequest("/api/getview");

            var response = await _client.ExecuteGetAsync<List<ViewUser>>(request);
            List<ViewUser> posortowaniOperatorzy = new List<ViewUser>();

            var apiResponse = new ApiResponse<List<ViewUser>>
            {
                IsSuccessful = response.IsSuccessful
            };

            if (response.IsSuccessful)
            {

                apiResponse.Data = response.Data;

                posortowaniOperatorzy = response.Data.OrderBy(op =>
              op.OpeKod.StartsWith("K") ? 1 :
              op.OpeKod.StartsWith("Z") ? 2 :
              op.OpeKod.StartsWith("D") ? 3 :
              op.OpeKod.StartsWith("S") ? 4 :
              op.OpeKod.StartsWith("R") ? 5 :
              op.OpeKod.StartsWith("A") ? 6 :
              7).ToList();
            }

            return posortowaniOperatorzy;
        }



    }
}
