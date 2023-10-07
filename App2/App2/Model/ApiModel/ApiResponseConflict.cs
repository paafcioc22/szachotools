using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public class ApiResponseConflict<T>
    {
        public string Message { get; set; }
        public T Element { get; set; }
    }
}
