namespace MTG.Cards.Cards.Feats
{
    public interface IPowerFeat: ICardFeat
    {
        int GetPower(Card source, IGameInteraction interaction);
    }
}
