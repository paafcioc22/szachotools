using App2.Model.ApiModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public interface IMagazynSerwis
    {
        Task<ObservableCollection<Magazynn>> GetAllCustomers(string criteria = null);
        Task <string>InsertDataNiezgodnosci (ObservableCollection<Model.ListDiffrence> polecenie);
        Task <string>InsertDataSkan (IList<Model.AkcjeNagElem> polecenie, Int16 magNumer,string ase_operaotr);
        Task<TwrInfo> PobierzTwr (string ean);
        Task<ObservableCollection<AkcjeNagElem>> GetGidAkcje (string query);
        Task <string>PobierzWersjeApki();
        Task<ObservableCollection<T>> PobierzDaneZWeb<T> (string query);

    }
}
