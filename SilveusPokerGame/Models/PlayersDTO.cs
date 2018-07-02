using System;
using System.ComponentModel.DataAnnotations;

namespace SilveusPokerGame.Models
{
    public struct PlayersDTO
    {
		[MinLength(3)]
        public string Player1 { get; set; }
        [MinLength(3)]
        public string Player2 { get; set; }
    }
}
