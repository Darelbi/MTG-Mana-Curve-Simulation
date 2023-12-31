﻿using MTG.Cards;
using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Effects;
using MTG.Cards.Cards.Feats;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game.Utils;
using MTG.Mana;
using System.Collections.ObjectModel;

namespace MTG.Game
{
    public class Game : IGameInteraction
    {
        private readonly Grimoire grimoire;
        private readonly Effects effects;
        private readonly IStrategy strategy;
        private readonly bool meStarting;
        private readonly List<Card> hand;
        private readonly List<Card> play;
        private readonly List<Card> grave;
        private readonly List<Card> playedCardsThisTurn;
        private int damageDealt;
        private int playedLands;
        private int damageReceived;

        private int turn;
        public Game(Grimoire grimoire, IStrategy strategy, bool meStarting)
        {
            this.grimoire = grimoire;
            this.strategy = strategy;
            this.meStarting = meStarting;
            this.effects = new Effects();
            hand = new List<Card>();
            play = new List<Card>();
            grave = new List<Card>();
            playedCardsThisTurn = new List<Card>();
            turn = 1;
            damageDealt = 0;
            damageReceived = 0;
        }

        public int GetDamageReceived()
        {
            return damageReceived;
        }

        public void ReceiveDamage(int damage)
        {
            damageReceived += damage;
        }

        public void Mulligan()
        {
            Console.WriteLine("\n--Mulligan!\n");
            foreach (var card in hand)
            {
                grimoire.PutCardBack(card);
            }

            hand.Clear();
        }

        public void PrintHand()
        {
            foreach (var card in hand)
                Console.WriteLine(card.CardName);
        }

        public void Draw(int toDraw)
        {
            for (int i = 0; i < toDraw; i++)
            {
                hand.Add(grimoire.DrawCard());
            }
        }

        public void BeginTestGame(int toDraw)
        {
            Draw(toDraw);
        }

        public void BeginGame()
        {
            int toDraw = 7;
            Draw(toDraw);

            RecomputePotentialDynamicMana(); // otherwise we don't mulligan correctly the tolarian academy
            while (strategy.ShouldMulligan(hand) && toDraw > 4)
            {
                Mulligan();
                Draw(--toDraw);
            }

            PrintHand();
        }

        void Draw()
        {
            if (turn == 1 && meStarting)
                return;

            var card = grimoire.DrawCard();
            Console.WriteLine($"Drawed: {card.CardName}\n");
            hand.Add(card);
        }

        public bool HasWon()
        {
            return damageDealt >= 30;
        }

        public int TotalDamageDealt()
        {
            return damageDealt;
        }

        public void Turn()
        {
            Console.WriteLine($"\n TURN \n");
            playedCardsThisTurn.Clear();

            // begin turn phase
            UntapPhase(); // TODO: lore counting is done after drawing.... does not matter for now.

            UpkeepPhase();
            Draw();

            MainPhase();

            CombatPhase();

            // MainPhase(); // in a simulation we play everything we can in first MainPhase().

            EndPhase();


            turn++;
        }

        private void UpkeepPhase()
        {
            foreach (var card in play)
                card.Upkeep?.PayUpkeep(card, this);
        }

        private void EndPhase()
        {
            List<Card> cardsToSacrifice = new();
            foreach (var card in play)
                if (card.SacrificeIfNoArtifacts && play.Where(x => x.Artifact).Count() == 0)
                    cardsToSacrifice.Add(card);

            foreach (var card in cardsToSacrifice)
            {
                play.Remove(card);
                grave.Add(card);
            }

            effects.RemoveUntilEndOfTurnEffects();
        }

        private void CombatPhase()
        {
            // there are no foes. So we just assume all creatures are attacking. so we don't need really any strategy for that
            var attacking = play.Where(
                x => x.Creature
                && x.Status_Tapped == false
                && x.Status_Weakness == false);

            var attacckEffects = attacking.Where(
                x => x.AttackPhaseEffects != null && x.AttackPhaseEffects.Count > 0)
                .Select(x => x.AttackPhaseEffects).SelectMany(x => x).ToList();

            effects.AddAttackPhaseEffects(attacckEffects);

            foreach (var attack in attacking)
            {
                attack.Status_Attacking = true;

                damageDealt += GetCardPower(attack);
                if (attack.AttackWithoutTapping == false)
                    attack.Status_Tapped = true;

                attack.Status_Attacking = false;
            }

            effects.RemoveAttackPhaseEffects(attacckEffects);
        }

        public void RecomputeDynamicMana()
        {
            var affectedCards = GetHandCards().Concat(GetPlayCards());

            foreach (var card in affectedCards)
            {
                if (card != null)
                {
                    if (card.Features.ContainsKey(typeof(IDynamicManaValue)))
                    {
                        ((IDynamicManaValue)card.Features[typeof(IDynamicManaValue)]).ActualManaVaule(card, this);
                    }
                }
            }
        }

        public void RecomputePotentialDynamicMana()
        {
            var affectedCards = GetHandCards().Concat(GetPlayCards());

            foreach (var card in affectedCards)
            {
                if (card != null)
                {
                    if (card.Features.ContainsKey(typeof(IDynamicManaValue)))
                    {
                        ((IDynamicManaValue)card.Features[typeof(IDynamicManaValue)]).PotentialManaValue(card, this);
                    }
                }
            }
        }

        private void MainPhase()
        {
            var selectedFreeCards = strategy.SelectManaAndFreeCardsToPlay(this);

            foreach (var card in selectedFreeCards)
                PutCardInPlayFromHand(card);

            ActivateAbilities(true);

            bool tappedMana = true;

            while (tappedMana)
            {
                tappedMana = false;

                var selectedCostlyCards = strategy.SelectCardsToPlay(this);
                foreach (var card in selectedCostlyCards)
                {
                    // THE CHECK IS STUPID BECAUSE AS A RESULT OF A CARD I CAN DRAW FREE CARDS AND PLAY THESE NOW
                    //if (card.ManaCost == null || card.ManaCost.ConvertedManaValue() == 0)
                    //    throw new System.Exception("Free cards should be played before this point");

                    if (TapMana(card.ManaCost, false, card, card.Affinity))
                    {
                        tappedMana = true; // played a card. Maybe its a mana source so repeat to find additional cards to play 
                                           // this is a very greed and basic approach. but it works in most cases (for affinity deck)
                        PutCardInPlayFromHand(card);
                    }
                }

                selectedFreeCards = strategy.SelectManaAndFreeCardsToPlay(this);

                foreach (var card in selectedFreeCards)
                    PutCardInPlayFromHand(card);
            }

            DestroyLeftoverMana();
        }

        private void DestroyLeftoverMana()
        {
            play.RemoveAll(x => x.Status_UntappedMana == true);
        }

        private void ActivateAbilities(bool allowedSorceries)
        {
            // get all abilities
            var abilities = play.Select(x => x.Abilities).SelectMany(x => x).ToList();

            // TODO: check if theere are abilities to be played
            strategy.ActivateAbilities(abilities, this, allowedSorceries);
        }

        private void UntapPhase()
        {
            playedLands = 0;

            foreach (var card in play.ToList())
            {
                if (card.DontUntap == false)
                    card.Status_Tapped = false;
                card.Status_Weakness = false;
            }

            ActivateAbilities(false);

            foreach (var card in play.ToList())
            {
                var feat = card.GetFeat<IOnBeginPhaseFeat>();
                feat?.OnBeginPhase(card, this);
            }

            DestroyLeftoverMana();
        }

        public List<Card> GetPlayCards()
        {
            return play.ToList(); // duplicate avoid accidental modifying cards in play without triggerin an event
        }

        public List<Card> GetHandCards()
        {
            return hand.ToList();
        }

        public void Sacrifice(Card card)
        {
            if (card.IsCard)
            {
                play.Remove(card);
                grave.Add(card);
            }
            else
            {
                // tokens get removed from game
                play.Remove(card);
            }
        }

        public List<Card> GetPlayedCardsThisTurn()
        {
            return playedCardsThisTurn.ToList();
        }

        public List<Card> GetCardsInPlayThisTurn()
        {
            return play.ToList();
        }

        public void PutCardInPlay(Card card)
        {
            Console.WriteLine($"played: {card.CardName}");

            if (card.Creature && !card.Haste)
                card.Status_Weakness = true;

            if (card.EntersGameTapped) // && TODO: ! Amulet Of Vigor
                card.Status_Tapped = true;

            if (card.Land)
                playedLands++;

            playedCardsThisTurn.Add(card);

            if (card.Sorcery || card.Instant)
            {
                var spell = (ICastEffect)card;
                spell.CastEffect(this);
                grave.Add(card);
                return;
            }

            var feat = card.GetFeat<IOnEnterGameFeat>();
            feat?.OnEnterGame(card, this);

            play.Add(card);

            effects.CardEnterPlay(card);

        }

        public void PutCardInGraveyardFromPlay(Card card)
        {
            CardLeavesPlay(card);
            // TODO: card reset OR create clone with reflection and discard this
        }

        public void CardLeavesPlay(Card card)
        {
            card.PlusOnePlusOneTokens = 0;

            effects.CardExitPlay(card);

            if (card.EquippedTo != null)
            {
                card.EquippedTo.EquippedEquipment.Remove(card);
                card.EquippedTo = null;
            }

            if (card.EquippedEquipment.Any())
            {
                foreach (var item in card.EquippedEquipment)
                {
                    item.EquippedTo = null;
                }
                card.EquippedEquipment.Clear();
            }

        }

        private void PutCardInPlayFromDeck(Card card)
        {
            grimoire.Remove(card);
            PutCardInPlay(card);
        }

        public void PutCardInPlayFromHand(Card card)
        {
            hand.Remove(card);
            PutCardInPlay(card);
        }

        public void ActivateAbility(IActivatedAbility ability)
        {
            // this is stupid, there should be a strategy just for mana allocation on all possible stuff to do. TODO: NOT EASY TO DO
            // Ideally I need a IManaStrategy class. But don't want to bother for now
            if (TapMana(ability.ManaCost, ability.TapOnActivate, ability.Owner, false)) // alwasy false activating ability as affinity
            {
                ability.Outcome(this);
                if (ability.TapOnActivate)
                {
                    if (ability.Owner.Status_Tapped == true)
                        throw new System.Exception("Cannot tap if ability is tapped and requires tap as cost");

                    ability.Owner.Status_Tapped = true;
                }
            }
        }

        private bool TapMana(Mana.Mana manaCost, bool excludingCardIsBeingTappedAndCantBeSelected, Card exclude, bool affinity)
        {
            RecomputeDynamicMana();
            List<Card> manaSources = null;

            if (excludingCardIsBeingTappedAndCantBeSelected)
                manaSources = play.Where(x => x != exclude && x.ManaSource != null && x.Status_Tapped == false).ToList();
            else
                manaSources = play.Where(x => x.ManaSource != null && x.Status_Tapped == false).ToList();

            var toTap = FindCardsToTap(manaCost, affinity, manaSources);

            if (toTap != null)
            {
                foreach (var card in toTap)
                {
                    card.Status_Tapped = true;
                    if (card.ManaSourcePrice != null)
                        card.ManaSourcePrice.PayPrice(card, this);
                }

                return true;
            }

            return false;
        }

        // TODO: this can be improved a lot
        private List<Card> FindCardsToTap(Mana.Mana manaCost, bool affinity, List<Card> manaSources)
        {
            if (manaSources == null)
                throw new System.ArgumentException("manaSources null expected at least a empty list");

            var cardsWithAdditionalCostsGroups = manaSources.Where(x => x.ManaSourcePrice != null && x.Status_Tapped == false)
                .GroupBy(x => x.ManaSourcePrice.GetType());

            if (cardsWithAdditionalCostsGroups != null)

                foreach (var group in cardsWithAdditionalCostsGroups)
                {
                    // this is stupid. but better than nothing. TODO: There's nothing that can be done easily for that
                    // basically group SourcePrice by type, ASSUMING DIFFERENT TYPES HAVE DIFFERENT KIND OF COSTS (WHICH IS NOT TRUE FOR ALL CARDS, TODO)
                    // for each group we count how many costs can be payed, this is selected by strategy with game as proxy.
                    // cards that exceed the ammissible cost are removed from mana sources.
                    var cardsInGroup = group.ToList();
                    var first = cardsInGroup.First();
                    int pricesCanPay = first.ManaSourcePrice.HowManyCanPayPrice(first, this);
                    int toRemove = cardsInGroup.Count() - pricesCanPay;
                    if (toRemove < 0)
                    {
                        toRemove = 0;
                    }

                    if (toRemove > 0 && toRemove <= cardsInGroup.Count)
                        foreach (var cardToRemove in cardsInGroup.GetRange(0, toRemove))
                        {
                            manaSources.Remove(cardToRemove);
                        }
                }

            List<Card> toTap = new();
            var sortedSources = manaSources
                // Pay first sources that do not have additional costs regardless if we can pay the cost
                .Where(x => x.GetType() != typeof(UrzasSaga) || (x.GetType() == typeof(UrzasSaga) && x.LoreCounter <= 1))
                .OrderBy(x => x.GetType() == typeof(UrzasSaga))
                .ThenBy(x => x.ManaSourcePrice != null)
                .ThenByDescending(x => x.ManaSource.ConvertedManaValue())
                .ThenBy(x => x.ManaSource.ManaComplexity()).ToList();

            int blue = manaCost.TotalBlueValue();
            for (int i = 0; i < blue; i++)
                if (sortedSources.Any(x => x.ManaSource.HaveBlue())) // why not a enum for colors? sigh
                {
                    var card = sortedSources.First(x => x.ManaSource.HaveBlue());

                    int tappedMana = card.ManaSource.ConvertedManaValue();
                    i += tappedMana;

                    if (i >= blue)
                        sortedSources.AddRange(CreateBlueLeftoverMana(i - blue));

                    toTap.Add(card);
                    sortedSources.Remove(card);

                    //for now implement leftover just for blue and colorless
                    //if(card.)
                }
                else
                {
                    return null;
                }

            int black = manaCost.TotalBlackValue();
            for (int i = 0; i < black; i++)
                if (sortedSources.Any(x => x.ManaSource.HaveBlack()))
                {
                    var card = sortedSources.First(x => x.ManaSource.HaveBlack());
                    toTap.Add(card);
                    sortedSources.Remove(card);
                }
                else
                {
                    return null;
                }

            int red = manaCost.TotalRedValue();
            for (int i = 0; i < red; i++)
                if (sortedSources.Any(x => x.ManaSource.HaveRed()))
                {
                    var card = sortedSources.First(x => x.ManaSource.HaveRed());
                    toTap.Add(card);
                    sortedSources.Remove(card);
                }
                else
                {
                    return null;
                }

            int colorless = manaCost.TotalColorlessValue();

            if (affinity)
                colorless -= play.Where(x => x.Artifact).Count();

            if (colorless < 0)
                colorless = 0;

            for (int i = 0; i < colorless;)
            {
                var card = sortedSources.FirstOrDefault();
                if (card == null)
                    return null;

                // I tap anyway. TODO: should do more complex check because there are mana sources that tap more than 1 mana
                // in this way I could end up using Sol Ring to pay for springleaf Drum... that's ok for now
                // the average case is triggering ability of Urza Saga with Sol ring. TODO: that's why the simulation removed
                // springleaf drum from deck. I need to implements leftover mana
                int tappedMana = card.ManaSource.ConvertedManaValue();
                i += tappedMana;

                if (i >= colorless)
                    CreateColorlessLeftoverMana(i - colorless);

                toTap.Add(card);
                sortedSources.Remove(card);
            }

            return toTap;
        }

        public List<Card> CreateColorlessLeftoverMana(int amount)
        {
            var cards = new List<Card>();

            for (int i = 0; i < amount; i++)
            {
                var card = new Card()
                {
                    ManaSource = new Mana.Mana { Colorless = 1 },
                    Status_UntappedMana = true
                };

                cards.Add(card);
                play.Add(card);
            }

            return cards;
        }

        public List<Card> CreateBlueLeftoverMana(int amount)
        {
            var cards = new List<Card>();

            for (int i = 0; i < amount; i++)
            {
                var card = new Card()
                {
                    ManaSource = new Mana.Mana { Blue = 1 },
                    Status_UntappedMana = true
                };

                cards.Add(card);
                play.Add(card);
            }

            return cards;
        }

        public void TapCreatureAsCost(Card source)
        {
            strategy.TapCreatureAsCost(source, this);
        }

        public int HowManyCreaturesICanTapAsCost(Card source)
        {
            return strategy.HowManyCreaturesICanTapAsCost(source, this);
        }

        public int GetCardPower(Card card)
        {
            if (!play.Contains(card))
                throw new System.ArgumentException("Power can be computed only for cards in play");

            int totalPower = card.Power;

            totalPower += card.PlusOnePlusOneTokens;

            // TO save on performance I should use a "ArtifactCounter Watch" (TODO).
            // so I can return its value directly instead of iterating all artifacts everytime.
            // However this adds unnecessary complexity and the project is already too complex
            if (card.Features.ContainsKey(typeof(IPowerFeat)))
            {
                totalPower +=
                ((IPowerFeat)card.Features[typeof(IPowerFeat)]).GetPower(card, this);
            }

            foreach (var equipment in card.EquippedEquipment)
            {
                if (equipment.Features.ContainsKey(typeof(IEquippedPowerFeat)))
                {
                    totalPower +=
                    ((IEquippedPowerFeat)equipment.Features[typeof(IEquippedPowerFeat)]).GetEquippedPower(card, this);
                }
            }

            totalPower += effects.GetPowerIncreaseForCard(card, this);

            // missing temporary effects (like Signal Pest) etc.. (TODO)

            return totalPower;
        }

        public void Equip(Card equipment)
        {
            // call strategy to decide to which creature equip the equipment
            var creature = strategy.SelectCreatureToEquip(equipment, this);
            if (creature == null)
            {
                throw new System.Exception("Why was equip called if there are no equippable creatures?");
            }
            Console.WriteLine($"equipped: {equipment.CardName} to {creature.CardName}");
            creature.EquippedEquipment.Add(equipment);
            equipment.EquippedTo = creature;
        }

        public void FindCardToPlayFromDeck(Card source, Func<Card, bool> filter)
        {
            Card card = strategy.FindCardInDeck(source, filter, (IGameInteraction)this);
            if (card != null)
                PutCardInPlayFromDeck(card);
        }

        public ReadOnlyCollection<Card> GetGrimoireCards()
        {
            return grimoire.GetAllCards();
        }

        public void DrawFromGame(Card soruce, int howmanycards)
        {
            for (int i = 0; i < howmanycards; i++)
            {
                var card = grimoire.DrawCard();
                Console.WriteLine($"Drawed: {card.CardName}");
                hand.Add(card);
            }
        }

        public bool CanPlayLand()
        {
            return playedLands < GetMaxPlayableLands();
        }

        private int GetMaxPlayableLands()
        {
            return 1;
        }

        public int FoeLifeLeft()
        {
            return 30 - damageDealt;
        }

        public List<Card> GetArtifactsICanSacrifice()
        {
            return strategy.GetArtifactsICanSacrifice(this);
        }

        public void AddUntilEndOfTurnEffects(IEffect effect)
        {
            effects.AddUntilEndOfTurnEffects(effect);
        }
    }
}
