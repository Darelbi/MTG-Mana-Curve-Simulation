using MTG.Cards;

namespace MTG.Game
{
    public class DeckSideboardFinder
    {
        private readonly Deck startingDeck;
        private readonly Deck tryList;
        private readonly int simulations;

        public DeckSideboardFinder(Deck startingDeck, Deck tryList, int simulations)
        {
            this.startingDeck = startingDeck;
            this.tryList = tryList;
            this.simulations = simulations;
        }

        void PrintDeck()
        {
            int totalCards = 0;
            foreach (var card in startingDeck.Cards())
            {
                totalCards += startingDeck.GetCardCount(card);
                Console.WriteLine($" - {startingDeck.GetCardCount(card)}\tx {startingDeck.GetFactory(card)().CardName}");
            }
            Console.WriteLine($"Total: {totalCards} Total Cards\n");
        }

        IEnumerable<(Func<Card> Removed, Func<Card> Added, double turn)> AllPossibilities()
        {
            // first simulate the deck to have a compare point 
            var simulation = new Simulation(startingDeck, simulations);
            simulation.Run().GetAwaiter().GetResult();

            double lowerBound = simulation.GetAverageWinningTurn();
            double upperBound = double.MaxValue;

            Type betterRemoved = null;
            Type betterAdded = null;

            for (int i = 0; i < 1000; i++)
            {

                var typesToTryRemove = startingDeck.Cards().ToList();
                foreach (var types in typesToTryRemove)
                {
                    int previousNumber = startingDeck.GetCardCount(types);
                    var previousFactory = startingDeck.GetFactory(types);

                    var attemps = tryList.Cards().ToList();

                    foreach (var attemp in attemps)
                    {
                        startingDeck.SetCards(previousNumber, types, previousFactory);

                        if (attemp != types) // save identical simulations
                        {
                            startingDeck.SetCards(previousNumber - 1, types, previousFactory);
                            startingDeck.SetCards(
                                startingDeck.GetCardCount(attemp) + 1
                                , attemp, tryList.GetFactory(attemp));

                            simulation = new Simulation(startingDeck, simulations);
                            simulation.Run().GetAwaiter().GetResult();

                            var winningTurn = simulation.GetAverageWinningTurn();

                            bool condition = winningTurn > lowerBound && winningTurn < upperBound;
                            if (condition)
                            {
                                upperBound = winningTurn;
                                betterAdded = attemp;
                                betterRemoved = types;
                            }

                            startingDeck.SetCards(
                               startingDeck.GetCardCount(attemp) - 1
                               , attemp, tryList.GetFactory(attemp));
                        }

                        startingDeck.SetCards(previousNumber, types, previousFactory);
                    }
                }



                if (betterRemoved != null)
                {
                    // Ok first tranche of simulations runned now apply permanently the change to the deck
                    int countRemoved = startingDeck.GetCardCount(betterRemoved) - 1;
                    var factoryRemoved = startingDeck.GetFactory(betterRemoved);
                    startingDeck.SetCards(countRemoved, betterRemoved, factoryRemoved);

                    int countAdded = startingDeck.GetCardCount(betterAdded) + 1;
                    var factoryAdded = tryList.GetFactory(betterAdded);
                    startingDeck.SetCards(countAdded, betterAdded, factoryAdded);

                    // also remove the card from trylist
                    int countInjected = tryList.GetCardCount(betterAdded) - 1;
                    tryList.SetCards(countInjected, betterAdded, factoryAdded);
                    yield return (factoryRemoved, factoryAdded, upperBound);

                    lowerBound = upperBound;
                    upperBound = double.MaxValue;
                    betterRemoved = null;
                    betterAdded = null;
                }
                else
                    yield break;
            }
        }

        public void Run()
        {
            Console.WriteLine("Finding sideboard: with initial deck\n");
            PrintDeck();
            Console.WriteLine("");

            // First, save the standard output.
            TextWriter standardOutput = Console.Out;

            Console.SetOut(StreamWriter.Null);

            foreach (var (Removed, Added, Turn) in AllPossibilities())
            {
                Console.SetOut(standardOutput);
                Console.WriteLine($" - Removed: {Removed().CardName}, Added:{Added().CardName} (AWT: {Turn})");
                Console.SetOut(StreamWriter.Null);
            }

            Console.SetOut(standardOutput);
            Console.WriteLine("");
            Console.WriteLine("Found sideboard: with final deck\n");
            PrintDeck();
            Console.WriteLine("");
        }
    }
}
