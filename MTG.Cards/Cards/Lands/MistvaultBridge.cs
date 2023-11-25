namespace MTG.Cards.Cards.Lands
{
    public class MistvaultBridge : Card
    {
        public MistvaultBridge()
        {
            CardName = "Mistvault Bridge";
            ManaSource = new Mana.Mana()
            {
                Or = new List<Mana.Mana>
                {
                    new Mana.Mana { Blue = 1 },
                    new Mana.Mana { Black = 1}
                }
            };

            Land = true;
            Artifact = true;
            EntersGameTapped = true;
        }
    }
}
