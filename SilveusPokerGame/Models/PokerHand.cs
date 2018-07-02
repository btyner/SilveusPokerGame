using System;
using System.Collections.Generic;
using System.Linq;

namespace SilveusPokerGame.Models
{
    public class PokerHand : IComparable<PokerHand>
    {
        public Card[] Cards { get; private set; }

        public PokerHand(Card c1, Card c2, Card c3, Card c4, Card c5)
        {
            Cards = new Card[] { c1, c2, c3, c4, c5 };
            Sort();

            if (GetGroupByRankCount(5) != 0)
                throw new Exception("Cannot have five cards with the same rank");
            if (HasDuplicates())
                throw new Exception("Cannot have duplicates");
        }

        public bool Contains(Card card)
        {
            return Cards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit);
        }
        public bool HasDuplicates()
        {
            return Cards.GroupBy(c => new { c.Rank, c.Suit }).Any(c => c.Skip(1).Any());
        }
        public static bool HasDuplicates(IList<PokerHand> hands)
        {
            for (var i = 0; i < hands.Count; i++)
                foreach (var card in hands[i].Cards)
                    for (var j = 0; j < hands.Count; j++)
                        if (i != j && hands[j].Cards.Contains(card))
                            return true;
            return false;
        }

        private void Sort()
        {
            Cards = Cards.OrderBy(c => c.Rank)
             .OrderBy(c => Cards.Where(c1 => c1.Rank == c.Rank).Count())
             .ToArray();

            if (Cards[4].Rank == RankType.Ace
                && Cards[0].Rank == RankType.Two
                && (int)Cards[3].Rank - (int)Cards[0].Rank == 3)
            {
                Cards = new Card[] { Cards[4], Cards[0], Cards[1], Cards[2], Cards[3] };
            }

        }

        public int CompareTo(PokerHand other)
        {
            for (var i = 4; i >= 0; i--)
            {
                RankType rank1 = Cards[i].Rank, rank2 = other.Cards[i].Rank;
                if (rank1 > rank2) return 1;
                if (rank1 < rank2) return -1;
            }
            return 0;
        }

        public enum HandType : int
        {
            RoyalFlush, StraightFlush, FourOfAKind,
            FullHouse, Flush, Straight, ThreeOfAKind, TwoPairs, OnePair, HighCard
        }

        public bool IsValid(HandType handType)
        {
            switch (handType)
            {
                case HandType.RoyalFlush:
                    return IsValid(HandType.StraightFlush) && Cards[4].Rank == RankType.Ace;
                case HandType.StraightFlush:
                    return IsValid(HandType.Flush) && IsValid(HandType.Straight);
                case HandType.FourOfAKind:
                    return GetGroupByRankCount(4) == 1;
                case HandType.FullHouse:
                    return IsValid(HandType.ThreeOfAKind) && IsValid(HandType.OnePair);
                case HandType.Flush:
                    return GetGroupBySuitCount(5) == 1;
                case HandType.Straight:
                    return (((int)Cards[0].Rank + 1 == (int)Cards[1].Rank
                            && (int)Cards[0].Rank + 2 == (int)Cards[2].Rank
                            && (int)Cards[0].Rank + 3 == (int)Cards[3].Rank
                            && (int)Cards[0].Rank + 4 == (int)Cards[4].Rank
                           )
                         || (Cards[0].Rank == RankType.Ace
                             && (int)Cards[1].Rank == 2
                             && (int)Cards[1].Rank + 1 == (int)Cards[2].Rank
                             && (int)Cards[1].Rank + 2 == (int)Cards[3].Rank
                             && (int)Cards[1].Rank + 3 == (int)Cards[4].Rank));
                case HandType.ThreeOfAKind:
                    return GetGroupByRankCount(3) == 1;
                case HandType.TwoPairs:
                    return GetGroupByRankCount(2) == 2;
                case HandType.OnePair:
                    return GetGroupByRankCount(2) == 1;
                case HandType.HighCard:
                    return GetGroupByRankCount(1) == 5;
            }
            return false;
        }

        private int GetGroupByRankCount(int n)
        { return Cards.GroupBy(c => c.Rank).Count(g => g.Count() == n); }

        private int GetGroupBySuitCount(int n)
        { return Cards.GroupBy(c => c.Suit).Count(g => g.Count() == n); }

        public static KeyValuePair<string, string> Evaluate(IDictionary<string, PokerHand> hands)
        {
            if (HasDuplicates(hands.Values.ToList()))
                throw new Exception("There are duplicate cards");

            var len = Enum.GetValues(typeof(HandType)).Length;
            var winners = new List<string>();
            HandType winningType = HandType.HighCard;

            foreach (var name in hands.Keys)
                for (var handType = HandType.RoyalFlush; (int)handType < len; handType = handType + 1)
                {
                    var hand = hands[name];
                    if (hand.IsValid(handType)) // Is hand one of the valid types?
                    {
                        int compareHands = 0, compareCards = 0;
                        if (winners.Count == 0 // Are there any winning hands? If no winning hands, add one.
                         || (compareHands = winningType.CompareTo(handType)) > 0 // Proceed to next check if current handType does not beat winning handType. If it does, add to winners.
                         || compareHands == 0 // Proceed to next check if hands are equal. If current hand is worse than winning hand, do nothing.
                          && (compareCards = hand.CompareTo(hands[winners[0]])) >= 0) // Check to see if current hand beats winning hand. New Winner?
                        {
                            if (compareHands > 0 || compareCards > 0) winners.Clear();
                            winners.Add(name);
                            winningType = handType;
                        }
                        break;
                    }
                }
            KeyValuePair<string, string> winner = new KeyValuePair<string, string>(winners.First(), winningType.ToString());
            return winner;
        }
    }
}
