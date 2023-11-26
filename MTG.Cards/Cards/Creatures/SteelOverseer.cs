using MTG.Cards.Cards.Creatures.Abilities;

namespace MTG.Cards.Cards.Creatures
{
    public class SteelOverseer : Card
    {
        public SteelOverseer()
        {
            CardName = "Steel Overseer";
            Artifact = true;
            Creature = true;
            Power = 1;
            Toughness = 1;
            ManaCost = new Mana.Mana()
            {
                Colorless = 2
            };

            Abilities.Add(new SteelOverseerAbility(this));
        }
    }
}
