namespace MTG.Cards.Cards.Lands
{
    public class SeatOfTheSynod : Card
    {
        public SeatOfTheSynod()
        {
            CardName = "Seat of the Synod";
            ManaSource = new Mana.Mana()
            {
                Blue = 1
            };

            Land = true;
            Artifact = true;
        }
    }
}
