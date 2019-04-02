using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using App2.SQLite;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(App2.Droid.SQLiteDb))]

namespace App2.Droid
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}
