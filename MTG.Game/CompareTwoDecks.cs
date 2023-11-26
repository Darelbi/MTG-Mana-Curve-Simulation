using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Game
{
    public class CompareTwoDecks
    {

        public CompareTwoDecks( Deck deck1, Deck deck2)
        {
            PrintDeck(deck1);
            PrintDeck(deck2);

            TextWriter standardOutput = Console.Out;
            Console.SetOut(StreamWriter.Null);

            var simulation1 = new Simulation(deck1, 100000);
            simulation1.Run();

            var winningTurn1 = simulation1.GetAverageWinningTurn();

            var simulation2 = new Simulation(deck2, 100000);
            simulation2.Run();

            var winningTurn2 = simulation2.GetAverageWinningTurn();

            Console.SetOut(standardOutput);

            if (winningTurn1 < winningTurn2)
            {
                Console.WriteLine($"Deck 1 is better by deltaAWT:{winningTurn2 - winningTurn1}");
            }
            else
            {
                Console.WriteLine($"Deck 2 is better by deltaAWT:{winningTurn1 - winningTurn2}");
            }
        }

        void PrintDeck(Deck deck)
        {
            int totalCards = 0;
            foreach (var card in deck.Cards())
            {
                totalCards += deck.GetCardCount(card);
                Console.WriteLine($" - {deck.GetCardCount(card)}\tx {deck.GetFactory(card)().CardName}");
            }
            Console.WriteLine($"Total: {totalCards} Total Cards\n");
        }
    }
}
