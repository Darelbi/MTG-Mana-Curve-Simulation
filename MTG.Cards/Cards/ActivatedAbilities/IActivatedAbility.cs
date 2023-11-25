namespace MTG.Cards.Cards.ActivatedAbilities
{
    public interface IActivatedAbility
    {
        public Mana.Mana ManaCost { get; set; } 

        public bool TapOnActivate { get; set; }

        public Card Owner { get; set; }

        public void Outcome(IGameInteraction interaction);
    }
}
