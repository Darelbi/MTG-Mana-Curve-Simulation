using MTG.Cards;

namespace MTG.Game
{
    public class Grimoire
    {
        private List<Card> cards;
        private readonly ICardDrawer drawer;

        public Grimoire(List<Card> cards, ICardDrawer drawer)
        {
            this.cards = cards;
            this.drawer = drawer;
        }

        public bool HasCards() { return cards.Count > 0; }

        public int Count()
        {
            return cards.Count;
        }

        public int Count(Func<Card,bool> selection)
        {
            return cards.Count(selection);
        }

        public Card DrawCard()
        {
            return drawer.Draw(cards);
            
        }

        public void PutCardBack(Card card)
        {
            cards.Add(card);
        }
    }
}
