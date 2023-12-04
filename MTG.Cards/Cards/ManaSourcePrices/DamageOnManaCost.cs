namespace MTG.Cards.Cards.ManaSourcePrices
{
    public class DamageOnManaCost : IManaSourcePrices
    {
        public Card Owner { get; set; }

        public DamageOnManaCost(Card owner) { Owner = owner; }

        public int HowManyCanPayPrice(Card source, IGameInteraction gameInteraction)
        {
            return (14-gameInteraction.GetDamageReceived())/2;
        }

        public void PayPrice(Card source, IGameInteraction gameInteraction)
        {
            gameInteraction.ReceiveDamage(2);
        }
    }
}
