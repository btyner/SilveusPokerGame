using System;

namespace SilveusPokerGame.Models
{
    public struct Player
    {
        public Player(string name) : this() { Name = name; }

        public string Name { get; private set; }
        public PokerHand Hand { get; set; }
    }
}
