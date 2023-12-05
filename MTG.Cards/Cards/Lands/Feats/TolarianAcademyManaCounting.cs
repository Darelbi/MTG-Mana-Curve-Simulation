using MTG.Cards.Cards.Feats;
using MTG.Mana;

namespace MTG.Cards.Cards.Lands.Feats
{
    public class TolarianAcademyManaCounting : IDynamicManaValue
    {
        public void ActualManaVaule(Card source, IGameInteraction interaction)
        {
            if (source.GetType() != typeof(TolarianAcademy))
                throw new ArgumentException("expected tolarian academy in mana counting");

            var artifacts = interaction.GetPlayCards().Where(x => x.Artifact).Count();

            if (artifacts > 0)
                source.ManaSource = new Mana.Mana { Blue = artifacts };
        }

        public void PotentialManaValue(Card source, IGameInteraction interaction)
        {
            if (source.GetType() != typeof(TolarianAcademy))
                throw new ArgumentException("expected tolarian academy in mana counting");

            var artifacts = interaction.GetPlayCards().Where(x => x.Artifact).Count();
            artifacts += interaction.GetHandCards().Where(x => x.ManaCost.ConvertedManaValue() == 0).Count();

            if (artifacts > 0)
                source.ManaSource = new Mana.Mana { Blue = artifacts };
        }
    }
}
