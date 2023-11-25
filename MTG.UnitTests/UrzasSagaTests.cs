using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Tokens;
using MTG.Game.Strategies;

namespace MTG.UnitTests
{
    [TestClass]
    public class UrzasSagaTests
    {
        [TestMethod]
        public void UrzaSagaGetPlayedAt2ndTurnAndAbilityActivated()
        {
            StrategyVariables.RiskOnSaga = false;

            var grimoire =
            TestUtils.SetupDeck(    new UrzasSaga(),
                                    new Memnite(),
                                    new VaultOfWhispers(),
                                    new SeatOfTheSynod(),
                                    new UrzasSaga(),
                                    new MistvaultBridge(),

                                    new Glimmervoid(),
                                    new Glimmervoid(),
                                    new SojournersCompanion(),
                                    new SeatOfTheSynod());

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1, should play Memnite and MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Memnite(), new MistvaultBridge()));

            // 2 turn should play urzas saga
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // 3 turn should not play urza saga (cannot support 2 at once), should play a land AND
            // check a construct is created
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct()));

            // 4 turn
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct()));

            // 5 turn
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct(), new Construct()));
        }

        [TestMethod]
        public void CheckIf2UrzaSagaCanCoexistIfEnoughMana()
        {
            StrategyVariables.RiskOnSaga = false;

            var grimoire =
            TestUtils.SetupDeck(    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),

                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new UrzasSaga(),
                                    new UrzasSaga(),
                                    new Memnite(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 4
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 5
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 6
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct()));

            // Turn 7
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Memnite(), new SeatOfTheSynod()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct(), new Construct()));

            // Turn 8
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Memnite(), new SeatOfTheSynod()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct(), new Construct(), new Construct()));
        }

        [TestMethod]
        public void CheckIf2ndUrzaSagaIsNotPlayedWith3Lands()
        {
            StrategyVariables.RiskOnSaga = false;

            var grimoire =
            TestUtils.SetupDeck(    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),

                                    new Memnite(),
                                    new Memnite(),
                                    new UrzasSaga(),
                                    new UrzasSaga(),
                                    new Memnite(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 4
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 5 IS FALSE
            game.Turn();
            Assert.IsFalse(game.PlayedCards(new UrzasSaga()));
        }

        [TestMethod]
        public void CheckIf2ndUrzaSagaIsPlayedWith3Lands()
        {
            // THIS IS A BIG SEMPLIFICATION. In general the automated Game will not play a 2nd Urza saga if there are 3 lands
            // the motivation is to not waste Constructs.. BUT there are edge cases where this may be wanted. (in example
            // use urza saga 2 times in a row for mana. I.E. to play Cranial plating. However why should I reach such case?
            // If I have 3 lands in play there is high chance I already played the cranial plating.
            // The Saga should be played if I have a 4th land in hand... 
            // but when I have 1 Saga in play, 1 saga in hand and 3 lands into play.. what is the chance of drawing a land
            // because I could RISK and play the 2nd saga hoping to draw a land... but lands are < 1/3 so no gain.
            // HOWEVER TESTS May show that PERFORMANCE (to be defined what is performance) increase when RISKING
            StrategyVariables.RiskOnSaga = true;

            var grimoire =
            TestUtils.SetupDeck(new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new Memnite(),
                                    new Memnite(),
                                    new Memnite(),

                                    new Memnite(),
                                    new Memnite(),
                                    new UrzasSaga(),
                                    new UrzasSaga(),
                                    new Memnite(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 4
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 5 IS FALSE
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            StrategyVariables.RiskOnSaga = false;
        }

        [TestMethod]
        public void CheckIf2ndUrzaSagaIsPlayedWith3LandsAnd1InHand()
        {
            // THIS IS A BIG SEMPLIFICATION. In general the automated Game will not play a 2nd Urza saga if there are 3 lands
            // the motivation is to not waste Constructs.. BUT there are edge cases where this may be wanted. (in example
            // use urza saga 2 times in a row for mana. I.E. to play Cranial plating. However why should I reach such case?
            // If I have 3 lands in play there is high chance I already played the cranial plating.
            StrategyVariables.RiskOnSaga = false;

            var grimoire =
            TestUtils.SetupDeck(    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new SeatOfTheSynod(),
                                    new Memnite(),
                                    new Memnite(),

                                    new Memnite(),
                                    new Memnite(),
                                    new UrzasSaga(),
                                    new UrzasSaga(),
                                    new Memnite(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(6);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 2
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 3
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));

            // Turn 4
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 5
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 6
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new SeatOfTheSynod()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct(), new Construct()));
        }

        [TestMethod]
        public void ComplexSagaCaseWithSolRing()
        {
            var grimoire =
            TestUtils.SetupDeck(    new Memnite(),
                                    new SojournersCompanion(),
                                    new UrzasSaga(),
                                    new Glimmervoid(),
                                    new VaultOfWhispers(),
                                    new SojournersCompanion(),
                                    new Memnite(),

                                    new MistvaultBridge(),
                                    new UrzasSaga(),
                                    new SolRing(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Glimmervoid(), new Memnite(), new Memnite()));

            // Turn 2 : Drawed MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 3 : Drawed UrzasSaga
            game.Turn(); // Played Mistvault Bridge... WHAT? CANNOT PLAY TAPPED AT SECOND TURN
                         // BECAUSE NEED SUPPORT FOR SAGA
            Assert.IsTrue(game.PlayedCards(new VaultOfWhispers()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct()));

            // Turn 4 : Drawed Sol Ring
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga() , new SolRing(), new SojournersCompanion(), new SojournersCompanion()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct()));
             
            // Turn 5 : Drawed Memnit
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new MistvaultBridge(), new Memnite()));
            Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct(), new Construct()));
        }

        [TestMethod]
        public void ComplexSagaCaseWithSolRing_DontPlaySagaWithoutSol()
        {
            var grimoire =
            TestUtils.SetupDeck(    new Memnite(),
                                    new SojournersCompanion(),
                                    new UrzasSaga(),
                                    new Glimmervoid(),
                                    new VaultOfWhispers(),
                                    new SojournersCompanion(),
                                    new Memnite(),

                                    new MistvaultBridge(),
                                    new UrzasSaga(),
                                    new Memnite(),
                                    new Memnite()
                                    );

            var game = new Game.Game(grimoire, new DefaultStrategy(), true);
            game.BeginTestGame(7);

            // Turn 1
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new Glimmervoid(), new Memnite(), new Memnite()));

            // Turn 2 : Drawed MistvaultBridge
            game.Turn();
            Assert.IsTrue(game.PlayedCards(new UrzasSaga()));

            // Turn 3 : Drawed UrzasSaga
            game.Turn(); // Played Mistvault Bridge... WHAT? CANNOT PLAY TAPPED AT SECOND TURN
                         // BECAUSE NEED SUPPORT FOR SAGA
                         // Assert.IsTrue(game.PlayedCards(new VaultOfWhispers()));
                         //Assert.IsTrue(game.CheckCardsInPlay(new Construct()));

            // Turn 4 : Drawed Sol Ring
            game.Turn();
            //Assert.IsTrue(game.PlayedCards());
            //Assert.IsTrue(game.CheckCardsInPlay(new Construct(), new Construct()));

            // Turn 5 : Drawed Memnit
            game.Turn();
            //Assert.IsTrue(game.PlayedCards());
        }
    }
}