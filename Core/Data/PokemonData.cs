using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PokemonBot.Resources.Database;
using System.Linq;
using System.Data.SQLite;
using Newtonsoft.Json;
namespace PokemonBot.Core.Data
{
    public class Id
    {
        public ulong Userid { get; set; }
        public int PokemonNumber { get; set; }
    }
    public static class PokemonData
    {
        public static int GetXp(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            int i;
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string intString = result["xp"].ToString();
                    i = System.Convert.ToInt32(intString);
                    Console.WriteLine($"{result}");
                    return i;
                }
            }
            result.Close();
            database.CloseConnection();
            return 0;
        }
        public static async Task ResetXp(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE '{id.ToString()}' SET xp = @xp WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@xp", 0);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
        }
        public static async Task AddXp(ulong id, int num, int xp)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE '{id.ToString()}' SET xp = @xp WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@xp", GetXp(id, num) + xp);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine("Added xp");
        }
        public static async Task LevelUp(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE '{id.ToString()}' SET level = @level WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@level", GetLevel(id, num) + 1);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
        }
        public static int GetSelected(ulong id)
        {
            SqliteDbContext database = new SqliteDbContext();
            //SELECT "_rowid_",* FROM "main"."koreanpanda345" ORDER BY "_rowid_" ASC LIMIT 0, 49999;
            string query = $"SELECT selected, * FROM player WHERE id = '{id.ToString()}'";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int i;
                    string intString = result["selected"].ToString();
                    i = System.Convert.ToInt32(intString);
                    Console.WriteLine($"{result}");
                    return i;
                }
            }
            result.Close();
            database.CloseConnection();
            return 0;

        }
        public static string GetNature(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine($"{result}");
                    return $"{result["nature"]}";
                }
            }
            result.Close();
            database.CloseConnection();
            return "";
        }
        public static string GetPokemon(ulong id, int num)
        {

            /***
             * Create data, Need to extract it based on the Id, instead of The row id number.
             */
            SqliteDbContext database = new SqliteDbContext();
            //SELECT "_rowid_",* FROM "main"."koreanpanda345" ORDER BY "_rowid_" ASC LIMIT 0, 49999;
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine($"{result}");
                    return $"{result["pokemon"]}";
                }
            }
            result.Close();
            database.CloseConnection();
            Console.WriteLine(database.myConnection);
            return "";


        }

        public static int GetLevel(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine($"{result["level"]}");
                    return (int)result["level"];
                }
            }
            result.Close();
            database.CloseConnection();
            Console.WriteLine(database.myConnection);
            return 0;
        }
        public static string GetMoves(ulong id, int num)
        {
            int select = GetSelected(id);
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {select}, * FROM '{id.ToString()}' WHERE id = {select}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    return $"{result[$"Move{num}"]}";
                }
            }
            result.Close();
            database.CloseConnection();
            Console.WriteLine(database.myConnection);
            return "";
        }
        public static int GetIvs(ulong id, int num, int iv)
        {
            List<int>IVs = new List<int>();
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    IVs.Add(((int)result["hp"]));
                    IVs.Add((int)result["atk"]);
                    IVs.Add((int)result["def"]);
                    IVs.Add((int)result["spatk"]);
                    IVs.Add((int)result["spdef"]);
                    IVs.Add((int)result["spe"]);
                }
            }
            result.Close();
            database.CloseConnection();
            return IVs[iv];

        }
        public static async Task CreateAccount( ulong id, string PokeName, int level, int[] Iv, string nature)
        {
            string LevelString = level.ToString();
            SqliteDbContext database = new SqliteDbContext();

            string query = "INSERT INTO player (`Id`, `credits`) VALUES(@Id, @credits)";
                SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            Console.WriteLine(database.myConnection.State);
            myCommand.Parameters.AddWithValue("@Id", $"{id.ToString()}");
                myCommand.Parameters.AddWithValue("@credits", 2000);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine("Player info has been stored");

            string pokemonQuery = $"CREATE TABLE IF NOT EXISTS '{id.ToString()}'(id varchar(24), pokemon varchar(24), level int, nature varchar(24), hp int, atk int, def int, spatk int, spdef int, spe int, shiny int, Move1 text, Move2 text, Move3 text, Move4 text)";
                SQLiteCommand newcommand = new SQLiteCommand(pokemonQuery, database.myConnection);
            database.OpenConnection();
            newcommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine("Passed the Database section");
            string pokeQuery = $"INSERT INTO '{id.ToString()}'(`id`,`pokemon`, `level`, nature, hp, atk, def,spatk,spdef,spe, shiny, Move1, Move2, Move3, Move4) VALUES(@id, @pokemon, @level, @nature, @hp, @atk,@def, @spatk, @spdef, @spe, @shiny, @Move1, @Move2, @Move3, @Move4);";
                SQLiteCommand _command = new SQLiteCommand(pokeQuery, database.myConnection);
            database.OpenConnection();
            _command.Parameters.AddWithValue("@id", $"1");
                _command.Parameters.AddWithValue("@pokemon", $"{PokeName}");
                _command.Parameters.AddWithValue("@level", $"{level}");
            _command.Parameters.AddWithValue("@nature", $"{nature}");
            _command.Parameters.AddWithValue("@hp", $"{Iv[0]}");
            _command.Parameters.AddWithValue("@atk", $"{Iv[1]}");
            _command.Parameters.AddWithValue("@def", $"{Iv[2]}");
            _command.Parameters.AddWithValue("@spatk", $"{Iv[3]}");
            _command.Parameters.AddWithValue("@spdef", $"{Iv[4]}");
            _command.Parameters.AddWithValue("@spe", $"{Iv[5]}");
            _command.Parameters.AddWithValue("@shiny", $"0");
            _command.Parameters.AddWithValue("@Move1", $"Tackle");
            _command.Parameters.AddWithValue("@Move2", $"Tackle");
            _command.Parameters.AddWithValue("@Move3", $"Tackle");
            _command.Parameters.AddWithValue("@Move4", $"Tackle");
            _command.ExecuteNonQuery();
            
            database.CloseConnection();

            Console.WriteLine(database.myConnection.State);
            Console.WriteLine($"Created a new row with {PokeName}, {level}");
        }
        public static int GetId(ulong id)
        {
            int i = 0;
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT * FROM '{id.ToString()}' ORDER BY id DESC LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string intString = result["id"].ToString();
                    i = System.Convert.ToInt32(intString);
                    return i;
                }
            }

            result.Close();
            database.CloseConnection();
            Console.WriteLine(database.myConnection);
            return 0;
        }

        public static bool IsShiny(ulong id, int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT {num}, * FROM '{id.ToString()}' WHERE id = {num}";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int i;
                    string intString = result["shiny"].ToString();
                    i = System.Convert.ToInt32(intString);
                    if(i == 1)
                    {
                        Console.WriteLine($"{result}");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            database.CloseConnection();
            return false;
        }
        public static async Task SavePokemon(ulong id, string PokeName, int level, int[] Iv, string nature, int shiny)
        {
            int i = 0;
            SqliteDbContext database = new SqliteDbContext();
            string query = $"SELECT * FROM '{id.ToString()}' ORDER BY id DESC LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string intString = result["id"].ToString();
                     i = System.Convert.ToInt32(intString);

                }
            }
            result.Close();
            database.CloseConnection();
            string pokeQuery = $"INSERT INTO '{id.ToString()}'(`id`,`pokemon`, `level`, nature, hp, atk, def,spatk,spdef,spe, shiny) VALUES(@id, @pokemon, @level, @nature, @hp, @atk,@def, @spatk, @spdef, @spe, @shiny);";
            SQLiteCommand _command = new SQLiteCommand(pokeQuery, database.myConnection);

            database.OpenConnection();
            _command.Parameters.AddWithValue("@id", $"{ i + 1}");
            _command.Parameters.AddWithValue("@pokemon", $"{PokeName}");
            _command.Parameters.AddWithValue("@level", $"{level}");
            _command.Parameters.AddWithValue("@nature", $"{nature}");
            _command.Parameters.AddWithValue("@hp", $"{Iv[0]}");
            _command.Parameters.AddWithValue("@atk", $"{Iv[1]}");
            _command.Parameters.AddWithValue("@def", $"{Iv[2]}");
            _command.Parameters.AddWithValue("@spatk", $"{Iv[3]}");
            _command.Parameters.AddWithValue("@spdef", $"{Iv[4]}");
            _command.Parameters.AddWithValue("@spe", $"{Iv[5]}");
            _command.Parameters.AddWithValue("@shiny", shiny);
            _command.Parameters.AddWithValue("@Move1", $"Tackle");
            _command.Parameters.AddWithValue("@Move2", $"Tackle");
            _command.Parameters.AddWithValue("@Move3", $"Tackle");
            _command.Parameters.AddWithValue("@Move4", $"Tackle");
            _command.ExecuteNonQuery();

            database.CloseConnection();

            Console.WriteLine(database.myConnection.State);
            Console.WriteLine($"Created a new row with {PokeName}, {level}");

        }

        /**
         * SELECT TOP 1 * FROM MyTable ORDER BY MyColumn DESC
         * 
         */

    }
}
