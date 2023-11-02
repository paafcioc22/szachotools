using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public interface IDatabaseExporter
    {
        Task ExportDatabaseAsync();
        void RequestStoragePermission();

    }
}
