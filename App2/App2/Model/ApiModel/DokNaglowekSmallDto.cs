using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace App2.Model.ApiModel
{
    public class DokNaglowekSmallDto
    {
        public int Id { get; set; }
        public string MagKod { get; set; }
        public string? NumerDokumentu { get; set; }
        public string? Opis { get; set; }
        public DateTime? UpdateDokDate { get; set; }
        public string UserName { get; set; }
        public List<DokElementSmallDto>? DokElements { get; set; }

    }

    public class DokElementSmallDto
    {
        public int Lp { get; set; }
        public string? TwrKod { get; set; }
        public string? TwrNazwa { get; set; }
        public int? TwrIlosc { get; set; }

    }
}
