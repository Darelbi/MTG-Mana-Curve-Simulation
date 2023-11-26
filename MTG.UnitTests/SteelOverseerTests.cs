using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class SteelOverseerTests
    {
        [TestMethod]
        public void CheckOverseerIsActivated()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),
                                    new SteelOverseer(),
                                    new SeatOfTheSynod(),
                                    new SolRing(),

                                    new Ornithopter());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1, should play Ornitopher and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards( new Memnite(), 
                                            new Memnite(),
                                            new Memnite(),
                                            new Memnite(),
                                            new SeatOfTheSynod(),
                                            new SolRing(),
                                            new SteelOverseer()));

            // Turn 2, should play Ornitopther, Seat Of the Synod AND Frogmite
            game.Turn();
            Assert.AreEqual(8,game.TotalDamageDealt());

           
        }
    }
}