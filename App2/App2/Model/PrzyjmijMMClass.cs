﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.Model
{


    public class PrzyjmijMMClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
         
        public string id { set; get; }
        public string twrkod { set; get; }
        public int ilosc { set; get; }
        public string url { set; get; }
        public string symbol { set; get; }
        public string nazwa { set; get; }
        public string ean { set; get; }
        public string cena { set; get; }
        public string nrdokumentuMM { set; get; }
        public string DatadokumentuMM { set; get; }
        public string OpisdokumentuMM { set; get; }
        public string Operator { set; get; }
        public int GIDdokumentuMM { set; get; }
        public int GIDMagazynuMM { set; get; }
        public int XLGIDMM { set; get; }
        public bool StatusMM { set; get; }


        public string GetNrDokMmp
        {
            get { return nrdokumentuMM; }
            set { nrdokumentuMM = value; }
        }

        private int _iloscOK;
        public int ilosc_OK
        {
            get { return _iloscOK; }
            set
            {
                if (_iloscOK == value)
                    return;
                _iloscOK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ilosc_OK)));
            }
        }


        SqlConnection connection; 

        public PrzyjmijMMClass()
        {
            var app = Application.Current as App;
            connection = new SqlConnection
            {

                ConnectionString = "SERVER=" + app.Serwer + ";" +
                "DATABASE=" + app.BazaProd + ";" +
                "TRUSTED_CONNECTION=No" +
                ";UID=" + app.User + ";" +
                "PWD=" + app.Password
            };
        }


       private SQLiteAsyncConnection _connection = DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();

        public static ObservableCollection<PrzyjmijMMClass> ListOfTwrOnMM = new ObservableCollection<PrzyjmijMMClass>();
        public static ObservableCollection<PrzyjmijMMClass> ListaMMDoPrzyjcia = new ObservableCollection<PrzyjmijMMClass>();


        public ObservableCollection<PrzyjmijMMClass> CreateListMM(PrzyjmijMMClass przyjmijMM)
        {
            ListaMMDoPrzyjcia.Clear();

            var tmpKlas = przyjmijMM;

            ListaMMDoPrzyjcia.Add(przyjmijMM);

            return ListaMMDoPrzyjcia;
        }

        public async   Task<ObservableCollection<PrzyjmijMMClass>> getListMM()
        {
            //DokMM dokMM = new DokMM();

            try
            {
                ListaMMDoPrzyjcia.Clear();
                connection.Open();
                string query = @"SELECT  trn_trnid,trn_gidnumer,
                     cast(trn_datadok as date) dataMM
	                 ,trn_numerobcy
	                 ,trn_opis
                     ,trn_magzrdID magId
                  FROM[CDN].[TraNag]
                        where
                    (trn_rodzaj= 312010 and trn_bufor<>0 )
                    or(trn_rodzaj = 312010 and trn_datadok > getdate() - 8)
             
                 order by 1";


                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader sqlData = command.ExecuteReader();
                //await _connection.CreateTableAsync<Model.PrzyjmijMMLista>();
                while (sqlData.Read())
                {
                    DateTime dateTime = System.Convert.ToDateTime(sqlData["dataMM"]);
                    //ListaMMDoPrzyjcia.Add(new PrzyjmijMMClass
                    PrzyjmijMMClass mMClass = new PrzyjmijMMClass()
                    
                    {
                        GIDdokumentuMM = System.Convert.ToInt32(sqlData["trn_trnid"]),
                        DatadokumentuMM = dateTime.ToString("dd-MM-yyyy"),
                        nrdokumentuMM = System.Convert.ToString(sqlData["trn_numerobcy"]),
                        OpisdokumentuMM = System.Convert.ToString(sqlData["trn_opis"]),
                        GIDMagazynuMM = System.Convert.ToInt32(sqlData["magId"]),
                        XLGIDMM = System.Convert.ToInt32(sqlData["trn_gidnumer"]),
                        StatusMM = await PobierzSatus(System.Convert.ToInt32(sqlData["trn_gidnumer"]))
                    };


                    CreateListMM(mMClass);
                    //PobierzSatus(System.Convert.ToInt32(sqlData["trn_gidnumer"]));

                }
                sqlData.Close();
                sqlData.Dispose();
                connection.Close();

                return ListaMMDoPrzyjcia;
            }
            catch (Exception)
            {

                throw;
            }
             
        }
        //private SQLiteAsyncConnection _connection= DependencyService.Get<SQLite.ISQLiteDb>().GetConnection();
        public async Task<bool>   PobierzSatus(int gidnumer)
        {
            try
            {
                //return await Task.Run(async () =>
                //{
                    await _connection.CreateTableAsync<Model.RaportListaMM>();
                //  var wynik= await _connection.Table<Model.RaportListaMM>().Where(c => c.GIDdokumentuMM == gidnumer).ToListAsync();



               var wynik = await _connection.QueryAsync<Model.RaportListaMM>("select * from RaportListaMM where XLGIDMM = ? ", gidnumer);
                if (wynik.Count > 0)
                {
                    var wpis = wynik[0].Sended;

                    var pozycja = ListaMMDoPrzyjcia.Where(x => x.XLGIDMM == gidnumer).ToList();
                    pozycja[0].StatusMM = wpis;
                    return true;//pozycja[0].StatusMM;
                }
                else
                {
                    return true;
                }
               // });
            }
            catch (Exception )
            {
                return false;
            }
        }


        public static IEnumerable<PrzyjmijMMClass> GetMmkas(string searchText = null)
        {

            if (String.IsNullOrWhiteSpace(searchText))
                return ListOfTwrOnMM;
            return ListOfTwrOnMM.Where(c => c.twrkod.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
        }


        


        public void ReturnGidNumerFromEANMM(string BarCodeMM, out int? MMgidNumer)
        {
            MMgidNumer = null;
            SqlCommand command = new SqlCommand();
            connection.Open();
            command.CommandText = @" 
                    DECLARE @codeLength AS TINYINT
                    DECLARE @gidTypLength AS TINYINT
                    DECLARE @gidTypOffset AS TINYINT
                    DECLARE @gidNumerLength AS TINYINT
                    DECLARE @gidNumerOffset AS TINYINT
                    DECLARE @Code as VARCHAR(19)  

                    SET @codeLength = 19;
                    SET @gidTypLength = 5;
                    SET @gidTypOffset = 3;
                    SET @gidNumerLength = 10;
                    SET @gidNumerOffset = 8;
                    set @Code=@BarCodeString
                    IF LEN(@Code) <> @codeLength
                            RETURN 

                    IF LEFT(@Code,2) <> '01'
                            RETURN 
 
                    IF RIGHT('0'+CONVERT(VARCHAR(2),98-(CONVERT(BIGINT,SUBSTRING(@Code,2,@codeLength-3)) % 97)),2) <> RIGHT(@Code,2)
                            RETURN 

                    --select   CONVERT(INT,SUBSTRING(@Code,@gidTypOffset,@gidTypLength))gidtyp,
                    --CONVERT(INT,SUBSTRING(@Code,@gidNumerOffset,@gidNumerLength)) gidnumer
                    
                    if exists(select trn_trnid gidnumer 
					from cdn.tranag 
					where trn_gidnumer =( 
					select	CONVERT(INT,SUBSTRING(@Code,@gidNumerOffset,@gidNumerLength)) gidnumer)
					and  trn_gidtyp =1600) 
					begin
						select trn_trnid gidnumer 
					from cdn.tranag 
					where trn_gidnumer =( 
					select	CONVERT(INT,SUBSTRING(@Code,@gidNumerOffset,@gidNumerLength)) gidnumer)
					and  trn_gidtyp =1600
					end else select 0 as gidnumer";
            SqlCommand query = new SqlCommand(command.CommandText, connection);
            query.Parameters.AddWithValue("@BarCodeString", BarCodeMM);

            using (SqlDataReader reader = query.ExecuteReader())
            {
                if (reader.Read())
                {
                    MMgidNumer = (int?)reader["gidnumer"];
                }
            }
        }


        public void GetlistMMElements(string KodEanMM = null,int gidnumer=0)
        {
            //ListOfTwrOnMM.Clear();
            int? MMgidNumer = null;
            if (KodEanMM!=null)
            ReturnGidNumerFromEANMM(KodEanMM, out MMgidNumer);
            var app = Application.Current as App;
            try
            {
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = "SERVER=" + app.Serwer + ";DATABASE=" + app.BazaProd + ";TRUSTED_CONNECTION=No;UID=" + app.User + ";PWD=" + app.Password
                };
                connection.Open();
                command.CommandText = @" select trn_trnid,trn_gidnumer,
                         tre_lp,twr_nazwa,
                         TrN_NumerPelny TrN_DokumentObcy,
                          cast(TrE_ilosc as int) ilosc
                         ,twr_kod, mag_gidnumer trn_magzrdID
                         ,twr_numerkat as symbol
                         ,case when len(twr_kod) > 5 and len(twr_url)> 5 then
                         replace(twr_url, substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4),
                        substring(twr_url, 1, len(twr_url) - len(twr_kod) - 4) + 'Miniatury/') else twr_kod end as urltwr,
                        cast(trn_datadok as date) dataMM
                                from cdn.TraNag
                                join cdn.traelem on TrN_TrNID = TrE_TrNId
                                join cdn.magazyny on trn_magdocid=mag_magid
                                join cdn.towary on TrE_TwrId = Twr_TwrId
                                where (trn_gidnumer = @trnGidnumer or trn_trnid=@trnGidnumer)
                                and  (trn_rodzaj=312010 )
                             order by tre_lp";// tre_lp;


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                if (KodEanMM != null)
                {
                    query.Parameters.AddWithValue(@"trnGidnumer", MMgidNumer);
                }
                else {
                    query.Parameters.AddWithValue(@"trnGidnumer", gidnumer);
                }
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {
                    nrdokumentuMM = System.Convert.ToString(rs["TrN_DokumentObcy"]);
                    string mmtwrkod = System.Convert.ToString(rs["twr_kod"]);
                    string mmtwrnazwa = System.Convert.ToString(rs["twr_nazwa"]);
                    int mmilosc = System.Convert.ToInt16(rs["ilosc"]);
                    string mmurl = System.Convert.ToString(rs["urltwr"]);
                    int mmgidnumer = System.Convert.ToInt32(rs["trn_trnid"]);
                    int GidMagazynu = System.Convert.ToInt32(rs["trn_magzrdID"]);
                    string mmid = System.Convert.ToString(rs["tre_lp"]);
                    symbol = System.Convert.ToString(rs["symbol"]);
                    DateTime dateTime = System.Convert.ToDateTime(rs["dataMM"]);
                    GetNrDokMmp = nrdokumentuMM;
                    string pozycja = mmtwrkod;
                    int xlGidNumer = System.Convert.ToInt32(rs["trn_gidnumer"]);
                    ListOfTwrOnMM.Add(new PrzyjmijMMClass
                    {
                        id = mmid,
                        twrkod = mmtwrkod,
                        ilosc = mmilosc,
                        url = mmurl,
                        nazwa = mmtwrnazwa,
                        symbol = this.symbol,
                        GIDdokumentuMM = mmgidnumer,
                        nrdokumentuMM = GetNrDokMmp,
                        GIDMagazynuMM = GidMagazynu,
                        DatadokumentuMM = dateTime.ToString("yyyy-MM-dd"),
                        XLGIDMM = xlGidNumer
                    }
                    );
                }
                rs.Close();
                rs.Dispose();
                connection.Close();

            }
            catch (Exception )
            {
                return;
            }
        }

        List<PrzyjmijMMClass> TwrDataList;

        public List<PrzyjmijMMClass> PobierzDaneZKarty(string _twrkod)
        {
            //try
            //{

            TwrDataList = new List<PrzyjmijMMClass>();

            SqlCommand command = new SqlCommand();
                //SqlConnection connection = new SqlConnection
                //{
                //    ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd
                //};
                connection.Open();
                command.CommandText = @"Select twr_kod, twr_nazwa, twr_katalog, cast(twc_wartosc as decimal(5,2))cena ,
                        twr_url, twr_ean
                        from cdn.twrkarty  
                        join cdn.TwrCeny on twr_gidnumer=TwC_TwrNumer and TwC_TwrLp=2
                        where twr_kod=@twr_kod";


                SqlCommand query = new SqlCommand(command.CommandText, connection);
                query.Parameters.AddWithValue("@twr_kod", _twrkod);
                SqlDataReader rs;
                rs = query.ExecuteReader();
                while (rs.Read())
                {

                    TwrDataList.Add( new PrzyjmijMMClass
                    { 
                        twrkod = System.Convert.ToString(rs["twr_kod"]),
                        nazwa = System.Convert.ToString(rs["twr_nazwa"]),
                        symbol = System.Convert.ToString(rs["twr_katalog"]),
                        cena = System.Convert.ToString(rs["cena"]),
                        url = System.Convert.ToString(rs["twr_url"]),
                        ean = System.Convert.ToString(rs["twr_ean"])
                    });
                 
                }
                rs.Close();
                rs.Dispose();
                connection.Close();

            //}
            //catch (Exception exception)
            //{
            //   //return  exception.Message; //DisplayAlert("Uwaga", exception.Message, "OK");
            //}

            return TwrDataList;
        }

       
    }
}
