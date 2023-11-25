using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class SpringleafDrumTests
    {
        [TestMethod]
        public void TestDrumsInARow()
        {
            var grimoire = 
            TestUtils.SetupDeck(    new SpringleafDrum(),
                                    new SpringleafDrum(),
                                    new SpringleafDrum(),
                                    new Memnite(),
                                    new Ornithopter(),
                                    new Frogmite(),
                                    new Glimmervoid(),

                                    new Ornithopter(),
                                    new Glimmervoid());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

        // Turn 1, 
        // regression: After strategy changes the frogmite is played before the springleaf drum
        // is that ok or should I tailor the strategy?
        // I play: Glimmervoid, Memnite, Ornithopter, I can tap 2 creatures
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Glimmervoid(), new Frogmite(),
                new Ornithopter(), new Memnite(), new SpringleafDrum(), new SpringleafDrum(), new SpringleafDrum()));
        }

        [TestMethod]
        public void TestDrumsInARow2OutOf3()
        {
            var grimoire =
            TestUtils.SetupDeck(    new SpringleafDrum(),
                                    new SpringleafDrum(), // one is played because the game just find free mana
                                    new SpringleafDrum(),
                                    new Ornithopter(),
                                    new Frogmite(),
                                    new Glimmervoid());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1, should play Ornitopher and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Glimmervoid(), 
                new Ornithopter(), new SpringleafDrum(), new SpringleafDrum()));
        }

    }
}