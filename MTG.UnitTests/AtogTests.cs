using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class AtogTests

    {
        [TestMethod]
        public void CheckAtogActivation_NoPowerHouses()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),
                                    new GreatFurnace(),
                                    new DarksteelCitadel(),
                                    new Atog(),

                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1, 
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new Memnite(),
                                            new Memnite(),
                                            new Memnite(),
                                            new Memnite(),
                                            new GreatFurnace()));

            // Turn 2,
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new DarksteelCitadel(), new Atog()));
            Assert.AreEqual(4, game.TotalDamageDealt());

            // Turn 3
            game.Turn();
            Assert.AreEqual(9, game.TotalDamageDealt());

            // Turn 4
            game.Turn();
            Assert.AreEqual(14, game.TotalDamageDealt());

            // Turn 5s
            game.Turn();
            Assert.AreEqual(19, game.TotalDamageDealt());

            // Turn 6s
            game.Turn();
            Assert.AreEqual(32, game.TotalDamageDealt()); //atog powered
        }

        [TestMethod]
        public void CheckAtogActivation_1PowerHouses()
        {
            var grimoire =
            TestUtils.SetupDeck(    new Ornithopter(),
                                    new Ornithopter(),
                                    new Ornithopter(),
                                    new GreatFurnace(),
                                    new Atog(),
                                    new CranialPlating(),

                                    new DarksteelCitadel(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island(),
                                    new Island());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1, 
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new Ornithopter(),
                                            new Ornithopter(),
                                            new Ornithopter(),
                                            new GreatFurnace()));

            // Turn 2,
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new CranialPlating(), new DarksteelCitadel())); //TODO: LEFTOVERMANA
            Assert.AreEqual(0, game.TotalDamageDealt());

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Atog())); //equipped cranial plating
            Assert.AreEqual(6, game.TotalDamageDealt());

            // Turn 4
            game.Turn();
            Assert.AreEqual(6+6+1, game.TotalDamageDealt());

            // Turn 5s
            game.Turn();
            Assert.AreEqual(6+6+6+1+1, game.TotalDamageDealt());

            // Turn 6s
            game.Turn();
            Assert.AreEqual(20+ 8+1+2, game.TotalDamageDealt()); //atog powered
        }

    }
}