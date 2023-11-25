namespace MTG.Game.Strategies
{
    public static class StrategyVariables
    {
        // If true, assumes the next drawed card is a land when deciding if there is enough mana to support Saga
        public static bool RiskOnSaga = false;

        // Springleaf drum tap creatures only with power equal or less to this variable
        public static int DrumMaxPowerSelection = 1;

        // in which order playing cards that were skipped by the strategy selection (if any)
        public static bool PlayLeftOverCardsFromCheaper = false;
    }
}
