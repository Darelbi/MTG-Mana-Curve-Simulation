namespace MTG.Cards.Cards.Artifacts
{
    public class SolRing : Card
    {
        public SolRing()
        {
            CardName = "Sol Ring";
            Artifact = true;
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };

            ManaSource = new Mana.Mana
            {
                Colorless = 2
            };
        }
    }
}
