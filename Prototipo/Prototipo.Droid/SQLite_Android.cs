using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using Prototipo;
using Prototipo.Droid;
using System.IO;

[assembly: Dependency (typeof (SQLite_Android))]

namespace Prototipo.Droid
{
    public class SQLite_Android : ISQLite
    {

        public SQLite_Android()
        {

        }

        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "wiishperdb.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            if(!File.Exists(path))
            {
                var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.wiishperdb);
                FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
            }

            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }

        void ReadWriteStream(Stream readStream, Stream writeSream)
        {
            int length = 256;
            Byte[] buffer = new Byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);

            while(bytesRead > 0)
            {
                writeSream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }

            readStream.Close();
            writeSream.Close();
        }
    }
}