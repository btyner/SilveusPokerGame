using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilveusPokerGame.Models;

namespace SilveusPokerGame.Services
{
    public class PokerService
    {
        private static Player player1;
        private static Player player2;
        private static List<Card> activeCards = new List<Card>();
        private static List<Card> cardList = new List<Card>();
        private static Array cardRanks = Enum.GetValues(typeof(RankType));
        private static Array cardSuits = Enum.GetValues(typeof(SuitType));
        private static Random random = new Random();

        public PokerService()
        {
        }

        public static PlayerHandsDTO StartGame(PlayersDTO players)
        {
            activeCards.Clear();

            player1 = new Player(players.Player1);
            player2 = new Player(players.Player2);

            player1.Hand = DealHand();
            player2.Hand = DealHand();

            return new PlayerHandsDTO() { Player1Hand = player1.Hand, Player2Hand = player2.Hand };;
        }

        private static PokerHand DealHand()
        {

            cardList.Clear();

            for (var i = 0; i < 5; i++)
            {

                Card card = GetRandomCard();

                while (CardIsInPlay(card))
                {
                    card = GetRandomCard();
                }

                activeCards.Add(card);
                cardList.Add(card);

            }

            return new PokerHand(cardList[0], cardList[1], cardList[2], cardList[3], cardList[4]);

        }

        private static Card GetRandomCard()
        {
            RankType rank = (RankType)cardRanks.GetValue(random.Next(cardRanks.Length));
            SuitType suit = (SuitType)cardSuits.GetValue(random.Next(cardSuits.Length));

            return new Card(rank, suit);;
        }

        private static bool CardIsInPlay(Card card)
        {
            if (activeCards.Contains(card))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static WinnerDTO GetWinner()
        {
            KeyValuePair<string, PokerHand> winningPlayer1 = new KeyValuePair<string, PokerHand>(player1.Name, player1.Hand);
            KeyValuePair<string, PokerHand> winningPlayer2 = new KeyValuePair<string, PokerHand>(player2.Name, player2.Hand);
            IDictionary<string, PokerHand> potentialWinners = new Dictionary<string, PokerHand>();

            potentialWinners.Add(winningPlayer1);
            potentialWinners.Add(winningPlayer2);

            KeyValuePair<string, string> winners = PokerHand.Evaluate(potentialWinners);

            return new WinnerDTO() { Winner = winners.Key, Type = winners.Value };
        }
    }
}
