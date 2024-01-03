using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model
{
    public class InventoriedItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //public int Twr_Gidnumer { get; set; }
        public int DokumentId { get; set; }
        public int ItemOrder { get; set; }
        public string Twr_Kod { get; set; }
        public string Twr_Nazwa { get; set; }
        public string Twr_Ean { get; set; }
        public string ImageUrl { get; set; }
        public int ActualQuantity { get; set; }
        public DateTime ScannedTime { get; set; }
    }
}
