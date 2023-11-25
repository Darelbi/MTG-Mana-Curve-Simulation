using MTG.Cards.Cards.ActivatedAbilities;

namespace MTG.Cards.Cards.Artifacts.Abilities
{
    public class CranialPlatingAbility:IActivatedAbility
    {
        public Mana.Mana ManaCost { get; set; }
        public bool TapOnActivate { get; set; }
        public Card Owner { get; set; }

        public CranialPlatingAbility(Card owner)
        {
            ManaCost = new Mana.Mana
            {
                Colorless = 1
            };

            TapOnActivate = false;
            Owner = owner;
        }

        public void Outcome(IGameInteraction interaction)
        {
            interaction.Equip(Owner);
        }
    }
}
