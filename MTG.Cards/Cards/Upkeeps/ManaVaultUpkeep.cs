namespace MTG.Cards.Cards.Upkeeps
{
    public class ManaVaultUpkeep : IUpkeepPrice
    {
        public void PayUpkeep(Card source, IGameInteraction gameInteraction)
        {
            if(source.Status_Tapped)
                gameInteraction.ReceiveDamage(1);
        }
    }
}
