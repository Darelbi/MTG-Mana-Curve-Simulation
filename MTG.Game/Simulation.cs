using MTG.Game.Strategies;
using MTG.Game.Utils;

namespace MTG.Game
{
    internal class Simulation
    {
        private Deck startingDeck;
        private readonly int games;
        private List<int> winningTurn;

        public double GetAverageWinningTurn()
        {
            return winningTurn.OrderBy(x => x).Average();
        }

        public Simulation(Deck startingDeck, int games)
        {
            this.startingDeck = startingDeck;
            this.games = games;
            this.winningTurn = new List<int>();
        }

        public void Run()
        {
            int turns = 0;
            var drawer = new RandomCardDrawer();
            var strategy = new DefaultStrategy();
            for (int i = 0; i < games; i++)
            {
                var grimoire = startingDeck.GetGrimoire(drawer);
                turns = 0;
                var game = new Game(grimoire,strategy, i % 2 == 0);
                game.BeginGame();

                while (!game.HasWon())
                {
                    turns++;
                    game.Turn();
                }

                winningTurn.Add(turns);
            }
        }
    }
}