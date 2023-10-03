using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class ApiResponse<T> //where T : class   
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public string Path { get; set; }
    }
}
