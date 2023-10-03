using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class TwrKodRequest
    {
        public int Twrcenaid { get; set; }
        public string Twrkod { get; set; }
        public string Twrean { get; set; }
        public List<string> Twrkody { get; set; }
    }
}
