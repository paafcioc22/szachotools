using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public  class DokWithElementsDto
    {        
        public long? Id { get; set; }
        public long? GidTyp { get; set; }
        public object GidNumer { get; set; }
        public string MagKod { get; set; }
        public object NumerDokumentu { get; set; }
        public object IsFinish { get; set; }
        public object IsExport { get; set; }
        public string Opis { get; set; }
        public DateTimeOffset? CreateDokDate { get; set; }
        public object UpdateDokDate { get; set; }
        public string UserName { get; set; }
        public List<DokElementDto> DokElements { get; set; }
    }
}
