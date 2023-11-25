using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Artifacts.Feats
{
    public class ArtifactAddedPower : IEquippedPowerFeat
    {
        public int GetEquippedPower(Card source, IGameInteraction interaction)
        {
            return interaction.GetPlayCards().Count(x => x.Artifact);
        }
    } 
}
