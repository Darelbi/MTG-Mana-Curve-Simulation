using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Effects;

namespace MTG.Cards.Cards.Creatures.Abilities
{
    public class AtogAbility : IActivatedAbility
    {
        public Mana.Mana ManaCost { get; set; }
        public bool TapOnActivate { get; set; }
        public Card Owner { get; set; }

        public AtogAbility(Card owner)
        {
            ManaCost = null;
            TapOnActivate = false;
            Owner = owner;
        }

        public void Outcome(IGameInteraction interaction)
        {
            List<Card> artifacts = interaction.GetArtifactsICanSacrifice();
            if (artifacts.Any())
            {
                //TODO: ability really activated only there.
                interaction.Sacrifice(artifacts.First());
                interaction.AddUntilEndOfTurnEffects(new PlainPowerIncreaseEffect (2)
                {
                    Benefits = (_, target) => target == Owner,
                    Owner = Owner
                });

                interaction.AddUntilEndOfTurnEffects(new PlainToughnessIncreaseEffect(2)
                {
                    Benefits = (_, target) => target == Owner,
                    Owner = Owner
                });
            }
            else
                throw new ArgumentException("Requested wrong activation of Atog Ability");
        }
    }
}
