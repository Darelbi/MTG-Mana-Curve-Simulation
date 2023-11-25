using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Creatures.Feats
{
    public class ArtifactPower : IPowerFeat
    {
        public int GetPower(Card source, IGameInteraction interaction)
        {
            return interaction.GetPlayCards().Count(x => x.Artifact);
        }
    }
}
