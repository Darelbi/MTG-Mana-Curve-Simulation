using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Tokens;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class LandTests
    {
        [TestMethod]
        public void MistVaultPlayedFirstAtFirstTurn()
        {
            var grimoire =
            TestUtils.SetupDeck(new MistvaultBridge(),
                                new SeatOfTheSynod(),

                                new Memnite()
                                );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(2);

            // Turn 1, should play Ornitopher and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new MistvaultBridge()));

            // Turn 2, should play Ornitopther, Seat Of the Synod AND Frogmite
            // TODO: When implemented main phase, SojournersCompanion will be played
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod(), new Memnite()));
        }

        [TestMethod]
        public void ArtifactLandsPlayedFirst()
        {
            var grimoire =
            TestUtils.SetupDeck(new Glimmervoid(),
                                new SeatOfTheSynod(),

                                new Memnite()
                                );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(2);

            // Turn 1, should play Ornitopher and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 2, should play Ornitopther, Seat Of the Synod AND Frogmite
            // TODO: When implemented main phase, SojournersCompanion will be played
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Glimmervoid(), new Memnite()));
        }

        [TestMethod]
        public void PlayGlimmerBeforeVaultOfWhispers()
        {
            var grimoire =
            TestUtils.SetupDeck(new VaultOfWhispers(),
                                new Frogmite(),
                                new Glimmervoid(),
                                new Ornithopter()
                                );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(4);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Ornithopter(), new Glimmervoid()));
        }

        [TestMethod]
        public void AncientTombEnablesUrzasSaga()
        {
            var grimoire =
            TestUtils.SetupDeck(new AncientTomb(),
                                new UrzasSaga(),

                                new Ornithopter()
                                );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(2);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new AncientTomb(), new Ornithopter()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct()));
        }

    }
}