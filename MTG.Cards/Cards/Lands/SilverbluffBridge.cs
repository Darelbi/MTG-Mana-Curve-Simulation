namespace MTG.Cards.Cards.Lands
{
    public class SilverbluggBridge : Card
    {
        public SilverbluggBridge()
        {
            CardName = "Silverbluff Bridge";
            ManaSource = new Mana.Mana()
            {
                Or = new List<Mana.Mana>
                {
                    new Mana.Mana { Blue = 1 },
                    new Mana.Mana { Red = 1}
                }
            };

            Land = true;
            Artifact = true;
            EntersGameTapped = true;
        }
    }
}
