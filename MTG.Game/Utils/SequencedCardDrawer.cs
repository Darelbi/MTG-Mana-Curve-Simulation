using MTG.Cards;

namespace MTG.Game.Utils
{
    public class SequencedCardDrawer : ICardDrawer
    {
        public Card Draw(List<Card> cards)
        {
            var card = cards.ElementAt(0);
            cards.RemoveAt(0);
            return card;
        }
    }
}
