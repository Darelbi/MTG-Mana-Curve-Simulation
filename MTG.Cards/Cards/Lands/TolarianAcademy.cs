using MTG.Cards.Cards.Feats;
using MTG.Cards.Cards.Lands.Feats;

namespace MTG.Cards.Cards.Lands
{
    public class TolarianAcademy : Card
    {
        public TolarianAcademy()
        {
            CardName = "Tolarian Academy";
            ManaSource = new Mana.Mana()
            {
                Blue = 1
            };

            Land = true;

            this.Features.Add(typeof(IDynamicManaValue), new TolarianAcademyManaCounting());
        }
    }
}
