using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WebApiLib.Serwis
{
    public interface IszachoApi
    {
        Task<ObservableCollection<TwrKarty>> PobierzTwr(string ean);
        // Task<TwrKarty> PobierzTwrAsync(string ean);
        Task<IEnumerable<T>> ExecSQLCommandAsync<T>(string command);
    }
}