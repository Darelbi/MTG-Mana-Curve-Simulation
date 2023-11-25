namespace MTG.Cards.Cards.ManaSourcePrices
{
    public interface IManaSourcePrices
    {
        public int HowManyCanPayPrice(Card source, IGameInteraction gameInteraction);

        public void PayPrice(Card source, IGameInteraction gameInteraction);

        public Card Owner { get; set; }
    }
}
