using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
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

        public Task<string> InsertDataNiezgodnosci(ObservableCollection<Model.ListDiffrence> insert)
        {
             return soapService.InsertDataNiezgodnosci(insert);
        }

        public Task<string> InsertDataSkan(IList<Model.AkcjeNagElem> insert, Int16 magnumer, string ase_operator)
        {
            return soapService.InsertDataSkan(insert, magnumer, ase_operator);
        }

        public Task<List<RaportListaMM>> PobierzTwrAsync(string ean)
        {
            return soapService.PobierzTwr(ean);
        }
         
        public Task<ObservableCollection<AkcjeNagElem>> GetGidAkcjeAsync(string query)
        {
            return soapService.GetGidAkcje(query);
        }

        public Task<string> GetBuildVer()
        {
            return soapService.PobierzWersjeApki();
        }


        public Task<ObservableCollection<T>> PobierzDaneZWeb<T>(string query)
        {
            return soapService.PobierzDaneZWeb<T>(query);
        }
    }
}
