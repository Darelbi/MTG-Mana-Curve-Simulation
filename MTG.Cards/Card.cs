using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Effects;
using MTG.Cards.Cards.Feats;
using MTG.Cards.Cards.ManaSourcePrices;

namespace MTG.Cards
{
    public class Card
    {
        public bool IsCard { get; set; } = true; //false for tokens like the construct
        public string? CardName { get; set; }
        public Mana.Mana? ManaCost { get; set; }
        public Mana.Mana? ManaSource { get; set; }

        public IManaSourcePrices ManaSourcePrice { get; set; }
        public int Toughness { get; set; }
        public int Power { get; set; }
        public bool EntersGameTapped { get; set; } 
        public bool Land { get; set; }
        public bool Enchantment { get; set; }
        public bool Creature { get; set; }

        public bool Sorcery { get; set; }
        public bool Artifact { get; set; }
        public bool Affinity { get; set; }
        public Mana.Mana? ArtifactLandcyclingCost { get; set; }
        public bool ArtifactLandcycling { get; set; }
        public bool Flying { get; set; }
        public bool Indestructible { get; set; }
        public bool SacrificeIfNoArtifacts { get; set; }
        public int LoreCounter { get; set; }

        // status variables

        public bool Status_Tapped { get; set; }
        public bool Status_Weakness { get; set; }

        /// <summary>
        /// Features allows to do calculations that are card specific
        /// </summary>
        public Dictionary<Type, ICardFeat>? Features { get; set; } = new Dictionary<Type, ICardFeat>();

        /// <summary>
        /// List of equipped items
        /// </summary>
        public List<Card> EquippedEquipment { get; set; } = new List<Card> { };

        public Card EquippedTo { get; set; }

        /// <summary>
        /// list of abilities
        /// </summary>
        public List<IActivatedAbility> Abilities { get; set; } = new List<IActivatedAbility> ();

        /// <summary>
        /// Effects this cards keeps active while in play
        /// </summary>
        public List<IEffect> Effects { get; set; } = new List<IEffect>();

        /// <summary>
        /// How many +1/+1 tokens are on this creature
        /// </summary>
        public int PlusOnePlusOneTokens { get; set; }
        public bool Haste { get; set; }
        public bool AttackWithoutTapping { get; set; }
        public bool Instant { get; set; }
    }
}