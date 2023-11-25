namespace MTG.Cards.Cards.Feats
{
    public interface IOnEnterGameFeat : ICardFeat
    {
        void OnEnterGame(Card source, IGameInteraction interaction);
    }
}
