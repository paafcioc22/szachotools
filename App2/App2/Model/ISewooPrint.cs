using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public interface ISewooPrint
    {
        Task Print(string drukarka ,int countPrint);
        String Status(string drukarka);

    }
}
