using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class CombatTests
    {
        [TestMethod]
        public void TestZeroCreaturesDamage()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new Ornithopter(),
                                    new Memnite(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(2);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Ornithopter(), new Memnite()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new SeatOfTheSynod()));
            Assert.AreEqual(1, game.TotalDamageDealt());
        }

        [TestMethod]
        public void TestAffinityDamage()
        {
            var grimoire =
            TestUtils.SetupDeck(    new SeatOfTheSynod(),
                                    new SolRing(),
                                    new SolRing(),
                                    new Ornithopter(),
                                    new Frogmite(),
                                    new Frogmite(),
                                    new MyrEnforcer(),
                                    new SeatOfTheSynod()
                                    ) ;

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.GetPlayCards().Count() == 7);

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));
            Assert.AreEqual(8, game.TotalDamageDealt());
        }

        [TestMethod]
        public void TestMasterOfEtheriumDamage()
        {
            var grimoire =
            TestUtils.SetupDeck(    new Ornithopter(),
                                    new Ornithopter(),
                                    new MasterOfEtherium(),
                                    new DarksteelCitadel(),

                                    new DarksteelCitadel(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(), // turn 4.
                                    new SeatOfTheSynod());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(4);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.GetPlayCards().Count == 3);

            // Turn 2
            game.Turn();
            Assert.AreEqual(0, game.TotalDamageDealt());

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new MasterOfEtherium()));
            Assert.AreEqual(2, game.TotalDamageDealt());

            // Turn 4
            game.Turn();
            Assert.AreEqual(12, game.TotalDamageDealt());
        }
    }
}