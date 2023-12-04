namespace MTG.Cards.Cards.Artifacts
{
    public class ArtifactPlaceholder1 : Card
    {
        public ArtifactPlaceholder1()
        {
            CardName = "Artifact placeholder with cost 1";
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };
            Artifact = true;
        }
    }
}
