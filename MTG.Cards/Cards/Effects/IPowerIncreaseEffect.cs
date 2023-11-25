namespace MTG.Cards.Cards.Effects
{
    public interface IPowerIncreaseEffect:IEffect
    {
        bool BenefitsOfPower(Card source, IGameInteraction interaction, Card target);
        int GetPower(Card source, IGameInteraction interaction, Card target);
    }
}
