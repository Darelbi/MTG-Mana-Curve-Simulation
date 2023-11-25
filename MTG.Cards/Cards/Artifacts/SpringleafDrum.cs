using MTG.Cards.Cards.ManaSourcePrices;

namespace MTG.Cards.Cards.Artifacts
{
    public class SpringleafDrum : Card
    {
        public SpringleafDrum()
        {
            CardName = "Springleaf Drum";
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };

            ManaSource = new Mana.Mana
            {
                Or = new List<Mana.Mana>
                {
                    new Mana.Mana { Red = 1 },
                    new Mana.Mana { Black = 1},
                    new Mana.Mana { Blue = 1},
                }
            };

            Artifact = true;
            ManaSourcePrice = new TapCreatureOnManaCost(this);
        }
    }
}
