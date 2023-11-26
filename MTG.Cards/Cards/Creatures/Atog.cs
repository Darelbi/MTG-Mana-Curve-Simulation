using MTG.Cards.Cards.Creatures.Abilities;

namespace MTG.Cards.Cards.Creatures
{
    public class Atog : Card
    {
        public Atog()
        {
            CardName = "Atog";
            Creature = true;
            Power = 1;
            Toughness = 2;
            ManaCost = new Mana.Mana()
            {
                Colorless = 1,
                Red = 1
            };

            Abilities.Add(new AtogAbility(this));
        }
    }
}
