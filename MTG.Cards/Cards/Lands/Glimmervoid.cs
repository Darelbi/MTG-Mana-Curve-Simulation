namespace MTG.Cards.Cards.Lands
{
    public class Glimmervoid : Card
    {
        public Glimmervoid()
        {
            CardName = "Glimmervoid";
            ManaSource = new Mana.Mana()
            {
                Or = new List<Mana.Mana> 
                { 
                    new Mana.Mana { Red = 1 },
                    new Mana.Mana { Black = 1},
                    new Mana.Mana { Blue = 1},
                }
            };

            Land = true;
            Artifact = false;
            SacrificeIfNoArtifacts = true;
        }
    }
}
