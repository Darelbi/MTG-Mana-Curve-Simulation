namespace MTG.Cards.Cards.Creatures
{
    public class SojournersCompanion : Card
    {
        public SojournersCompanion()
        {
            CardName = "Sojourner's Companion";
            Artifact = true;
            Creature = true;
            Power = 4;
            Toughness = 4;
            Affinity = true;
            ManaCost = new Mana.Mana()
            {
                Colorless = 7
            };
            ArtifactLandcycling = true;
            ArtifactLandcyclingCost = new Mana.Mana
            {
                Colorless = 2
            };
        }
    }
}
