using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Artifacts.Feats
{
    public class ArtifactAddedToughness : IEquippedToughnessFeat
    {
        public int GetEquippedToughness(Card source, IGameInteraction interaction)
        {
            return interaction.GetPlayCards().Count(x => x.Artifact);
        }
    }
}
