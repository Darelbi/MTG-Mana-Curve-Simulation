using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class AffinityTests
    {
        [TestMethod]
        public void TestAffinityWithCost0GetsPlayed()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new SojournersCompanion(),
                                    new SeatOfTheSynod(),
                                    new Frogmite(),
                                    new MistvaultBridge(),
                                    new VaultOfWhispers(),
                                    new Ornithopter(),

                                    new Ornithopter(),
                                    new Glimmervoid());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1, should play Ornitopher and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Ornithopter(), new MistvaultBridge()));

            // Turn 2, should play Ornitopther, Seat Of the Synod AND Frogmite
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new Ornithopter(), 
                                            new SeatOfTheSynod(),
                                            new Frogmite(),
                                            new SojournersCompanion()
                                            ));

            // Turn 3, should play
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new VaultOfWhispers()));
        }

        [TestMethod]
        public void ToughtMonitorDrawCards()
        {
            var grimoire =
            TestUtils.SetupDeck(    new DarksteelCitadel(),
                                    new SeatOfTheSynod(),
                                    new ThoughtMonitor(),
                                    new Ornithopter(),

                                    new DarksteelCitadel(),
                                    new Ornithopter(),
                                    new Ornithopter(),
                                    new Glimmervoid());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(4);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Ornithopter(), new SeatOfTheSynod()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new DarksteelCitadel()));

            // Turn 3  // Found bug, lands are not played in this phase, but possibly should be played
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Ornithopter(), new DarksteelCitadel(), new ThoughtMonitor()
                                        , new Ornithopter()));
        }


        [TestMethod]
        public void ToughtcastDrawCards()
        {
            var grimoire =
            TestUtils.SetupDeck(    new Thoughtcast(),
                                    new SolRing(),
                                    new Ornithopter(),
                                    new SpringleafDrum(),
                                    new SeatOfTheSynod(),

                                    new Ornithopter(),
                                    new Ornithopter(),
                                    new Ornithopter());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(5);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Thoughtcast(), new SolRing(),
                new Ornithopter(), new SpringleafDrum(), new SeatOfTheSynod(),
                new Ornithopter(), new Ornithopter()));
        }

        [TestMethod]
        public void PlayFrogmitWithLotus()
        {
            var grimoire =
            TestUtils.SetupDeck(new LotusPetal(),
                                    new SeatOfTheSynod(),
                                    new Frogmite());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(3);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Frogmite(), new SeatOfTheSynod(),new LotusPetal()));
        }
    }
}