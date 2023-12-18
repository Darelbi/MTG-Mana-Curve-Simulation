namespace MTG.Cards.Cards.Upkeeps
{
    public interface IUpkeepPrice
    {
        public void PayUpkeep(Card source, IGameInteraction gameInteraction);
    }
}
