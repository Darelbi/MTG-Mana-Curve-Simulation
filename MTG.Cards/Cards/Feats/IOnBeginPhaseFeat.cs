namespace MTG.Cards.Cards.Feats
{
    public interface IOnBeginPhaseFeat : ICardFeat
    {
        void OnBeginPhase(Card source, IGameInteraction interaction);
    }
}
