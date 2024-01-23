using App2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class FotoRelacjeViewModel : BaseViewModel
    {
        public ObservableCollection<NagElem> ListaZFiltrem { get; set; }

        public Command LoadItemsCommand { get; set; }

        public FotoRelacjeViewModel()
        {
            ListaZFiltrem = new ObservableCollection<NagElem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            string Webquery2 = $@"cdn.PC_WykonajSelect N'select distinct AkN_GidNumer, AkN_GidTyp  
                            , AkN_GidNazwa ,AkN_NazwaAkcji, AkN_DataStart,AkN_DataKoniec,Ake_FiltrSQL,IsSendData
                            from cdn.pc_akcjeNag 
                            INNER JOIN   CDN.PC_AkcjeElem ON AkN_GidNumer =Ake_AknNumer
                            join [CDN].[PC_AkcjeTyp] on akn_gidtyp = GidTypAkcji
                            where AkN_GidTyp=16 and AkN_DataKoniec>=GETDATE() -30  
                            and getdate() >= AkN_DataStart
                            order by AkN_DataStart desc
                         '";

            try
            {
            var items = await App.TodoManager.PobierzDaneZWeb<NagElem>(Webquery2);

            
                ListaZFiltrem.Clear();
               
                foreach (var item in items)
                {
                    ListaZFiltrem.Add(item);
                }
              
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
}
