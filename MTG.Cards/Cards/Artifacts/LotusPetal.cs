using MTG.Cards.Cards.ManaSourcePrices;

namespace MTG.Cards.Cards.Artifacts
{
    public class LotusPetal:Card
    {
        public LotusPetal()
        {
            CardName = "Lotus Petal";
            Artifact = true;
            ManaCost = new Mana.Mana
            {
                Colorless = 0
            };
            ManaSource = new Mana.Mana()
            {
                Or = new List<Mana.Mana>
                {
                    new Mana.Mana { Red = 1 },
                    new Mana.Mana { Black = 1},
                    new Mana.Mana { Blue = 1},
                }
            };

            ManaSourcePrice = new SacrificeOnManaCost(this) { Owner = this };
        }
    }
}
