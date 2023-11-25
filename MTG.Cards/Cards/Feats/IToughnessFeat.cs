namespace MTG.Cards.Cards.Feats
{
    public interface IToughnessFeat: ICardFeat
    {
        int GetToughness(Card source, IGameInteraction interaction);
    }
}
