using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class SolRingTests
    {
        [TestMethod]
        public void CheckSagaIsPlayedAtFirstTurnWithSolRing()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new SolRing(),
                                    new UrzasSaga());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(2);

            // Turn 1, should play Ornitopher and MistvaultBridge // TODO: IMPLEMENT RUDIMENTARY PLAY STRATEGYs
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SolRing(), new UrzasSaga()));
        }

    }
}