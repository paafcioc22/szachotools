using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class ViewUser:BaseViewModel
    {
        private int opegidnr;

        public int OpeGidnumer
        {
            get { return opegidnr; }
            set { SetProperty(ref opegidnr, value); }
        }

        //public int OpeGidnumer { get; set; }
        public string OpeKod { get; set; }
        public string OpeNazwa { get; set; }
    }
}
