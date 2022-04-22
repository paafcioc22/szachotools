using App2.Model;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class OddzialyViewModel : ViewModelBase
    {

        private PhotoViewModel photoView;
        public AsyncCommand LoadItemsCommand { get; set; }
        public ObservableRangeCollection<FotoOddzial> Items { get; set; }
        public ObservableRangeCollection<FotoOddzial> AllItems { get; set; }
        public ObservableRangeCollection<string> FilterOptions { get; }
 

        string selectedFilter = "Wszystkie";
        public string SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if (SetProperty(ref selectedFilter, value))
                    FilterItems();
            }
        }

        public OddzialyViewModel(PhotoViewModel _photoView)
        {
            photoView = _photoView;
            Items = new ObservableRangeCollection<FotoOddzial>();
            AllItems = new ObservableRangeCollection<FotoOddzial>();

            FilterOptions = new ObservableRangeCollection<string>
            {
                "R02",
                "R03",
                "R04",
                "R06",
                "Wszystkie"
            };
           
            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);
         
        }


        private void FilterItems()
        {
            Items.ReplaceRange(AllItems.Where(a => a.mag_region == SelectedFilter || SelectedFilter == "Wszystkie"));
        }
        int _fotki = 1;

        async Task<List<PathSplitter>> GetIleFotek()
        {
            await photoView.ListBlobsHierarchicalListing(photoView.containerClient, photoView.Title,1);
            var lista = photoView.fotki;
            List<PathSplitter> ps = new List<PathSplitter>();
            string[]  pthsplt ;

            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    pthsplt = item.Split("/");

                    ps.Add(new PathSplitter
                    {
                        Akcja = pthsplt[0],
                        Sklep = pthsplt[1],
                        NazwaFoto = pthsplt[2]
                    });
                }
            }
            return ps;
            
        }

        private async Task ExecuteLoadItemsCommand()
        {


            if (IsBusy)
                return;

            IsBusy = true;

            string Webquery2 = $@"cdn.PC_WykonajSelect N'select mag_kod 
                                ,mag_nazwa
                                ,mag_gidnumer
                                ,MAG_Opis as mag_region 
                                from CDN.Magazyny
                                where MAG_Zablokowany=0 and MAG_Wewnetrzny=1 order by mag_kod'";

            try
            {
                Items.Clear();
                var items = await App.TodoManager.PobierzDaneZWeb<FotoOddzial>(Webquery2);

                var listaZdjec = await GetIleFotek();

                var SumaList = (
                from fWeb in items
                join azure in listaZdjec on fWeb.mag_kod equals azure.Sklep into ps
                from p in ps.DefaultIfEmpty()
                select new FotoOddzial
                {
                     mag_kod= fWeb.mag_kod,
                     mag_nazwa= fWeb.mag_nazwa,
                     mag_region =  fWeb.mag_region,
                     mag_gidnumer= p?.NazwaFoto ?? string.Empty
                }).GroupBy(c => c.mag_kod)
               .SelectMany(a => a
               .Select(o => new FotoOddzial
               {
                   mag_kod = o.mag_kod,
                   mag_region = o.mag_region,
                   IleFotek = a.Where(d => d.mag_gidnumer != "").Count()
               })).GroupBy(p => new { p.mag_kod }).Select(f => f.First());

                AllItems.ReplaceRange(SumaList);

                FilterItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
    }

    class PathSplitter
    {
        public string  Akcja { get; set; }
        public string  Sklep { get; set; }
        public string  NazwaFoto { get; set; }
    }
}


