using MTG.Cards;
using MTG.Cards.Cards.ActivatedAbilities;
using MTG.Cards.Cards.Feats;
using MTG.Game.Utils;
using MTG.Mana;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            Draw();

            MainPhase();

            CombatPhase();

            // MainPhase(); // in a simulation we play everything we can in first MainPhase().

            EndPhase();


            turn++;
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
        }

        private void CombatPhase()
        {
            // there are no foes. So we just assume all creatures are attacking. so we don't need really any strategy for that
            var attacking = play.Where(
                x => x.Creature 
                && x.Status_Tapped == false
                && GetCardPower(x) > 0
                && x.Status_Weakness == false);

            foreach(var attack in attacking)
            {
                damageDealt += GetCardPower(attack);
                if (attack.AttackWithoutTapping == false)
                    attack.Status_Tapped = true;
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
                    if (card.ManaCost == null || card.ManaCost.ConvertedManaValue() == 0)
                        throw new System.Exception("Free cards should be played before this point");

                    if (TapMana(card.ManaCost, false, card, card.Affinity))
                    {
                        tappedMana = true; // played a card. Maybe its a mana source so repeat to find additional cards to play 
                                           // this is a very greed and basic approach. but it works in most cases (for affinity deck)
                        PutCardInPlayFromHand(card);
                    }
                }
            }
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
            foreach (var card in play.ToList())
            {
                card.Status_Tapped = false;
                card.Status_Weakness = false;
            }

            ActivateAbilities(false);

            foreach (var card in play.ToList())
            {
                var feat = card.GetFeat<IOnBeginPhaseFeat>();
                feat?.OnBeginPhase(card, this);
            }

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
            if (card.Creature && !card.Haste)
                card.Status_Weakness = true;

            if (card.EntersGameTapped) // && TODO: ! Amulet Of Vigor
                card.Status_Tapped = true;

            playedCardsThisTurn.Add(card);

            var feat = card.GetFeat<IOnEnterGameFeat>();
            feat?.OnEnterGame(card, this);

            play.Add(card);

            effects.CardEnterPlay(card);

            Console.WriteLine($"played: {card.CardName}");
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

            if(card.EquippedTo!=null)
            {
                card.EquippedTo.EquippedEquipment.Remove(card);
                card.EquippedTo = null;
            }

            if(card.EquippedEquipment.Any())
            {
                foreach(var item in card.EquippedEquipment)
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
                    if(card.ManaSourcePrice!=null)
                        card.ManaSourcePrice.PayPrice(card, this);
                }

                return true;
            }

            return false;
        }

        // TODO: this can be improved a lot
        private List<Card> FindCardsToTap(Mana.Mana manaCost, bool affinity, List<Card> manaSources)
        {
            var cardsWithAdditionalCostsGroups = manaSources.Where(x => x.ManaSourcePrice != null && x.Status_Tapped == false)
                .GroupBy(x => x.ManaSourcePrice.GetType());

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

                foreach (var cardToRemove in cardsInGroup.GetRange(0, toRemove))
                {
                    manaSources.Remove(cardToRemove);
                }
            }

            List<Card> toTap = new();
            var sorted = manaSources
                // Pay first sources that do not have additional costs regardless if we can pay the cost
                .OrderBy(x => x.ManaSourcePrice != null)
                .OrderByDescending(x => x.ManaSource.ConvertedManaValue())
                .ThenBy(x => x.ManaSource.ManaComplexity()).ToList();

            int blue = manaCost.TotalBlueValue();
            for (int i = 0; i < blue; i++)
                if (sorted.Any(x => x.ManaSource.HaveBlue())) // why not a enum for colors? sigh
                {
                    var card = sorted.First(x => x.ManaSource.HaveBlue());
                    toTap.Add(card);
                    sorted.Remove(card);
                }
                else
                {
                    return null;
                }

            int black = manaCost.TotalBlackValue();
            for (int i = 0; i < black; i++)
                if (sorted.Any(x => x.ManaSource.HaveBlack()))
                {
                    var card = sorted.First(x => x.ManaSource.HaveBlack());
                    toTap.Add(card);
                    sorted.Remove(card);
                }
                else
                {
                    return null;
                }

            int red = manaCost.TotalRedValue();
            for (int i = 0; i < red; i++)
                if (sorted.Any(x => x.ManaSource.HaveRed()))
                {
                    var card = sorted.First(x => x.ManaSource.HaveRed());
                    toTap.Add(card);
                    sorted.Remove(card);
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
                var card = sorted.FirstOrDefault();
                if (card == null)
                    return null;

                // I tap anyway. TODO: should do more complex check because there are mana sources that tap more than 1 mana
                // in this way I could end up using Sol Ring to pay for springleaf Drum... that's ok for now
                // the average case is triggering ability of Urza Saga with Sol ring.
                int tappedMana = card.ManaSource.ConvertedManaValue();
                i += tappedMana;

                toTap.Add(card);
                sorted.Remove(card);
            }

            return toTap;
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

            foreach( var equipment in card.EquippedEquipment)
            {
                if (card.Features.ContainsKey(typeof(IEquippedPowerFeat)))
                {
                    totalPower +=
                    ((IEquippedPowerFeat)card.Features[typeof(IEquippedPowerFeat)]).GetEquippedPower(card, this);
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
            Debug.WriteLine($"equipped: {equipment.CardName} to {creature.CardName}");
            creature.EquippedEquipment.Add(equipment);
            equipment.EquippedTo=creature;
        }

        public void FindCardToPlayFromDeck(Card source, Func<Card, bool> filter)
        {
            Card card = strategy.FindCardInDeck(source, filter, (IGameInteraction)this);
            if(card!=null)
                PutCardInPlayFromDeck(card);
        }

        public ReadOnlyCollection<Card> GetGrimoireCards()
        {
            return grimoire.GetAllCards();
        }
    }
}
