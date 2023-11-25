using MTG.Cards;

namespace MTG.Game.Utils
{
    public class RandomCardDrawer : ICardDrawer
    {
        public Card Draw(List<Card> cards)
        {
            int randomIndex = (int)Random.Shared.NextInt64(0, cards.Count);
            var card = cards.ElementAt(randomIndex);
            cards.RemoveAt(randomIndex);
            return card;
        }
    }
}
