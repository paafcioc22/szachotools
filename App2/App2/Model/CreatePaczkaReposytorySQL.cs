using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App2.Model
{
    public class CreatePaczkaReposytorySQL
    {
        public CreatePaczkaReposytorySQL()
        {


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

            query += $"({gidnr},@id,'''','''','''','''',getdate(),{fedex.Fmm_MagDcl},{fedex.Fmm_MagZrd},'''')'";//+ Environment.NewLine;
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
          
            query += $"({gidnr},@id,'''',''karton'','''','''',getdate(),@magdcl,@magzrd,'''')'";//+ Environment.NewLine;
                                                                                                                //}
            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);

            return wykonano;
        }

        internal async Task<bool> SaveMMToBase(FedexPaczka fedexPaczka, int fmm_gidnumer, int fmm_eleNumer)
        {
            bool wykonano = false;

            var query = $@" cdn.PC_WykonajSelect 'declare @id int  = (select IsNull(MAX(Fmm_EleNumer), -1) from cdn.pc_fedexmm
                            where fmm_gidnumer={fmm_gidnumer})+1,@magdcl int, @magzrd int

                        
                        set @magdcl=(select top 1 Fmm_MagDcl from cdn.pc_fedexmm  where fmm_gidnumer={fmm_gidnumer})
						set @magzrd=(select top 1 Fmm_MagZrd from cdn.pc_fedexmm  where fmm_gidnumer={fmm_gidnumer})

                    delete from cdn.pc_fedexmm where fmm_gidnumer={fmm_gidnumer} and fmm_eleNumer={fmm_eleNumer} and Fmm_NazwaPaczki=''''
                    
                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

            string insert = "";
            string NryMMek = "Numery MM(ek):";
            string nn = "karton";

            query += $"({fmm_gidnumer},{fmm_eleNumer},'''',''karton'',''{fedexPaczka.Fmm_NazwaPaczki}'','''',getdate(),@magdcl,@magzrd,'''')'";//+ Environment.NewLine;
                                                                                               
            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);

            return wykonano;
        }

        [XmlType("Table")]
        public class ff
        {
            public int Fmm_gidnumer { get; set; }
        }

    }
}
