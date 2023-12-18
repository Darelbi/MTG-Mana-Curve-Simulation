using MTG.Cards.Cards.Upkeeps;

namespace MTG.Cards.Cards.Artifacts
{
    public class ManaVault : Card
    {
        public ManaVault()
        {
            CardName = "Mana Vault";
            Artifact = true;
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };

            ManaSource = new Mana.Mana
            {
                Colorless = 3
            };

            DontUntap = true;

            Upkeep = new ManaVaultUpkeep();
        }
    }
}
