namespace MTG.Cards.Cards.Feats
{
    public interface IEquippedToughnessFeat: ICardFeat
    {
        int GetEquippedToughness(Card source, IGameInteraction interaction);
    }
}
