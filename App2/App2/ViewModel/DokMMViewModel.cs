using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App2.ViewModel
{
    public class DokMMViewModel
    {

        public static  ObservableCollection<Model.DokMM> dokMMs { get; set; }


        public   DokMMViewModel()
        {
            dokMMs = new Model.DokMM().getMMki(); 
        }

        //public static IEnumerable<Model.DokMM> dokMMs()
        //{
        //    return dokMMs = new Model.DokMM().getMMki();
        //}


        //public List<Model.DokMM> dokMMs { get; set; }

        //public DokMMViewModel()
        //{
        //    dokMMs = new Model.DokMM().getMMki();
        //}

    }
}
