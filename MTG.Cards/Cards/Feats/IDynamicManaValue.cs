namespace MTG.Cards.Cards.Feats
{
    public interface IDynamicManaValue: ICardFeat
    {
        void ActualManaVaule(Card source, IGameInteraction interaction);

        void PotentialManaValue(Card source, IGameInteraction interaction);
    }
}
