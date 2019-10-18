using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.IO;
using PokemonBot.Resources.Database;

namespace PokemonBot.Core.Data
{

    public class Turns
    {
        //What turn it is on.
        public int turn { get; set; }
        //Who is player 1.
        public ulong player1 { get; set; }
        //Who is player 2.
        public ulong player2 { get; set; }
        //Which pokemon is active for player 1
        public int p1active { get; set; }
        //Which pokemon is active for player 2
        public int p2active { get; set; }
        
    }

    public class teamData
    {
        public int pokemon1 { get; set; }
        public int pokemon2 { get; set; }
        public int pokemon3 { get; set; }
        public int pokemon4 { get; set; }
        public int pokemon5 { get; set; }
        public int pokemon6 { get; set; }

        public teamData(ulong player)
        {
           
        }
        public int GetPokemon(ulong user, int slot)
        {
            SqliteDbContext db = new SqliteDbContext();
            string query = $"SELECT * FROM \"Teams\" where user = {user}";
            SQLiteCommand cmd = new SQLiteCommand(query, db.myConnection);
            db.OpenConnection();
            cmd.Prepare();
            SQLiteDataReader result = cmd.ExecuteReader();
            int i;
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string intString = result[$"pokemon{slot}"].ToString();
                    i = System.Convert.ToInt32(intString);
                    return i;
                }
            }
            result.Close();
            db.CloseConnection();
            return 0;
        }
    }
}
