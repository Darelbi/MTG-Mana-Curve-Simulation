namespace MTG.Cards.Cards.Lands
{
    public class VaultOfWhispers : Card
    {
        public VaultOfWhispers()
        {
            CardName = "Vault of Whispers";
            ManaSource = new Mana.Mana()
            {
                Black = 1
            };

            Land = true;
            Artifact = true;
        }
    }
}
