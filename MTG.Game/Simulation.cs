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

        public static List<int> Run(int games, Deck startingDeck)
        {
            var list = new List<int>();
            var drawer = new RandomCardDrawer();
            var strategy = new DefaultStrategy();

            for (int i = 0; i < games; i++)
            {
                var grimoire = startingDeck.GetGrimoire(drawer);
                int turns = 0;
                var game = new Game(grimoire, strategy, i % 2 == 0);
                game.BeginGame();

                while (!game.HasWon())
                {
                    turns++;
                    game.Turn();
                }

                list.Add(turns);
            }

            return list;
        }

        public async Task Run()
        {
            await RunInternal();
        }

        public async Task RunInternal()
        {
            int cpus = Environment.ProcessorCount * 2;
            List<Task<List<int>>> tasks= new(cpus);

            for(int i=0; i<cpus; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => Run(games / cpus, startingDeck)));
            }

            var results = await Task.WhenAll(tasks);

            foreach(var result in results)
            {
                winningTurn.AddRange(result);
            }
        }
    }
}