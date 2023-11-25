namespace MTG.Cards.Cards.Effects
{
    public interface IToughnessIncreaseEffect:IEffect
    {
        bool BenefitsOfToughness(Card source, IGameInteraction interaction, Card target);
        int GetToughness(Card source, IGameInteraction interaction, Card target);
    }
}
