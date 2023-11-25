using MTG.Cards.Cards.Creatures.Feats;
using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Creatures
{
    public class ThoughtMonitor : Card
    {
        public ThoughtMonitor()
        {
            CardName = "Thought Monitor";
            Artifact = true;
            Creature = true;
            Power = 2;
            Toughness = 2;
            Affinity = true;
            ManaCost = new Mana.Mana()
            {
                Colorless = 6,
                Blue = 1
            };

            Flying = true;

            Features[typeof(IOnEnterGameFeat)] = new DrawCardsOnEnterGame(2);
        }
    }
}
