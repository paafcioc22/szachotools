using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public interface IMagazynSerwis
    {
        Task<List<Magazynn>> GetAllCustomers(string criteria = null);
        Task <string>InsertDataNiezgodnosci (ObservableCollection<Model.ListDiffrence> polecenie);
        Task<List<RaportListaMM>> PobierzTwr (string ean);
        Task <string>PobierzWersjeApki();
    }
}
