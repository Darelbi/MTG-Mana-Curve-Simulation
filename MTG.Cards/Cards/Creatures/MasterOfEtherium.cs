using MTG.Cards.Cards.Creatures.Feats;
using MTG.Cards.Cards.Effects;
using MTG.Cards.Cards.Feats;

namespace MTG.Cards.Cards.Creatures
{
    public class MasterOfEtherium : Card
    {
        public MasterOfEtherium()
        {
            CardName = "Master of Etherium";
            Artifact = true;
            Creature = true;
            Power = 0;
            Toughness = 0;
            ManaCost = new Mana.Mana()
            {
                Colorless = 2,
                Blue = 1
            };

            Features[typeof(IPowerFeat)] = new ArtifactPower();
            Features[typeof(IToughnessFeat)] = new ArtifactToughness();

            Effects.Add(new PlainPowerIncreaseEffect(1) { Owner = this, 
                Benefits = (_,target) => target.Artifact && target.Creature // TODO: creature is redundant, but better be explicit
            });
            Effects.Add(new PlainToughnessIncreaseEffect(1) { Owner = this,
                Benefits = (_, target) => target.Artifact && target.Creature 
            });
        }
    }
}
