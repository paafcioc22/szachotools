﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace App2.Model
{
    [XmlType("Table")]
    public class AkcjeNagElem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Lp { get; set; } 
        public int AkN_GidNumer { get; set; }
        public byte AkN_GidTyp { get; set; }
        public string AkN_GidNazwa { get; set; }
        public string AkN_NazwaAkcji { get; set; }
        public string AkN_DataStart { get; set; }
        public string AkN_DataKoniec { get; set; }
        public int Ake_ElemLp { get; set; }
        public string Ake_NazwaFiltrSQL { get; set; }
        public string Ake_FiltrSQL { get; set; }


        public string TwrKod { get; set; }
        [Indexed]
        public int TwrGidNumer { get; set; }
        public int TwrStan { get; set; }
        //public int TwrSkan { get; set; }
        public string TwrNazwa { get; set; }
        public string TwrGrupa { get; set; }
        public string TwrDep { get; set; }
        public string TwrUrl { get; set; }
        public string TwrSymbol { get; set; }
        public decimal TwrCena { get; set; }
        public decimal TwrCena1 { get; set; }
        public decimal TwrCena30 { get; set; }
        public string TwrEan { get; set; }
        public string Operator { get; set; }
        public bool IsSendData { get; set; }
        public bool SyncRequired{ get; set; }

        public string AkN_ZakresDat
        {
            get { return String.Format("{0} - {1}", AkN_DataStart, AkN_DataKoniec); }
        }

        private int _iloscOK;
        public int TwrSkan
        {
            get { return _iloscOK; }
            set
            {
                //if (_iloscOK == value)
                //    return;
                _iloscOK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TwrSkan)));
            }
        }

        private string _skan;
        public string TwrStanVsSKan
        {
            get
            {
                _skan= String.Format("{0}/{1}", _iloscOK, TwrStan);
                return _skan;
            }
            set
            {
                _skan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TwrSkan)));
            }
        }

    }
}
