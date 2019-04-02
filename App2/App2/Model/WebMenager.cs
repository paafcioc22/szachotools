﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class WebMenager
    {

        IMagazynSerwis soapService;

        public WebMenager(IMagazynSerwis service)
        {
            soapService = service;
        }

        public Task<List<Magazynn>> GetTodoItemsAsync(string query)
        {
            return soapService.GetAllCustomers(query);
        }

        public Task<string> InsertDataNiezgodnosci(ObservableCollection<Model.ListDiffrence> insert)
        {
             return soapService.InsertDataNiezgodnosci(insert);
        }

        public Task<List<RaportListaMM>> PobierzTwrAsync(string ean)
        {
            return soapService.PobierzTwr(ean);
        }

        public Task<string> GetBuildVer()
        {
            return soapService.PobierzWersjeApki();
        }

    }
}
