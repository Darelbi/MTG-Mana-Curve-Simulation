namespace MTG.Cards.Cards.Lands
{
    public class Mountain : Card
    {
        public Mountain()
        {
            CardName = "Mountain";
            ManaSource = new Mana.Mana()
            {
                Red = 1
            };

            Land = true;
        }
    }
}
