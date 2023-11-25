using MTG.Cards;

namespace MTG.Game
{
    public class Deck
    {
        Dictionary<Type, Func<Card>> FactoriesDic { get; set; }

        Dictionary<Type, int> NumbersDic { get; set; }

        public IEnumerable<Type> Cards()
        {
            foreach (var type in NumbersDic.Keys)
                yield return type;
        }

        public int GetCardCount(Type type)
        {
            if(!NumbersDic.ContainsKey(type))
                return 0;

            return NumbersDic[type];
        }

        public Func<Card> GetFactory(Type type)
        {
            return FactoriesDic[type];
        }

        public Deck()
        {
            FactoriesDic = new Dictionary<Type, Func<Card>>();
            NumbersDic = new Dictionary<Type, int>();
        }

        public int Count()
        {
            return NumbersDic.Select(x => x.Value).Sum();
        }

        public void SetCards(int num, Type type, Func<Card> cardFactory)
        {
            if (cardFactory().IsCard == false)
                throw new ArgumentException($"Not a card! {type.Name}");

            if (num < 0)
                throw new ArgumentException("Cannot have a negative number of cards");

            if (num == 0)
            {
                if (FactoriesDic.ContainsKey(type))
                {
                    FactoriesDic.Remove(type);
                    NumbersDic.Remove(type);
                }
            }
            else
            {

                FactoriesDic[type] = cardFactory;
                NumbersDic[type] = num;
            }
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