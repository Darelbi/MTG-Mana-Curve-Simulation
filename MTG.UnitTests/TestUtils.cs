using MTG.Cards;
using MTG.Game;
using MTG.Game.Utils;

namespace MTG.UnitTests
{
    public static class TestUtils
    {
        public static Grimoire SetupDeck(params Card[] cards)
        {
            return new Grimoire(cards.ToList(), new SequencedCardDrawer());
        }

        public static bool PlayedCards(this Game.Game game, params Card[] cards )
        {
            var playedCards = game.GetPlayedCardsThisTurn();

            foreach(var card in cards)
            {
                if (!playedCards.Where(x => x.GetType() == card.GetType()).Any())
                    return false;

                var match = playedCards.First(x => x.GetType() == card.GetType());
                playedCards.Remove(match);
            }

            return true;
        }

        public static bool CheckCardsInPlay(this Game.Game game, params Card[] cards)
        {
            var cardsInPlay = game.GetCardsInPlayThisTurn();

            foreach (var card in cards)
            {
                var match = cardsInPlay.First(x => x.GetType() == card.GetType());
                if (match == null)
                    return false;

                cardsInPlay.Remove(match);
            }

            return true;
        }
    }
}
