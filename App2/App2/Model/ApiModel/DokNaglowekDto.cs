using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class DokNaglowekDto
    {
        public int Id { get; set; }
        public GidTyp GidTyp { get; set; }
        public int GidNumerXl { get; set; }
        public string MagKod { get; set; }
        public string NumerDokumentu { get; set; }
        public bool?  IsFinish { get; set; }
        public bool? IsExport { get; set; }
        public string Opis { get; set; }
        public DateTime? CreateDokDate { get; set; }
        public string UserName { get; set; }
        public bool HasElements { get; set; }
                
    }
}
