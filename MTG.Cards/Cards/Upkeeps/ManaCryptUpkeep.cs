namespace MTG.Cards.Cards.Upkeeps
{
    public class ManaCryptUpkeep : IUpkeepPrice
    {
        public void PayUpkeep(Card source, IGameInteraction gameInteraction)
        {
            if ((int)Random.Shared.NextInt64(0, 2)==1)
                gameInteraction.ReceiveDamage(3);
        }
    }
}
