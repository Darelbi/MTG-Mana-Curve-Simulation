using MTG.Cards.Cards.ManaSourcePrices;

namespace MTG.Cards.Cards.Artifacts
{
    public class Placeholder1 : Card
    {
        public Placeholder1()
        {
            CardName = "Placeholder with cost 1";
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };
        }
    }
}
