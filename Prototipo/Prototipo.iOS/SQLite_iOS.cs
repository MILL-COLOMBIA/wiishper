using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Prototipo.iOS;
using System.IO;

[assembly: Dependency (typeof (SQLite_iOS))]

namespace Prototipo.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }

        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "wiishperdb.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}
