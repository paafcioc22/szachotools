﻿using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public  class DokElementDto
    {        
        public int Id { get; set; }
        public int Lp { get; set; }
        public int DokTyp { get; set; }
        public string TwrKod { get; set; }
        public string TwrNazwa { get; set; }
        public int? TwrIlosc { get; set; }
        public DateTime? TwrAddTime { get; set; }
        public int DokNaglowekId { get; set; }
    }
}
