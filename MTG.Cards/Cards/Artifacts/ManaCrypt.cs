using MTG.Cards.Cards.Upkeeps;

namespace MTG.Cards.Cards.Artifacts
{
    public class ManaCrypt : Card
    {
        public ManaCrypt()
        {
            CardName = "Mana Crypt";
            Artifact = true;
            ManaCost = new Mana.Mana
            {
                Colorless = 0
            };

            ManaSource = new Mana.Mana
            {
                Colorless = 2
            };

            Upkeep = new ManaCryptUpkeep();
        }
    }
}
