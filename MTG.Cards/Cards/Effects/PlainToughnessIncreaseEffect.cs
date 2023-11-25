namespace MTG.Cards.Cards.Effects
{
    public class PlainToughnessIncreaseEffect : IToughnessIncreaseEffect
    {
        public Card Owner { get; set; } // TODO: is redudant to have both Owner and source all other the place. decide for one and simplify the code

        private readonly int amount;

        public Func<IGameInteraction, Card, bool> Benefits { get; set; }

        public PlainToughnessIncreaseEffect( int amount)
        {
            this.amount = amount;
        }

        public bool BenefitsOfToughness(Card source, IGameInteraction interaction, Card target)
        {
            return Benefits.Invoke(interaction, target);
        }

        public int GetToughness(Card source, IGameInteraction interaction, Card target)
        {
            return amount;
        }
    }
}
