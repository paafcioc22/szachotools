using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.SQLite
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
