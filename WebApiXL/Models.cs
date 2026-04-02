// Models.cs – modele danych biblioteki SzachoApi.Client

using System;
using System.ComponentModel;

namespace SzachoApi.Client.Models
{
    public class User
    {
        public int GID { get; set; }
        public string Ope_ident { get; set; }
        public string Prc_Akronim { get; set; }
        public string Haslo { get; set; }
    }

    public class SzachoSettings
    {
        public string VersionApp { get; set; }
        public bool IsCena1Enabled { get; set; }
    }

    public class MagazynInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TwrKarty
    {
        public int Twr_Gidnumer { get; set; }
        public string Twr_Kod { get; set; }
        public string Twr_Ean { get; set; }
        public string Twr_Nazwa { get; set; }
        public string Twr_Symbol { get; set; }
        public string Twr_Url { get; set; }
        public decimal Twr_Cena { get; set; }
        public decimal Twr_Cena1 { get; set; }
    }

    public class AkcjeNagElem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        public int TwrGidNumer { get; set; }
        public int TwrStan { get; set; }
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
        public bool SyncRequired { get; set; }

        public string AkN_ZakresDat =>
            string.Format("{0} - {1}", AkN_DataStart, AkN_DataKoniec);

        private int _twrSkan;
        public int TwrSkan
        {
            get => _twrSkan;
            set
            {
                _twrSkan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TwrSkan)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TwrStanVsSkan)));
            }
        }

        public string TwrStanVsSkan => string.Format("{0}/{1}", _twrSkan, TwrStan);
    }

    public class PMM_RaportElement
    {
        public int Id { get; set; }
        public string Twr_Ean { get; set; }
        public string Twr_Url { get; set; }
        public string Twr_Kod { get; set; }
        public string Twr_Nazwa { get; set; }
        public int OczekiwanaIlosc { get; set; }
        public int RzeczywistaIlosc { get; set; }
        public int Difference => RzeczywistaIlosc - OczekiwanaIlosc;
        public string Status { get; set; }
        public int MagNumer { get; set; }
        public int TrnGidNumer { get; set; }
        public string TrnDokumentObcy { get; set; }
        public string DataMM { get; set; }
        public string Operator { get; set; }
        public int Lp { get; set; }
        public string GroupName { get; set; }
    }

    // ─── Odpowiedzi API ───────────────────────────────────────────────────────

    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
        public int RowCount { get; set; }
    }
}
