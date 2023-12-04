using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Lands;
using MTG.Game.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.UnitTests
{
    [TestClass]
    public class SpecialManaTests
    {
        [TestMethod]
        public void CheckAncientTombDoNotExceed10Damage()
        {
            var grimoire =
            TestUtils.SetupDeck(    new AncientTomb(),
                                    new AncientTomb(),
                                    new CranialPlating(),
                                    new CranialPlating(),
                                    new CranialPlating(),
                                    new CranialPlating(),
                                    new CranialPlating(),

                                    new CranialPlating(),
                                    new CranialPlating(),
                                    new CranialPlating(),
                                    new CranialPlating()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new AncientTomb(), new CranialPlating()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new AncientTomb(), new CranialPlating(), new CranialPlating()));

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new CranialPlating(), new CranialPlating()));

            // turn 4
            game.Turn();
            Assert.IsTrue(game.CheckCardsInPlay(new CranialPlating(), new CranialPlating(),
                new CranialPlating(), new CranialPlating(), new CranialPlating(),
                new AncientTomb(), new AncientTomb()));
        }
    }
}
