namespace MTG.Cards.Cards.Effects
{
    public class PlainPowerIncreaseEffect : IPowerIncreaseEffect
    {
        private readonly int amount;

        public Card Owner { get; set; }

        public Func<IGameInteraction, Card, bool> Benefits { get; set; }

        public PlainPowerIncreaseEffect(int amount)
        {
            this.amount = amount;
        }

        public bool BenefitsOfPower(Card source, IGameInteraction interaction, Card target)
        {
            return Benefits.Invoke(interaction, target);
        }

        public int GetPower(Card source, IGameInteraction interaction, Card target)
        {
            return amount;
        }
    }
}
