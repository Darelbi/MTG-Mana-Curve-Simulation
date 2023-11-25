using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Tokens;

namespace MTG.Cards.Cards.Lands.Abilities
{
    public class UrzasSagaAbility : IActivatedAbility
    {
        public Mana.Mana ManaCost { get; set; }
        public bool TapOnActivate { get; set; }
        public Card Owner { get; set; }

        public UrzasSagaAbility( Card owner)
        {
            ManaCost = new Mana.Mana
            {
                Colorless = 2
            };

            TapOnActivate = true;
            Owner = owner;
        }

        public void Outcome(IGameInteraction interaction)
        {
            interaction.PutCardInPlay(new Construct());
        }
    }
}
