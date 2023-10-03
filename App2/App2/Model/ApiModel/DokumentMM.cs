using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class DokumentMM
    {
        public int Trn_Trnid { get; set; }
        public int Trn_Gidnumer { get; set; }
        public string DataMM { get; set; }
        public string TrN_DokumentObcy { get; set; }
        public string TrN_Opis { get; set; }
        public int Trn_MagZrdId { get; set; }

        public List<TwrInfo>? Elementy { get; set; }
    }
}
