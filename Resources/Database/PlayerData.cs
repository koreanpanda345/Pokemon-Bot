using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PokemonBot.Resources.Database
{
    public class PlayerData
    {
        [Key]
        public ulong UserId { get; set; }
        public int credits { get; set; }
        public string pokeName { get; set; }
        public string level { get; set; }
        public string IV { get; set; }

    }
}
