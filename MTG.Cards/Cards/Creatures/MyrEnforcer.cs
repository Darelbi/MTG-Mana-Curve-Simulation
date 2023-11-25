namespace MTG.Cards.Cards.Creatures
{
    public class MyrEnforcer : Card
    {
        public MyrEnforcer()
        {
            CardName = "Myr Enforcer";
            Artifact = true;
            Creature = true;
            Power = 4;
            Toughness = 4;
            Affinity = true;
            ManaCost = new Mana.Mana()
            {
                Colorless = 7
            };
        }
    }
}
