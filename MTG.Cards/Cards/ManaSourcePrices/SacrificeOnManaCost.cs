namespace MTG.Cards.Cards.ManaSourcePrices
{
    public class SacrificeOnManaCost : IManaSourcePrices
    {
        public SacrificeOnManaCost(Card owner)
        {
            Owner = owner;
        }

        public Card Owner { get; set; }

        public int HowManyCanPayPrice(Card source, IGameInteraction gameInteraction)
        {
            return 1;
        }

        public void PayPrice(Card source, IGameInteraction gameInteraction)
        {
            gameInteraction.Sacrifice(source);
        }
    }
}
