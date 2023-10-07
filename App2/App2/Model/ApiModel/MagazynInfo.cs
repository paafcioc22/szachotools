using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class MagazynInfo
    {
        public int Id { get; set; }
        public string Magazyn { get; set; }
        public string MagKod { get; set; }
        public string? Region { get; set; }
        public string? Telefon { get; set; }
        public int? Ilosc { get; set; }

        public MagazynInfo()
        {
            Id = 0;
        }
    }
}
