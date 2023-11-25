using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Creatures.Feats
{
    public class ArtifactToughness : IToughnessFeat
    {
        public int GetToughness(Card source, IGameInteraction interaction)
        {
            return interaction.GetPlayCards().Count(x => x.Artifact);
        }
    }
}
