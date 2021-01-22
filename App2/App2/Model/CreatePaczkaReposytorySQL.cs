﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class CreatePaczkaReposytorySQL
    {
        public CreatePaczkaReposytorySQL()
        {


        }

        public bool SaveZlecenieToBase(FedexPaczka fedex, int gidnr, int nrpaczki = 0)
        {
            bool wykonano = false;




            var query = $@" cdn.PC_WykonajSelect 'declare @id int  = (select IsNull(MAX(Fmm_EleNumer), 0) from cdn.pc_fedexmm
                            where fmm_gidnumer={gidnr})+1

                        set @id= (select case when {nrpaczki}<>0 then {nrpaczki} else @id end)

                    insert into cdn.pc_fedexmm values " + Environment.NewLine;

            string insert = "";
            string NryMMek = "Numery MM(ek):";

            //foreach (var i in checkMMToPaczkaList)
            //{
            //    NryMMek += i.Trn_Numer + ", ";

            //}



            query += $"({gidnr},@id,'','','','',getdate(),{fedex.Fmm_MagDcl},{fedex.Fmm_MagZrd},'')'";//+ Environment.NewLine;
                                                                                        //}
            var odp = App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);
         
            return wykonano;
        }


        public async Task<int> GetLastGidNUmer()
        {
            var query = $@" cdn.PC_WykonajSelect 'select IsNull(MAX(Fmm_gidnumer), 0)+1 Fmm_gidnumer, from cdn.pc_fedexmm'";


            int lastGid = 0;

            var odp = await App.TodoManager.PobierzDaneZWeb<FedexPaczka>(query);

            if (odp.Count > 0)
                lastGid = odp[0].Fmm_GidNumer;
            return lastGid;
        }



    }
}
