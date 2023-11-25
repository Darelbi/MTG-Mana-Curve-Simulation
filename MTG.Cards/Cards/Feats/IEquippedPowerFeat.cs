namespace MTG.Cards.Cards.Feats
{
    public interface IEquippedPowerFeat: ICardFeat
    {
        int GetEquippedPower(Card source, IGameInteraction interaction);
    }
}
