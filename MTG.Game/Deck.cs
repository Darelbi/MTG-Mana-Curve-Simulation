using MTG.Cards;

namespace MTG.Game
{
    public class Deck
    {
        Dictionary<Type, Func<Card>> FactoriesDic { get; set; }

        Dictionary<Type, int> NumbersDic { get; set; }

        public Deck()
        {
            FactoriesDic = new Dictionary<Type, Func<Card>>();
            NumbersDic = new Dictionary<Type, int>();
        }

        public int Count()
        {
            return NumbersDic.Select(x => x.Value).Sum();
        }

        public void AddCards<T>(int num, Func<T> cardFactory) where T : Card
        {
            if (cardFactory().IsCard == false)
                throw new ArgumentException($"Not a card! {typeof(T).Name}");

            if (FactoriesDic.ContainsKey(typeof(T)))
                throw new Exception($"Card already present: {typeof(T).Name}");

            FactoriesDic[typeof(T)] = cardFactory;
            NumbersDic[typeof(T)] = num;
        }

        public Grimoire GetGrimoire(ICardDrawer cardDrawer)
        {
            List<Card> cards = new();
            foreach ((var key, var factory) in FactoriesDic.AsEnumerable())
                for (int i = 0; i < NumbersDic[key]; i++)
                    cards.Add(factory());

            return new Grimoire(cards, cardDrawer);
        }
    }
}