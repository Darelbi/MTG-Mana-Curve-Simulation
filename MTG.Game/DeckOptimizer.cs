using MTG.Cards;

namespace MTG.Game
{
    public class DeckOptimizer
    {
        private readonly Deck startingDeck;
        private readonly Deck tryList;

        public DeckOptimizer(Deck startingDeck, Deck tryList)
        {
            this.startingDeck = startingDeck;
            this.tryList = tryList;
        }

        void PrintDeck()
        {
            foreach(var card in startingDeck.Cards())
            {
                Console.WriteLine($"{startingDeck.GetCardCount(card)}\tx {startingDeck.GetFactory(card)().CardName}");
            }
        }

        IEnumerable<(Func<Card> Removed, Func<Card> Added)> AllPossibilities()
        {
            for(int i=0; i<10; i++)
            {
                // first simulate the deck to have a compare point 
                var simulation = new Simulation(startingDeck, 10000);
                simulation.Run();
                double betterTurn = simulation.GetAverageWinningTurn();

                var typesToTryRemove = startingDeck.Cards().ToList();
                Type betterRemoved = null;
                Type betterAdded = null;

                foreach(var types in typesToTryRemove )
                {
                    int previousNumber = startingDeck.GetCardCount(types);
                    var previousFactory = startingDeck.GetFactory(types);

                    var attemps = tryList.Cards().ToList();

                    foreach(var attemp in attemps)
                    {
                        startingDeck.SetCards(previousNumber, types, previousFactory);

                        if (attemp!= types) // save identical simulations
                        {
                            startingDeck.SetCards(previousNumber - 1, types, previousFactory);
                            startingDeck.SetCards(
                                startingDeck.GetCardCount(attemp)+1
                                , attemp, tryList.GetFactory(attemp));

                            simulation = new Simulation(startingDeck, 10000);
                            simulation.Run();

                            var winningTurn = simulation.GetAverageWinningTurn();

                            if(winningTurn < betterTurn)
                            {
                                betterTurn = winningTurn;
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
                    yield return (factoryRemoved, factoryAdded);
                }
            }
        }

        public void Run()
        {
            Console.WriteLine("Starting Optimization: with initial deck\n");
            PrintDeck();
            Console.WriteLine("");

            // First, save the standard output.
            TextWriter standardOutput = Console.Out;

            Console.SetOut(StreamWriter.Null);

            foreach(var (Removed, Added) in AllPossibilities())
            {
                Console.SetOut(standardOutput);
                Console.WriteLine($"Removed: {Removed().CardName}, Added:{Added().CardName}");
                Console.SetOut(StreamWriter.Null);
            }

            Console.SetOut(standardOutput);
            Console.WriteLine("");
            Console.WriteLine("Ended Optimization: with final deck\n");
            PrintDeck();
            Console.WriteLine("");
        }
    }
}
