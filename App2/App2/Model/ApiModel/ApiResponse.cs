using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace App2.Model.ApiModel
{
    public class ApiResponse<T> //where T : class   
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public string Path { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public ConflictInformation ConflictInformation { get; set; }
    }

    public class ConflictInformation
    {
        public string TwrKod { get; set; }
        public int ExistingQuantity { get; set; }
        public int AttemptedToAddQuantity { get; set; }
        public int IdElement { get; set; }
    }
}
