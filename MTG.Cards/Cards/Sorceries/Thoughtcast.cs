namespace MTG.Cards.Cards.Sorceries
{
    public class Thoughtcast : Card, ICastEffect
    {
        public Thoughtcast()
        {
            CardName = "Thoughtcast";
            Affinity = true;
            ManaCost = new Mana.Mana
            {
                Colorless = 4,
                Blue = 1
            };

            Sorcery = true;
        }

        public void CastEffect(IGameInteraction interaction)
        {
            interaction.DrawFromGame(this, 2);
        }
    }
}
