using MTG.Cards.Cards.Creatures.Feats;
using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Tokens
{
    public class Construct : Card
    {
        public Construct()
        {
            IsCard = false;
            CardName = "Construct";
            Artifact = true;
            Creature = true;

            Features[typeof(IPowerFeat)] = new ArtifactPower();
            Features[typeof(IToughnessFeat)] = new ArtifactToughness();
        }
    }
}
