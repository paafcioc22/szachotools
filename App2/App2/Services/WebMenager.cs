using App2.Model;
using App2.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class WebMenager
    {

        IMagazynSerwis soapService;

        public WebMenager(IMagazynSerwis service)
        {
            soapService = service;
        }

        public Task<ObservableCollection<Magazynn>> GetTodoItemsAsync(string query)
        {
            return soapService.GetAllCustomers(query);
        }

        public Task<string> InsertDataNiezgodnosci(ObservableCollection<ListDiffrence> insert)
        {
            return soapService.InsertDataNiezgodnosci(insert);
        }

        public Task<List<StatusTable>> InsertDataSkan(IList<AkcjeNagElem> insert, short magnumer, string ase_operator)
        {
            return soapService.InsertDataSkan(insert, magnumer, ase_operator);
        }

        public Task<TwrInfo> PobierzTwrAsync(string ean)
        {
            return soapService.PobierzTwr(ean);
        }

        public Task<ObservableCollection<AkcjeNagElem>> GetGidAkcjeAsync(string query)
        {
            return soapService.GetGidAkcje(query);
        }

        public Task<SzachoSettings> GetSzachoSettings()
        {
            return soapService.GetSzachoSettings();
        }


        public Task<ObservableCollection<T>> PobierzDaneZWeb<T>(string query)
        {
            return soapService.PobierzDaneZWeb<T>(query);
        }
    }
}
