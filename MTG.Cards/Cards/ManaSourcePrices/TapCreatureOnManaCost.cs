namespace MTG.Cards.Cards.ManaSourcePrices
{
    public class TapCreatureOnManaCost : IManaSourcePrices
    {
        public TapCreatureOnManaCost(Card owner)
        {
            Owner = owner;
        }

        public Card Owner { get; set; }

        public int HowManyCanPayPrice(Card source, IGameInteraction gameInteraction)
        {
            return gameInteraction.HowManyCreaturesICanTapAsCost(source);
        }

        public void PayPrice(Card source, IGameInteraction gameInteraction)
        {
            gameInteraction.TapCreatureAsCost(source);
        }
    }
}
