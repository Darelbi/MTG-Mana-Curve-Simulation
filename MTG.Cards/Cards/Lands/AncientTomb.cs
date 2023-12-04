using MTG.Cards.Cards.ManaSourcePrices;

namespace MTG.Cards.Cards.Lands
{
    public class AncientTomb : Card
    {
        public AncientTomb()
        {
            CardName = "Ancient Tomb";
            ManaSource = new Mana.Mana()
            {
                Colorless = 2
            };

            Land = true;

            ManaSourcePrice = new DamageOnManaCost(this);
        }
    }
}
