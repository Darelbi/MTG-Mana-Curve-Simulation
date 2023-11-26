using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Effects;
using System.Collections.ObjectModel;

namespace MTG.Cards
{
    public interface IGameInteraction
    {

        List<Card> GetPlayCards();

        List<Card> GetHandCards();

        ReadOnlyCollection<Card> GetGrimoireCards();

        void DrawFromGame(Card soruce, int howmanycards);

        bool CanPlayLand();

        void Sacrifice(Card card);

        void PutCardInPlay(Card card);

        void PutCardInPlayFromHand(Card card);
        void ActivateAbility(IActivatedAbility ability);
        void TapCreatureAsCost(Card source);
        int HowManyCreaturesICanTapAsCost(Card source);
        int GetCardPower(Card x);

        int FoeLifeLeft();
        void Equip(Card equipment);
        void FindCardToPlayFromDeck(Card source, Func<Card, bool> filter);
        List<Card> GetArtifactsICanSacrifice();
        void AddUntilEndOfTurnEffects(IEffect effect);
    }
}
