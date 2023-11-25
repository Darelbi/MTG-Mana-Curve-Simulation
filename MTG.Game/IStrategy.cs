using MTG.Cards;
using MTG.Cards.Cards.ActivatedAbilities;

namespace MTG.Game
{
    public interface IStrategy
    {
        void ActivateAbilities(List<IActivatedAbility> abilities, IGameInteraction gameInteraction, bool allowedSorceries);
        Card SelectCreatureToEquip(Card equipment, IGameInteraction gameInteraction);
        int HowManyCreaturesICanTapAsCost(Card source, IGameInteraction gameInteraction);
        List<Card> SelectCardsToPlay(IGameInteraction gameInteraction);
        List<Card> SelectManaAndFreeCardsToPlay(IGameInteraction gameInteraction);

        bool ShouldMulligan(List<Card> hand);
        void TapCreatureAsCost(Card source, IGameInteraction gameInteraction);
        Card FindCardInDeck(Card source, Func<Card, bool> filter, IGameInteraction gameInteraction);
    }
}
