using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace App2.Model
{
    public class RaportRoznicGrupa : List<ListDiffrence>
    {
        public string Grupa { get; set; }
        public Color kolor { get; set; }
      

        public RaportRoznicGrupa(string tytul, Color color)
        {
            Grupa = tytul;
            kolor = color;
        }
    }
}
