using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Memento.Droid.Persistence;
using Memento.Persistence;
using Android.Runtime;
using SQLite;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(SQLiteDb))]

namespace Memento.Droid.Persistence
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