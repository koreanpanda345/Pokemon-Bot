using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SQLite;
using PokemonBot.Resources.Database;

namespace PokemonBot.Core.Data
{
    public static class BattleData
    {
        public static async Task CreateBattle(ulong id1, ulong id2)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = "INSERT INTO battles(Challengerid, Challengedid, IsBattling)VALUES(@challengerid, @challengedid, @isBattling);";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@challengerid", id1);
            myCommand.Parameters.AddWithValue("@challengedid", id2);
            myCommand.Parameters.AddWithValue("@isBattling", false);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine("Battle was made.");
        }
        public static async Task StartBattle(ulong id)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE 'battles' SET IsBattling = @isBattling WHERE Challengedid = {id}, IsBattling = false";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@isBattling", "true");
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine("Set IsBattling to True");
        }
        public static int GetDuelId(ulong id)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT DuelId, * FROM battles WHERE Challengedid = {id}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int i;
                    string intString = result["Challengerid"].ToString();
                    i = System.Convert.ToInt32(intString);
                    Console.WriteLine($"{result}");
                    return i;
                }
            }
            result.Close();
            database.CloseConnection();
            return 0;
        }
        public static ulong GetChallenger(int Duelid)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT Challengerid, * FROM battles WHERE DuelId = {Duelid}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    ulong i;
                    string intString = result["Challengerid"].ToString();
                    i = System.Convert.ToUInt64(intString);
                    Console.WriteLine($"{result}");
                    return i;
                }
            }
            result.Close();
            database.CloseConnection();
            return 0;
        }
    }
}
