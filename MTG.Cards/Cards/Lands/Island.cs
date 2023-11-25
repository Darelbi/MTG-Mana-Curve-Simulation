namespace MTG.Cards.Cards.Lands
{
    public class Island : Card
    {
        public Island()
        {
            CardName = "Island";
            ManaSource = new Mana.Mana()
            {
                Blue = 1
            };

            Land = true;
        }
    }
}
