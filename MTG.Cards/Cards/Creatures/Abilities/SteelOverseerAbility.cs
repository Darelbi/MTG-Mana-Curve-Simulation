using MTG.Cards.Cards.ActivatedAbilities;

namespace MTG.Cards.Cards.Creatures.Abilities
{
    public class SteelOverseerAbility : IActivatedAbility
    {
        public Mana.Mana ManaCost { get; set; }
        public bool TapOnActivate { get; set; }
        public Card Owner { get; set; }

        public SteelOverseerAbility(Card owner)
        {
            ManaCost = null;
            TapOnActivate = true;
            Owner = owner;
        }

        public void Outcome(IGameInteraction interaction)
        {
            var creatures = interaction.GetPlayCards().Where(x => x.Creature && x.Artifact).ToList();
            foreach(var creature in creatures)
            {
                creature.PlusOnePlusOneTokens++;
            }
        }
    }
}
