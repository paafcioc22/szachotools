﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
	[XmlType("Table")]
    public class FedexPaczka
    {
		public int Fmm_GidNumer { get; set; }
		public int Fmm_EleNumer { get; set; }
		public string Fmm_NrListu { get; set; }
		public string Fmm_NrPaczki { get; set; }
		public string Fmm_NazwaPaczki { get; set; }
		public string Fmm_Elmenty { get; set; }
		public string Fmm_DataZlecenia { get; set; }
		public string Fmm_MagDcl { get; set; }
		public string Fmm_MagZrd { get; set; }
		public string Fmm_NrDokWydania { get; set; }
		public string Fmm_Opis { get; set; }

	}
}
//Fmm_GidNumer Fmm_EleNumer	Fmm_NrListu	Fmm_NrPaczki	Fmm_Elmenty	Fmm_DataZlecenia	Fmm_MagDcl