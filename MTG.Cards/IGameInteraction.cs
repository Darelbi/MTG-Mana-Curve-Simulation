using MTG.Cards.Cards.ActivatedAbilities;

namespace MTG.Cards
{
    public interface IGameInteraction
    {

        List<Card> GetPlayCards();

        List<Card> GetHandCards();

        void Sacrifice(Card card);

        void PutCardInPlay(Card card);

        void PutCardInPlayFromHand(Card card);
        void ActivateAbility(IActivatedAbility ability);
        void TapCreatureAsCost(Card source);
        int HowManyCreaturesICanTapAsCost(Card source);
        int GetCardPower(Card x);
        void Equip(Card equipment);
    }
}
