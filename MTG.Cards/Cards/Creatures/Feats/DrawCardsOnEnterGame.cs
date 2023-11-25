using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Creatures.Feats
{
    public class DrawCardsOnEnterGame : IOnEnterGameFeat
    {
        private readonly int howmanyCards;

        public DrawCardsOnEnterGame(int howmanyCards)
        {
            this.howmanyCards = howmanyCards;
        }
        public void OnEnterGame(Card source, IGameInteraction interaction)
        {
            interaction.DrawFromGame(source, howmanyCards);
        }
    }
}
