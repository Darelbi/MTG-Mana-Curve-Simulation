namespace MTG.Cards.Cards.Creatures
{
    public class Ornithopter : Card
    {
        public Ornithopter()
        {
            CardName = "Ornithopter";
            Artifact = true;
            Creature = true;
            Power = 0;
            Toughness = 2;
            ManaCost = new Mana.Mana()
            {
                Colorless = 0
            };
            Flying = true;
        }
    }
}
