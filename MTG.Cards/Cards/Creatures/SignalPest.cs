using MTG.Cards.Cards.Effects;

namespace MTG.Cards.Cards.Creatures
{
    public class SignalPest : Card
    {
        public SignalPest()
        {
            CardName = "Signal Pest";
            Artifact = true;
            Creature = true;
            Power = 0;
            Toughness = 1;
            ManaCost = new Mana.Mana()
            {
                Colorless = 1
            };

            AttackPhaseEffects.Add(new PlainPowerIncreaseEffect(1)
            {
                Owner = this,
                Benefits = (_,target) => target != this && target.Status_Attacking
            });
        }
    }
}
