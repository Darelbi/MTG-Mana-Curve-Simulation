namespace MTG.Cards.Cards.Creatures
{
    public class Frogmite : Card
    {
        public Frogmite()
        {
            CardName = "Frogmite";
            Artifact = true;
            Creature = true;
            Power = 2;
            Toughness = 2;
            Affinity = true;
            ManaCost = new Mana.Mana()
            {
                Colorless = 4
            };
        }
    }
}
