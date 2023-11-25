using MTG.Cards.Cards.Artifacts.Abilities;
using MTG.Cards.Cards.Artifacts.Feats;
using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Artifacts
{
    public class CranialPlating : Card
    {
        public CranialPlating(): base()
        {
            CardName = "Cranial Plating";
            ManaCost = new Mana.Mana()
            {
                Colorless = 2
            };

            Artifact = true;

            Abilities.Add( new CranialPlatingAbility(this));

            Features.Add(typeof(IEquippedPowerFeat), new ArtifactAddedPower());
        }
    }
}
