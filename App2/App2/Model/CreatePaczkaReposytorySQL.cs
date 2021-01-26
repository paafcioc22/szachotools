using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace App2.Model
{
    public class CreatePaczkaReposytorySQL
    {


        private static string _serwer;
        private static string _database;
        private static string _uid;
        private static string _pwd;
        SqlConnection connection;
        public CreatePaczkaReposytorySQL()
        {
            var app = Application.Current as App;
            _serwer = app.Serwer;
            _database = app.BazaProd;
            _uid = app.User;
            _pwd = app.Password;
            connection = new SqlConnection
            {

                ConnectionString = "SERVER=" + _serwer + ";DATABASE=" + _database + ";TRUSTED_CONNECTION=No;UID=" + _uid + ";PWD=" + _pwd

            };

        }

        public async Task<bool> SaveZlecenieToBase(FedexPaczka fedex, int gidnr, int nrpaczki = 0)
        {
            bool wykonano = false;

            var query = $@" cdn.PC_WykonajSelect 'declare @id int  = (select IsNull(MAX(Fmm_EleNumer), -1) from cdn.pc_fedexmm
                            where fmm_gidnumer={gidnr})+1

                        set @id= (select case when {nrpaczki}<>0 then {nrpaczki} else @id end)

                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

            string insert = "";
            string NryMMek = "Numery MM(ek):";

            //foreach (var i in checkMMToPaczkaList)
            //{
            //    NryMMek += i.Trn_Numer + ", ";

            //}

            query += $"({gidnr},@id,'''','''','''','''',getdate(),{fedex.Fmm_MagDcl},{fedex.Fmm_MagZrd},'''',''{fedex.Fmm_Opis}'')'";//+ Environment.NewLine;
                                                                                        //}
            var odp =await  App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);
         
            return wykonano;
        }


        public async Task<int> GetLastGidNUmer()
        {
            var query = $@" cdn.PC_WykonajSelect 'select IsNull(MAX(Fmm_gidnumer), 0)+1 Fmm_gidnumer from cdn.pc_fedexmm'";

            FedexPaczka fedexPaczka = new FedexPaczka();

            int lastGid = 0;

              


            var odp = await App.TodoManager.PobierzDaneZWeb<ff>(query);

            if (odp.Count > 0)
                lastGid = odp[0].Fmm_gidnumer;
            return lastGid;
        }

        internal async Task<bool> IsNotReadyAdded(string text)
        {
            bool wykonano = false;

            var query = $@" cdn.PC_WykonajSelect 'select Fmm_NazwaPaczki from cdn.PC_FedexMM
                        where Fmm_NazwaPaczki=''{text}'' '";

            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);
            if (odp.Count==0)
                wykonano = true;

            return wykonano;
        }

        public async Task<bool> SavePaczkaToBase(FedexPaczka fedex, int gidnr, int nrpaczki = 0)
        {
            bool wykonano = false;

            var query = $@" cdn.PC_WykonajSelect 'declare @id int  = (select IsNull(MAX(Fmm_EleNumer), -1) from cdn.pc_fedexmm
                            where fmm_gidnumer={gidnr})+1,@magdcl int, @magzrd int
            
                        set @id= (select case when {nrpaczki}<>0 then {nrpaczki} else @id end)
                        set @magdcl=(select top 1 Fmm_MagDcl from cdn.pc_fedexmm  where fmm_gidnumer={gidnr})
						set @magzrd=(select top 1 Fmm_MagZrd from cdn.pc_fedexmm  where fmm_gidnumer={gidnr})


                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

            string insert = "";
            string NryMMek = "Numery MM(ek):";
            string nn = "karton";
          
            query += $"({gidnr},@id,'''',''karton'','''','''',getdate(),@magdcl,@magzrd,'''','''')'";//+ Environment.NewLine;
                                                                                                                //}
            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);

            return wykonano;
        }

        internal async Task<bool> RemovePaczka(FedexPaczka fp, string mm="")
        {
            string mmUsun = "";

            if (!string.IsNullOrEmpty(mm))
            {

                  mmUsun = $"and Fmm_NazwaPaczki=''{fp.Fmm_NazwaPaczki}''";
            }
            
            var query = $@" cdn.PC_WykonajSelect 'declare @numery varchar(55) delete from cdn.PC_FedexMM where Fmm_gidnumer={fp.Fmm_GidNumer} and Fmm_EleNumer={fp.Fmm_EleNumer} {mmUsun}";

            var update = Environment.NewLine + $@"set @numery=(select +cast(Cast(PARSENAME(REPLACE(fmm_nazwapaczki,''/'',''.''), 3)  as int)as varchar(66)) +'','' from 
                                                cdn.PC_FedexMM where Fmm_GidNumer={fp.Fmm_GidNumer} and Fmm_EleNumer={fp.Fmm_EleNumer} order by fmm_nazwapaczki For XML PATH ('''')) 
                                        
                                                update cdn.PC_FedexMM 
                                                set Fmm_Elmenty= ''Numery MM(ek):''+  substring(@numery,0,len(@numery) )        
                                                where Fmm_GidNumer={fp.Fmm_GidNumer} and Fmm_EleNumer={fp.Fmm_EleNumer}
                                                    '";

            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query+update);
            return true;
        }

        internal async Task<bool> SaveMMToBase(FedexPaczka fp)
        {
            bool wykonano = false;

            var query = $@" cdn.PC_WykonajSelect 'declare @id int  = (select IsNull(MAX(Fmm_EleNumer), -1) from cdn.pc_fedexmm
                            where fmm_gidnumer={fp.Fmm_GidNumer})+1,@magdcl int, @magzrd int, @numery varchar(55)

                        
                        set @magdcl=(select top 1 Fmm_MagDcl from cdn.pc_fedexmm  where fmm_gidnumer={fp.Fmm_GidNumer})
						set @magzrd=(select top 1 Fmm_MagZrd from cdn.pc_fedexmm  where fmm_gidnumer={fp.Fmm_GidNumer})

                    delete from cdn.pc_fedexmm where fmm_gidnumer={fp.Fmm_GidNumer} and fmm_eleNumer={fp.Fmm_EleNumer} and Fmm_NazwaPaczki=''''
                    
                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

            string insert = "";
            string NryMMek = "Numery MM(ek):";
            string nn = "karton";

            query += $"({fp.Fmm_GidNumer},{fp.Fmm_EleNumer},'''',''karton'',''{fp.Fmm_NazwaPaczki}'','''',getdate(),@magdcl,@magzrd,'''','''')";//+ Environment.NewLine;

            var update = Environment.NewLine + $@"set @numery=(select +cast(Cast(PARSENAME(REPLACE(fmm_nazwapaczki,''/'',''.''), 3)  as int)as varchar(66)) +'','' from 
                                                cdn.PC_FedexMM where Fmm_GidNumer={fp.Fmm_GidNumer} and Fmm_EleNumer={fp.Fmm_EleNumer} order by fmm_nazwapaczki For XML PATH ('''')) 
                                        
                                                update cdn.PC_FedexMM 
                                                set Fmm_Elmenty= ''Numery MM(ek):''+  substring(@numery,0,len(@numery) )        
                                                where Fmm_GidNumer={fp.Fmm_GidNumer} and Fmm_EleNumer={fp.Fmm_EleNumer}
                                                    '";

            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query+update);

            return wykonano;
        }


        public  bool IsMMOKToSave(string text,string fmm_magdcl)
        {
            bool dozapis=false;
            var query = $@"
              select     dcl.Mag_GIDNumer magzrd, trn_bufor
              from cdn.tranag
	            join cdn.magazyny dcl on dcl.mag_magid=TrN_MagDocId 
	            join cdn.magazyny zr on zr.mag_magid=TrN_MagZrdId 
              where TrN_TypDokumentu=312  
              and trn_bufor=0 and dcl.Mag_GIDNumer={fmm_magdcl}
			  and trn_magzrdid=1 
			  and TrN_NumerPelny='{text}'
                    ";//and trn_Stan = 1


            
                connection.Open();
                using (SqlCommand command2 = new SqlCommand(query, connection))
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    if (reader.HasRows)
                        dozapis = true;
                }

                connection.Close();

            return dozapis;
        }





        [XmlType("Table")]
        public class ff
        {
            public int Fmm_gidnumer { get; set; }
        }

    }
}
