namespace MTG.Cards.Cards.Lands
{
    public class DarksteelCitadel : Card
    {
        public DarksteelCitadel()
        {
            CardName = "Darksteel Citadel";
            ManaSource = new Mana.Mana()
            {
                Colorless = 1
            };

            Land = true;
            Artifact = true;
            Indestructible = true;
        }
    }
}
