namespace MTG.Cards.Cards.Lands
{
    public class GreatFurnace : Card
    {
        public GreatFurnace()
        {
            CardName = "Great Furnace";
            ManaSource = new Mana.Mana()
            {
                Red = 1
            };

            Land = true;
            Artifact = true;
        }
    }
}
