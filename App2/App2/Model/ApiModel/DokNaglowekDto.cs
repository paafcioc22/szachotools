using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class DokNaglowekDto: BaseViewModel
    {
        private bool? isExport;
        private string numerDokumentu;
        private bool? isFinish;

        public int Id { get; set; }
        public GidTyp GidTyp { get; set; }
        public int? GidNumerXl { get; set; }
        public string MagKod { get; set; }
        public string NumerDokumentu
        {
            get => numerDokumentu;
            set => SetProperty(ref numerDokumentu, value);
        }
        public bool? IsFinish
        {
            get => isFinish;
            set => SetProperty(ref isFinish, value);
        }
        public bool? IsExport
        {
            get => isExport;
            set => SetProperty(ref isExport, value);
        }
        public string Opis { get; set; }
        public DateTime? CreateDokDate { get; set; }
        public string UserName { get; set; }
        public bool HasElements { get; set; }

    }
}
