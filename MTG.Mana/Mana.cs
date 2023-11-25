namespace MTG.Mana
{
    // missing green and white since I do not use them, also missing special costs like X or XX or "pay 2 life"
    public class Mana
    {
        public int? Colorless { get; set; }
        public int? Red { get; set; }
        public int? Blue { get; set; }
        public int? Black { get; set; }
        public List<Mana> And { get; set; } = new List<Mana>();
        public List<Mana> Or { get; set; } = new List<Mana>();
    }
}