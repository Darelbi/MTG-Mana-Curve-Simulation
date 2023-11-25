namespace MTG.Cards.Cards.Lands
{
    public class DrossforgeBridge : Card
    {
        public DrossforgeBridge()
        {
            CardName = "Drossforge Bridge";
            ManaSource = new Mana.Mana()
            {
                Or = new List<Mana.Mana>
                {
                    new Mana.Mana { Red = 1 },
                    new Mana.Mana { Black = 1}
                }
            };

            Land = true;
            Artifact = true;
            EntersGameTapped = true;
        }
    }
}
