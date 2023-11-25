using MTG.Cards;
using MTG.Cards.Cards.Feats;

namespace MTG.Game.Utils
{
    public static class FeatsExtension
    {
        public static T GetFeat<T> (this Card card) where T: class, ICardFeat
        {
            if (card.Features.ContainsKey(typeof(T)))
                return ((T)card.Features[typeof(T)]);

            return null;
        }
    }
}
