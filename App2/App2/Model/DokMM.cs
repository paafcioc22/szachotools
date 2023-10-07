using App2.Model.ApiModel;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App2.Model
{
    public class DokMM : INotifyPropertyChanged
    {
        public int gidnumer { set; get; }
        public string mag_dcl { set; get; }
        public string twrkod { set; get; }
        public string twrNazwa { set; get; }
        public int szt { set; get; }
        public Int16 fl_header { set; get; }
        public Int16 statuss { set; get; }
        public string opis { set; get; }
        public string nrdok { set; get; }
        public string symbol { set; get; }
        private bool _isExport { set; get; }


        private static string _serwer;
        private static string _database;
        private static string _uid;
        private static string _pwd;
   
        public static ObservableCollection<DokMM> dokMMs = new ObservableCollection<DokMM>();
        //public static ObservableCollection<DokNaglowekDto> dokMMsDto = new ObservableCollection<DokNaglowekDto>();
        public static ObservableCollection<DokMM> dokElementy = new ObservableCollection<DokMM>();
        int ile;

        public Boolean IsExport
        {
            get { return _isExport; }
            set
            {
                _isExport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExport)));
            }
        }

        public string Mag_Nrdok
        {

            get
            {
                if (nrdok == "")
                {
                    return string.Format("{0}  ", mag_dcl);
                }
                else
                {
                    return string.Format("{0} - {1} ", mag_dcl, nrdok);
                }
            }
        }

        private Color _theColor;
        public Color TheColor
        {
            get
            {
                return _theColor;
            }
            set
            {
                _theColor = value;
                OnPropertyChanged("TheColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public DokMM()
        {
            var app = Application.Current as App;
            _serwer = app.Serwer;
            _database = app.BazaProd;
            _uid = app.User;
            _pwd = app.Password;

             

        }

      //  public bool SaveMM(DokMM dokMM)
      //  {

             

      //      string create = "IF object_id('[dbo].[MM]') is null " +
      //                 "begin " +
      //                  "create TABLE[dbo].[MM]( " +
      //                  " [gidnumer][int]  NOT NULL, " +
      //                  " [mag_dcl] [varchar] (5) NULL, " +
      //                  " [kod] [varchar] (50) NULL, " +
      //                  " [ile] [int] NULL, " +
      //                  " [fl_header] [bit] NOT NULL, " +
      //                  " [statuss] [bit] NULL, " +
      //                  " [opis] [varchar] (255) NULL, " +
      //                  " [nrdok] [varchar] (55) NULL, " +
      //                  " [gid_optima] int NULL " +
      //                  ",[IsExport] [bit] NULL" +
      //                  ") ON[PRIMARY]  end " +
      //                  "else " +
      //"IF COL_LENGTH('[dbo].[MM]', 'IsExport') IS NULL " +
      // " begin " +
      // "   alter table[dbo].[MM] " +
      // "   add[IsExport][bit] NULL " +
      //"end";

      //      string query = "declare @id int " +
      //              "set @id = (select IsNull(MAX(gidnumer), 0) from [dbo].[MM])+1 " +
      //              "Insert into [dbo].[MM] values (@id," + //id
      //               "'" + dokMM.mag_dcl + "'," +           //mag_dcl
      //              "null," +                              //kod 
      //              "null," +                               //ile
      //              "1," +                                  //fl_header    
      //              "0," +                                  //status
      //              "'" + dokMM.opis + "'," +                    //opis
      //              "null," +                                //nrdok         
      //              "null," +                                //gidoptima         
      //              Convert.ToByte(dokMM.IsExport) +
      //              ") ";

      //      //SqlCommand command = new SqlCommand( create + " " + query,connection);
      //      SqlCommand command = new SqlCommand(create, connection);
      //      SqlCommand command2 = new SqlCommand(query, connection);
      //      command.ExecuteNonQuery();
      //      command2.ExecuteNonQuery();
      //      connection.Close();

      //      dokMMs.Add(new DokMM
      //      {
      //          gidnumer = dokMM.gidnumer,
      //          mag_dcl = dokMM.mag_dcl,
      //          opis = dokMM.opis,
      //          nrdok = dokMM.nrdok,
      //          statuss = dokMM.statuss,
      //          IsExport = dokMM.IsExport
      //      });

      //      return true;
      //  }

        //public ObservableCollection<DokMM> getMMki()
        //{
         
        //    dokMMs.Clear();
         
          
        //    string query = " IF object_id('[dbo].[MM]') is not null " +
        //        " begin delete mmki " +
        //    "from[dbo].[mm] as mmki " +
        //    "join cdn.tranag on mmki.gid_optima = TrN_TrNId " +
        //    "where trn_bufor = 0 " +
        //        "Select * from [dbo].[MM] where  fl_header=1 order by 1 asc ,fl_header desc end";

          

        //    while (sqlData.Read())
        //    {
 
        //        dokMMs.Add(new DokMM
        //        {
        //            gidnumer = Convert.ToInt32(sqlData["gidnumer"]),
        //            mag_dcl = Convert.ToString(sqlData["mag_dcl"]),
        //            opis = Convert.ToString(sqlData["opis"]),
        //            nrdok = Convert.ToString(sqlData["nrdok"]),
        //            statuss = Convert.ToInt16(sqlData["statuss"]),
        //            _theColor = (statuss == 1 ? Color.Black : Color.Green),
        //            IsExport = sqlData["IsExport"] is DBNull ? false : Convert.ToBoolean(sqlData["IsExport"])
        //        });
        //    }
        //    sqlData.Close();
        //    sqlData.Dispose();
        //    connection.Close(); 

        //    return dokMMs;
        //}

        //public List<DokMM> ListaIstniejacych(string twrkod)
        //{
        //    List<DokMM> dokMMs = new List<DokMM>();
        //    connection.Open();
        //    string query = $@" select gidnumer, mag_dcl, opis, 
        //              (select ile from dbo.[MM] where gidnumer=nad.gidnumer and kod = '{twrkod}')ile
        //              from dbo.mm nad where 
        //              gidnumer in (
			     //               select gidnumer from  dbo.mm pod
			     //               where kod='{twrkod}' and statuss=0  
			     //               )
        //              and [fl_header]=1 and statuss=0  
        //                    ";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    SqlDataReader sqlData = command.ExecuteReader();

        //    while (sqlData.Read())
        //    {

        //        dokMMs.Add(new DokMM
        //        {
        //            gidnumer = Convert.ToInt32(sqlData["gidnumer"]),
        //            mag_dcl = Convert.ToString(sqlData["mag_dcl"]),
        //            opis = Convert.ToString(sqlData["opis"]),
        //            szt = Convert.ToInt16(sqlData["ile"])
        //        });
        //    }
        //    sqlData.Close();
        //    sqlData.Dispose();
        //    connection.Close();

        //    return dokMMs;
        //}

        //public bool ExistsOtherDocs(DokMM dokMM)
        //{
        //    bool tmp = false;

        //    connection.Open();

        //    string query = $" select * from [dbo].[MM]  " +
        //        $"where gidnumer<>{ dokMM.gidnumer} ";
        //    SqlCommand command2 = new SqlCommand(query, connection);
        //    SqlDataReader sqlData = command2.ExecuteReader();
        //    if (sqlData.HasRows)
        //    {
        //        tmp = true;
        //    }

        //    sqlData.Close();
        //    connection.Close();

        //    return tmp;
        //}

        //public int SaveElement(DokMM dokMM)
        //{
        //    connection.Open();

        //    string query = "declare @id varchar(4) " +
        //            "set @id = (select IsNull(MAX(cast(opis as int)), 0) from [dbo].[MM] where gidnumer=" + dokMM.gidnumer + " and fl_header =0 )+1  " +

        //        "Insert into [dbo].[MM] values (" + dokMM.gidnumer + "," + //id
        //             "null," +           //mag_dcl
        //            "'" + dokMM.twrkod + "'," +                              //kod 
        //            +dokMM.szt + "," +                               //ile
        //            "0," +                                  //fl_header    
        //            "0," +                                  //status
        //            "@id," +                    //opis
        //            "null," +                                //nrdok         
        //            "null," +                                //nrdok         
        //            Convert.ToByte(dokMM.IsExport) +
        //            ") ";
        //    string SprCzyIstnieje = $"select isnull(sum(ile),0) ile from dbo.mm where kod='{dokMM.twrkod}' and gidnumer={dokMM.gidnumer}";

        //    SqlCommand command2 = new SqlCommand(SprCzyIstnieje, connection);
        //    SqlDataReader sqlData = command2.ExecuteReader();
        //    while (sqlData.Read())
        //    {
        //        ile = Convert.ToInt32(sqlData["ile"]);
        //    }
        //    if (ile != 0)
        //    {
        //        sqlData.Close();
        //        connection.Close();
        //        return ile;
        //    }
        //    else
        //    {
        //        sqlData.Close();
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.ExecuteNonQuery();
        //        connection.Close();
        //        dokElementy.Add(new DokMM
        //        {
        //            gidnumer = dokMM.gidnumer,
        //            twrkod = dokMM.twrkod,
        //            szt = dokMM.szt,
        //            opis = dokMM.opis   /////<<<<<<<<<<<<<<<<<<<<<<<<<<<< daodane 

        //        });

        //        return 0;
        //    }


        //}

        //public bool UpdateElement(DokMM dokMM)
        //{

        //    connection.Open();

        //    string query = "Update [dbo].[MM] " +
        //        "set ile=" + dokMM.szt + " where gidnumer=" + dokMM.gidnumer + " and kod='" + dokMM.twrkod + "'";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    connection.Close();
             

        //    return true;
        //}

        //public ObservableCollection<DokMM> getElementy(Int32 gidnumer)
        //{
     
        //    dokElementy.Clear(); 

        //    try
        //    {
        //        connection.Open();
        //        string query = "Select m.*, twr_numerkat, twr_nazwa  from [dbo].[MM] m " +
        //            "join cdn.towary on twr_kod=kod " +
        //            "where fl_header =0 and gidnumer=" + gidnumer.ToString() + " order by cast(opis as int)";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            using (SqlDataReader sqlData = command.ExecuteReader())
        //            {
        //                while (sqlData.Read())
        //                {
        //                    dokElementy.Add(new DokMM
        //                    {
        //                        gidnumer = Convert.ToInt32(sqlData["gidnumer"]),
        //                        twrkod = Convert.ToString(sqlData["kod"]),
        //                        szt = Convert.ToInt32(sqlData["ile"]),
        //                        opis = Convert.ToString(sqlData["opis"]),//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< dodane
        //                        symbol = Convert.ToString(sqlData["twr_numerkat"]),//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< dodane
        //                        twrNazwa = Convert.ToString(sqlData["twr_nazwa"])//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< dodane

        //                    });
        //                }
        //            }
                   
        //            connection.Close();
        //        }

        
        //    }
        //    catch (Exception x)
        //    {

        //        Crashes.TrackError(x);
        //    }

        //    return dokElementy;
        //}

        //public static bool DeleteMM(DokMM dokMM)
        //{
        //    SqlCommand command = new SqlCommand();
        //    SqlConnection connection = new SqlConnection
        //    {
        //        ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd
        //    };
        //    connection.Open();


        //    string query = "Delete from [dbo].[MM]  where gidnumer=" + dokMM.gidnumer;

        //    SqlCommand command1 = new SqlCommand(query, connection);
        //    command1.ExecuteNonQuery();
        //    connection.Close();

        //    return true;
        //}

        //public static bool DeleteElementMM(DokMM dokMM)
        //{
        //    SqlCommand command = new SqlCommand();
        //    SqlConnection connection = new SqlConnection
        //    {
        //        ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd
        //    };
        //    connection.Open();


        //    string query = "Delete from [dbo].[MM]  where fl_header =0 and kod='" + dokMM.twrkod + "' and gidnumer=" + dokMM.gidnumer;

        //    SqlCommand command2 = new SqlCommand(query, connection);
        //    command2.ExecuteNonQuery();
        //    connection.Close();

        //    return true;
        //}

        //public bool UpdateMM(DokMM dokMM)
        //{
        //    //SqlCommand command = new SqlCommand();
        //    //SqlConnection connection = new SqlConnection
        //    //{
        //    //    ConnectionString = "SERVER=" + MainPage._serwer + ";DATABASE=" + MainPage._database + ";TRUSTED_CONNECTION=No;UID=" + MainPage._uid + ";PWD=" + MainPage._pwd
        //    //};
        //    connection.Open();


        //    string query = $@"Update [dbo].[MM] 
        //            set mag_dcl='{dokMM.mag_dcl}'
        //            ,opis='{dokMM.opis}'  
        //            ,IsExport={Convert.ToByte(dokMM.IsExport)}                    
        //            where fl_header=1 and gidnumer=" + dokMM.gidnumer;

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    connection.Close();

        //    return true;
        //}

        public class StringToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var valueAsString = value.ToString();
                switch (valueAsString)
                {
                    case (""):
                        {
                            return Color.Default;
                        }
                    case ("Accent"):
                        {
                            return Color.Accent;
                        }
                    default:
                        {
                            return Color.FromHex(value.ToString());
                        }
                }
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }

            public object ProvideValue(IServiceProvider serviceProvider)
            {
                return this;
            }
        }

    }






}
