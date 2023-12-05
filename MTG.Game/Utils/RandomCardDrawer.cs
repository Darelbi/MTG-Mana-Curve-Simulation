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

        public static int RandomInterval(int minInclusive, int maxExclusive )
        {
            return (int)Random.Shared.NextInt64(minInclusive, maxExclusive);

            //return (int) XORShift() % (maxExclusive - minInclusive) + minInclusive;
        }

        private static uint x = 548787455, y = 842502087, z = 3579807591, w = 273326509;

        public static uint XORShift()
        {
            uint t = x ^ (x << 11);
            x = y; y = z; z = w;
            return w = w ^ (w >> 19) ^ t ^ (t >> 8);
        }
    }
}
