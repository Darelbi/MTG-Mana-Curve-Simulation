using MTG.Cards;

namespace MTG.Game
{
    public interface ICardDrawer
    {
        public Card Draw(List<Card> cards);
    }
}
