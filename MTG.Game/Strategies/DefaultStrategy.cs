using MTG.Cards;
using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Artifacts.Abilities;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Creatures.Abilities;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Lands.Abilities;
using MTG.Cards.Cards.Tokens;
using MTG.Game.Utils;
using MTG.Mana;

namespace MTG.Game.Strategies
{
    //TODO: Play GlimmerVoid if there are no blue mana AND there is colored mana cost in hand... THINK BETTER ABOUT THIS
    // DO THAT ONLY FOR thoughtmonitor and thoughtcast and there are no more non-land cards to play
    // need a simpler check for that
    public class DefaultStrategy : IStrategy
    {
        public List<Card> SelectManaAndFreeCardsToPlay(IGameInteraction interaction)
        {
            // duplicate, we just do selection, it is game responsibility to actuate the selection.
            var handDuplicate = interaction.GetHandCards().ToList();

            var land = interaction.CanPlayLand() ? SelectLandToPlay(handDuplicate, interaction) : null;

            var freeCards = SelectFreeCards(handDuplicate, interaction, land);


            if (land != null)
            {
                return new List<Card> { land }.Concat(freeCards).ToList();
            }

            return freeCards; //but mulligan should prevent that.
        }

        private Card SelectLandToPlay(List<Card> handDuplicate, IGameInteraction interaction)
        {
            // select all possible lands to play
            var manaLands = handDuplicate.RemoveWithPredicate(x =>
                 x.Land && x.ManaSource != null && x.ManaSource.ConvertedManaValue() > 0);

            // eventually get urza's saga.
            var urzaSaga = handDuplicate.RemoveWithPredicate(x =>
                 x.GetType() == typeof(UrzasSaga));

            if (urzaSaga.Any() && CanPlayCardsSupportSaga(interaction))
                return urzaSaga.First();

            // glimmmervoid, requires 1 artifact into play or 1 artifact that is going to be played
            // lands not counting because we can play only 1 land per turn (some cards allows playing more lands
            // but we do not see them in affinity) TODO: allow playing 2 lands per turn
            if (interaction.GetPlayCards().Where(x => x.Artifact).Any()
                || interaction.GetHandCards().Where(x => x.Land == false && x.Artifact == true
                                                         && x.ManaCost.ConvertedManaValue() <= 1).Any())
            {

            }
            else
            {
                manaLands = manaLands.RemoveWithPredicate(x => x.GetType() != typeof(Glimmervoid)).ToList();
            }

            if (!manaLands.Any())
                return null;

            // only at first turn play first lands with entersGameTapped = true
            bool firstTurn = interaction.GetPlayCards().Count() == 0;

            // if got a sol ring, play it immediatly since it supports Saga alone
            var handCards = interaction.GetHandCards();
            if (handCards.Where(x => x.GetType() == typeof(SolRing)).Any())
                firstTurn = false;

            // the same applies if there are 2 springleaf drums and enough creatures to be tapped (not keeping into account affinity for now)
            var drums = handCards.Where(x => x.GetType() == typeof(SpringleafDrum)).Count();
            var creatures = handCards.Where(x => x.Creature == true && x.ManaCost.ConvertedManaValue() == 0).Count();
            if (drums > 1 && creatures > 1)
                firstTurn = false;

            bool needsBlue = (handCards.Where(x => x.ManaCost != null && x.ManaCost.HaveBlue()).Count()
                            == handCards.Where(x => x.ManaCost != null).Count())
                && interaction.GetPlayCards().Where(x => x.ManaSource != null && x.ManaSource.HaveBlue()).Count() == 0;

            // TODO: check if I should keep into count lands that gives more than one mana.
            if (firstTurn)
            {
                // first land with blue, possibly entering game tapped.
                if (manaLands.Where(x => x.ManaSource != null && x.ManaSource.HaveBlue()).Any())
                    return manaLands
                                .OrderByDescending(x => x.Artifact)
                                .ThenByDescending(x => x.EntersGameTapped).ToList()
                                .FirstOrDefault(x => x.ManaSource.HaveBlue());

                // first land (anyland), possibly entering game tapped.
                return manaLands
                                .OrderByDescending(x => x.Artifact)
                                .ThenByDescending(x => x.EntersGameTapped)
                                .ThenBy(x => !x.ManaSource.HaveBlue()).FirstOrDefault();
            }
            else
            {
                // first land with blue, possibly not entering game tapped.
                if (manaLands.Where(x => x.ManaSource != null && x.ManaSource.HaveBlue()).Any() && needsBlue)
                    return manaLands
                                .OrderByDescending(x => x.Artifact)
                                .ThenBy(x => x.EntersGameTapped).ToList()
                                .FirstOrDefault(x => x.ManaSource.HaveBlue());

                // first land (anyland), possibly not entering game tapped.
                return manaLands
                                .OrderByDescending(x => x.Artifact)
                                .ThenBy(x => x.EntersGameTapped)
                                .ThenBy(x => !x.ManaSource.HaveBlue()).FirstOrDefault();
            }
        }

        private bool CanPlayCardsSupportSaga(IGameInteraction interaction)
        {
            // this is a delicate logic. We can play 1 saga foreach 2 lands (in play or in hand), but need
            // to keep into account lands that enters the game tapped and also Springleaf Drum
            // the goal is to play Saga at 2nd turn only if there are 2 mana (meaning also a sol ring)
            // examples
            // play saga at first turn and use it to play sol ring to cast Construct at second turn
            // play Springleaf drum && a creature at first turn and cast Saga at second turn
            // play saga at first turn use it to cast springleaf drum, only if there's a second land in hand and that land is not tapped
            // this gives an idea of how complex it is.
            // assume SpringLeaf drum and Solring will be played with priority (which it may not be true)
            // well at least I should prioritze them if there's a Saga.

            // assume springleaf drum and sol ring will be played
            // I can play both a land and Drum|Ring (not keeping into account Saga)

            // do not bother threating the rare case with 3 sagas... Eventually I will mulligan. And there will not
            // be anyway more than 2 in play at same time since they last 2 turns
            bool anyOtherSaga = interaction.GetPlayCards().Where(x => x.GetType() == typeof(UrzasSaga)).Any();
            int manaInPlay = ManaInPlay(interaction.GetPlayCards());

            if (anyOtherSaga)
            {
                // another saga is already into play "reserve" 2 mana for it
                manaInPlay -= 2;
            }

            // mana in hand refers to future mana so its ok to play another saga if we have another one
            // until we will have the extra 2 mana the following turn.
            int manaInHand = ManaInHand(interaction.GetHandCards());

            if (StrategyVariables.RiskOnSaga)
            {
                if (manaInHand == 0)
                    manaInHand = 1; // fake the reading of mana in hand.. assume we will draw a land or a mana
            }

            if (manaInHand >= 2)
                return true;

            if (manaInPlay == 1 && manaInHand == 1)
                return true;

            if (manaInPlay >= 2)
                return true;

            return false;
        }

        private int ManaInPlay(List<Card> play)
        {
            // if tapped this turn, will not be tapped the next turn
            var manaSources = play.Where(x => x.ManaSource != null && x.GetType() != typeof(UrzasSaga)).ToList();

            return manaSources.Select(x => x.ManaSource.ConvertedManaValue()).Sum();
        }

        private int ManaInHand(List<Card> hand)
        {
            // Basically we just check if there is a non-tappend land that will be played next turn,
            // or a sol ring. The springleaf drum would require a check on number of creatures (Ornitopther) TODO:

            // If return true, we are going to play Urza Sage, which gives 1 mana at first turn.
            // So we DON'T CHECK MANA cost of mana sources THIS TURN if the cost is 1 or less
            // because we "forecast" mana that will be used next turn

            // this turn we can't play another land. and we have just 1 mana at disposal. TODO: in late game we may afford
            // more expensive mana sources, but in that case we probably have already lands to support Saga.
            var manaSourceThanCanBePlayedThisTurn = hand.Where(
                x => x.ManaSource != null && x.Land == false && x.ManaCost.ConvertedManaValue() <= 1).ToList();

            // only lands that actually gives mana AND does not enter game tapped.
            var manaSourceThanCanBePlayedNextTurn = hand.Where(
                x => x.ManaSource != null && x.Land && x.EntersGameTapped == false).ToList();

            // TODO: play first cards that gives more mana (Sold ring etc.)
            int maxThisTurn = 0;
            if (manaSourceThanCanBePlayedThisTurn.Any())
                maxThisTurn = manaSourceThanCanBePlayedThisTurn.Select(x => x.ManaSource.ConvertedManaValue()).Max();

            int maxNextTurn = 0;
            if (manaSourceThanCanBePlayedNextTurn.Any())
                maxNextTurn = 1; // Do not know if playing the land that gives more mana is always the best strategy.
                                 // TODO: check if I should keep into count lands that gives more than one mana.

            return maxNextTurn + maxThisTurn;
        }

        List<Card> SelectFreeCards(List<Card> handDuplicate, IGameInteraction interaction, Card playedLand)
        {
            var zeroCost = handDuplicate.RemoveWithPredicate(x => x.ManaCost != null && x.ManaCost.ConvertedManaValue() == 0 && x.Land == false);
            List<Card> affinityZero = GetCardsAffinityZero(
                handDuplicate, zeroCost, interaction.GetPlayCards(), playedLand);
            return zeroCost.Concat(affinityZero).ToList();
        }

        private List<Card> GetCardsAffinityZero(List<Card> handDuplicate, List<Card> artifactsGonnaPlay, List<Card> play, Card playedLand)
        {
            var affinity = handDuplicate
                .Where(x => x.ManaCost != null)
                .OrderBy(x => x.ManaCost.ConvertedManaValue()).Where(x => x.Affinity).ToList();
            int artifacts = play.Where(x => x.Artifact).Count() + artifactsGonnaPlay.Where(x => x.Artifact).Count();
            if (playedLand != null && playedLand.Artifact)
                artifacts++;
            List<Card> cardsToPlay = new();

            foreach (var card in affinity)
            {
                var manaCostWithAffinity = card.ManaCost.ConvertedManaValueWithAffinity(artifacts);
                if (manaCostWithAffinity == 0)
                {
                    artifacts++;
                    cardsToPlay.Add(card);
                    handDuplicate.Remove(card);
                }
            }

            return cardsToPlay;
        }

        public bool ShouldMulligan(List<Card> hand)
        {
            if (StrategyVariables.OldMulligan)
                return !HaveDesiredCards(hand);
            else
                return !HaveDesiredCardsNew(hand);
        }

        private bool HaveDesiredCards(List<Card> hand)
        {
            //for combo decks mulligan choice is pretty simple
            // just check there are certain combinations of card
            // in affinity we just want 2 or 3 lands and one power combination
            // (1 creature + cranial plaiting || one power creature like urza saga or master of etherium)
            var mana = hand.Where(x => x.ManaSource != null && x.Land).Count();
            var creatures = hand.Where(x => x.Creature).Count();
            var powerhouses = hand.Where(x => x.GetType() == typeof(UrzasSaga) || x.GetType() == typeof(CranialPlating)
            || x.GetType() == typeof(MasterOfEtherium) || x.GetType() == typeof(SteelOverseer)).Count();

            if (hand.Where(x => x.GetType() == typeof(UrzasSaga)).Count() > 2)
                return false;

            return mana >= 2 && mana <= 3 && creatures > 0 && powerhouses > 0;
        }

        private bool HaveDesiredCardsNew(List<Card> hand)
        {
            //for combo decks mulligan choice is pretty simple
            // just check there are certain combinations of card
            // in affinity we just want 2 or 3 lands and one power combination
            // (1 creature + cranial plaiting || one power creature like urza saga or master of etherium)
            var mana = DesiredMana(hand);
            var creatures = hand.Where(x => x.Creature).Count();
            var powerhouses = hand.Where(x => x.GetType() == typeof(UrzasSaga) || x.GetType() == typeof(CranialPlating)
            || x.GetType() == typeof(MasterOfEtherium) || x.GetType() == typeof(SteelOverseer)).Count();

            bool urza = false;
            if(hand.Any( x => x.GetType() == typeof(UrzasSaga)))
            {
                urza = mana >= 2 && creatures >= 1;
            }

            bool cranial = false;
            if(hand.Any(x => x.GetType() ==typeof(CranialPlating)))
            {
                cranial = creatures >= 2;
                if (hand.Where(x => x.Creature).All(x => x.GetType() == typeof(MasterOfEtherium)))
                    cranial = creatures >= 2 && mana >= 3;
                else
                    cranial = creatures >= 2 && mana >= 2;
            }

            bool master = false;
            if(hand.Any( x=> x.GetType() == typeof(MasterOfEtherium)))
            {
                master = creatures>=2 && mana>=3;
            }

            bool overseer = false;
            if(hand.Any( x=> x.GetType() == typeof(SteelOverseer)))
            {
                overseer = creatures >= 3 && mana>=2;
            }

            return overseer || master || cranial || urza;
        }

        private int DesiredMana(List<Card> hand)
        {
            // just lands
            var lands = hand.Where(x => x.ManaSource != null && x.Land && x.GetType() != typeof(UrzasSaga)).Count();

            var solRing = hand.Where(x => x.GetType() == typeof(SolRing)).Count();
            var drums = hand.Where(x => x.GetType() == typeof(SpringleafDrum)).Count();
            var manaCreatures = hand.Select(x => x.GetType())
                .Count(x => x == typeof(Ornithopter) || x == typeof(Memnite) || x== typeof(SignalPest));

            if (lands >= 1)
                return lands + 2 * solRing + Math.Min(drums, manaCreatures);

            return 0;
        }

        public Card SelectCreatureToEquip(Card equipment, IGameInteraction gameInteraction)
        {
            var cards = gameInteraction.GetPlayCards();

            return cards.Where(x => x.Creature)
                .OrderBy(x => x.Status_Tapped || x.Status_Weakness) // first untapped creatures
                .ThenBy(x => gameInteraction.GetCardPower(x)) // first equip low power creatures
                .ThenByDescending(x => x.Flying) // then first creatures with flying
                .FirstOrDefault();
        }

        public void ActivateAbilities(List<IActivatedAbility> abilities, IGameInteraction gameInteraction, bool allowedSorceries)
        {
            var sagaAbility = abilities.Where(x => x.GetType() == typeof(UrzasSagaAbility) && (x.Owner.Status_Tapped == false)).ToList();

            foreach (var ability in sagaAbility)
                gameInteraction.ActivateAbility(ability);

            if (allowedSorceries)
            {
                bool anyCreatureInPlay = gameInteraction.GetPlayCards().Where(x => x.Creature).Any();
                if (anyCreatureInPlay)
                {
                    var platingAbility = abilities
                        .Where(x => x.GetType() == typeof(CranialPlatingAbility))
                        .Where(x => x.Owner.EquippedTo == null).ToList(); // do not equip already equipped equipment
                    foreach (var ability in platingAbility)
                        gameInteraction.ActivateAbility(ability);
                }
            }

            var overseerAbilities = abilities.Where(x => x.GetType() == typeof(SteelOverseerAbility) && x.Owner.Status_Tapped == false).ToList();

            // TODO: simulate attack phase without tapping (otherwise we don't get SignalPest and other effects into account)
            int totalDamageDealt = gameInteraction.GetPlayCards().Where(x => x.Creature).Select(x => gameInteraction.GetCardPower(x)).Sum();
            int creatures = gameInteraction.GetPlayCards().Where(x => x.Creature).Count();

            // this algorithm can IMPROVE a lot: TODO: what the fuss by such a simple card. If I do proper math
            // probably there is a way to compute it immediatly without recursion. Also It should keeps int account
            // the coming in to play of new creatures.
            foreach (var ability in overseerAbilities)
            {
                // now the problem is that +1/+1 counters are cumulative, so a lesser damage is acceptable if following turns damage increase.
                // thus we should simulate all possible outcomes that are a binary combinations so for 6 turns there are 64 combinations.
                int ownerPower = gameInteraction.GetCardPower(ability.Owner);
                int lifeLeft = gameInteraction.FoeLifeLeft();

                var turns1 = SimulateOverseerRecursive(true, 0, totalDamageDealt, ownerPower, lifeLeft, 0, creatures);
                var turns2 = SimulateOverseerRecursive(false, 0, totalDamageDealt, ownerPower, lifeLeft, 0, creatures);
                if (turns2 < turns1)
                {
                    break; // creatures will all attack
                }
                else
                {
                    // activate abilities
                    gameInteraction.ActivateAbility(ability);
                }
            }

            var atogAbility = abilities.Where(x => x.GetType() == typeof(AtogAbility)).FirstOrDefault(); //just activate one atog
            if (atogAbility != null && atogAbility.Owner.Status_Tapped == false && atogAbility.Owner.Status_Weakness == false)
            {
                var artifacts = GetArtifactsICanSacrifice(gameInteraction);
                var sacrificedDamage = artifacts.Where(x => gameInteraction.GetCardPower(x) == 1).Count();
                var artifactcount = artifacts.Count();
                //interested in activating atog ability: however there are still conditions
                int powerhouses = gameInteraction.GetPlayCards().Where(x => x.GetType() == typeof(Construct)
                                           || x.GetType() == typeof(MasterOfEtherium) || x.GetType() == typeof(CranialPlating)).Count();



                if (powerhouses < 2 &&
                    artifactcount > 0 && (gameInteraction.FoeLifeLeft() <
                                                                       (artifactcount * 2 + totalDamageDealt - artifactcount * powerhouses - sacrificedDamage)
                                         )
                                          )
                {

                    foreach (var artifact in artifacts)
                    {
                        gameInteraction.ActivateAbility(atogAbility);
                    }

                }
            }
        }

        private int SimulateOverseerRecursive(bool tap, int turn, int totalDamage, int ownerPower, int lifeLeft, int counters, int creatures)
        {
            if (turn == 6)
                return turn; // do not make simulation too deep.

            int damageDealt = 0;
            if (tap)
            {
                damageDealt = (totalDamage - ownerPower) + (creatures - 1) * (counters + 1);
            }
            else
            {
                damageDealt = (totalDamage) + creatures * counters;
            }

            if (damageDealt > lifeLeft)
                return turn;

            // simulate turns where this turn is tapping
            var turns1 = SimulateOverseerRecursive(true, turn + 1, totalDamage, ownerPower, lifeLeft - damageDealt, counters + 1, creatures);

            // simulate turns where this turn is not tapping
            var turns2 = SimulateOverseerRecursive(false, turn + 1, totalDamage, ownerPower, lifeLeft - damageDealt, counters, creatures);

            if (turns2 < turns1)
            {
                return turns2;
            }
            else
            {
                return turns1;
            }
        }

        public List<Card> SelectCardsToPlay(IGameInteraction gameInteraction)
        {
            // Just sort hand and play everything we can play?

            List<Card> cardsToPlay = new();
            var cardsInPlay = gameInteraction.GetPlayCards();
            var cardsInHand = gameInteraction.GetHandCards();

            // prioritize sol ring
            if (cardsInHand.Where(x => x.GetType() == typeof(SolRing)).Any())
                cardsToPlay.Add(cardsInHand.First(x => x.GetType() == typeof(SolRing)));

            // This function is called continuosly so.. Just check if there's one untapped creature
            // playing a springleaf drum is "free" if there is a creature into play
            if (CreaturesICanTapAsCost(gameInteraction).Count() >= 1 && cardsToPlay.Any() == false)
            {
                var aDrum = cardsInHand.Where(x => x.GetType() == typeof(SpringleafDrum)).FirstOrDefault();
                if (aDrum != null)
                    cardsToPlay.Add(aDrum);
            }

            if (cardsInPlay.Where(x => x.Creature).Any() && cardsToPlay.Any() == false)
            {
                var aPlate = cardsInHand.Where(x => x.GetType() == typeof(CranialPlating)).FirstOrDefault();
                if (aPlate != null)
                    cardsToPlay.Add(aPlate);
            }

            if (cardsInHand.Where(x => x.Creature).Any() && cardsToPlay.Any() == false)
            {
                var sortOrder = new Dictionary<Type, int>()
                {
                    {typeof(MasterOfEtherium),      0},
                    {typeof(MyrEnforcer),           10},
                    {typeof(SojournersCompanion),   20},
                    {typeof(Frogmite),              30},
                    {typeof(SignalPest),            40},
                    {typeof(Atog),                  50}
                };

                var creaturesInplay = gameInteraction.GetPlayCards().Where(x => x.Creature).Count();

                if (creaturesInplay > 1)
                {
                    sortOrder[typeof(SignalPest)] = 29;
                }

                if (creaturesInplay > 3)
                {
                    sortOrder[typeof(SignalPest)] = 9;
                }

                var creatures = cardsInHand.Where(x => x.Creature).OrderBy(
                        x => sortOrder.ContainsKey(x.GetType()) ? sortOrder[x.GetType()] : 1000
                    );

                cardsToPlay.AddRange(creatures);
            }

            // if no cards "by strategy" to play just sort by mana cost to play anyway something. Assuming bigger cost
            // is better card.. Bad Idea. If I start by lower cost I may benefit from affinity... Or no? StrategyVariable TODO:
            if (cardsToPlay.Count == 0)
            {
                if (StrategyVariables.PlayLeftOverCardsFromCheaper)
                    cardsToPlay = cardsInHand.Where(x => x.ManaCost != null).OrderBy(x => x.ManaCost.ConvertedManaValue()).ToList();
                else
                    cardsToPlay = cardsInHand.Where(x => x.ManaCost != null).OrderByDescending(x => x.ManaCost.ConvertedManaValue()).ToList();
            }


            return cardsToPlay;
        }

        public int HowManyCreaturesICanTapAsCost(Card source, IGameInteraction gameInteraction)
        {
            return CreaturesICanTapAsCost(gameInteraction).Count;
        }

        List<Card> CreaturesICanTapAsCost(IGameInteraction gameInteraction)
        {
            // very simple euristic, get cards with power less or equal to 2, and tap first the cards with less power.
            // this is stupid because some creature may have deatouch.. however I could equip first most "powerfull" creatures
            // with cranial plating, and once the creature is equipped it is no longer "tapped" by this function (TODO).
            var playCards = gameInteraction.GetPlayCards();

            var cards = playCards.Where(x => x.Creature && gameInteraction.GetCardPower(x) <= 2 && x.Status_Tapped == false);
            var finalCards = cards
                .OrderBy(x => x.AttackPhaseEffects.Count)
                .OrderBy(x => gameInteraction.GetCardPower(x)).ToList();
            return finalCards;
        }

        public void TapCreatureAsCost(Card source, IGameInteraction gameInteraction)
        {
            CreaturesICanTapAsCost(gameInteraction).First().Status_Tapped = true;
        }

        public Card FindCardInDeck(Card source, Func<Card, bool> filter, IGameInteraction gameInteraction)
        {
            var sortOrder = new Dictionary<Type, int>()
                {
                    {typeof(SolRing),               0},
                    {typeof(SpringleafDrum),        1},
                    {typeof(SignalPest),            2},
                    {typeof(Ornithopter),           3}
                };

            var spring = gameInteraction.GetPlayCards().Where(x => x.GetType() == typeof(SpringleafDrum)).Any();

            if (spring)
            {
                sortOrder[typeof(SpringleafDrum)] = 2;
                sortOrder[typeof(SignalPest)] = 1;
            }

            var cards = gameInteraction.GetGrimoireCards().Where(x => filter(x))
                .OrderBy(x => sortOrder.ContainsKey(x.GetType()) ? sortOrder[x.GetType()] : 1000);

            if (cards.Any())
                return cards.First();

            return null;
        }

        public List<Card> GetArtifactsICanSacrifice(IGameInteraction gameInteraction)
        {
            return gameInteraction.GetPlayCards().Where(x => x.Artifact).Where(
                x => (x.Creature && gameInteraction.GetCardPower(x) <= 1)
                    || (!x.Creature && x.GetType() != typeof(CranialPlating)))

                .ToList();
        }
    }
}
