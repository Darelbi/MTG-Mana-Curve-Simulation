using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class CranialPlatingTests
    {
        [TestMethod]
        public void SimplePlatingTest()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new SolRing(),
                                    new SeatOfTheSynod(),
                                    new Memnite(),
                                    new Ornithopter(),
                                    new CranialPlating(),
                                    new MyrEnforcer(),

                                    new Ornithopter(),
                                    new Glimmervoid());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1, 
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new SolRing(),
                                            new SeatOfTheSynod(),
                                            new Memnite(),
                                            new Ornithopter(),
                                            new CranialPlating()));

            // Turn 2,
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new MyrEnforcer(), new Ornithopter()));
            Assert.AreEqual(8, game.TotalDamageDealt());

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new Glimmervoid()));
        }

    }
}