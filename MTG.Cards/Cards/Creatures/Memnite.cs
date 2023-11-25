namespace MTG.Cards.Cards.Creatures
{
    public class Memnite : Card
    {
        public Memnite()
        {
            CardName = "Memnite";
            Artifact = true;
            Creature = true;
            Power = 1;
            Toughness = 1;
            ManaCost = new Mana.Mana()
            {
                Colorless = 0
            };
        }
    }
}
