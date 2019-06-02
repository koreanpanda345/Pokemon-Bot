using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data.SQLite;

namespace PokemonBot.Resources.Database
{
    public class SqliteDbContext
    {
        public SQLiteConnection myConnection;
        public SqliteDbContext()
        {
            myConnection = new SQLiteConnection(@"Data Source=Database.sqlite3");
            if (!File.Exists("./Database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
                Console.WriteLine("Database is created");
            }


        }
        public void OpenConnection()
        {
            if(myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }
        
        public void CloseConnection()
        {
            if(myConnection.State != System.Data.ConnectionState.Closed)
            {
                myConnection.Close();
            }
        }
        public void SetConnection()
        {
            myConnection.Close();
        }
    }
}
