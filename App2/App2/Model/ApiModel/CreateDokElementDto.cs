using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class CreateDokElementDto
    {
        public int Id { get; set; }
        public int DokTyp { get; set; }
        public int Lp { get; set; }
        public string TwrKod { get; set; }
        public string TwrNazwa { get; set; }
        public int TwrIlosc { get; set; }
    }
}
