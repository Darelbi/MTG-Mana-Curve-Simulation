By comparing 2 different mulligan strategies I found the latter is better than more
than 0.1 AWT. Totalling a 4.985 AWT Score (below Average Winning Turn 5)

```
var thirdDeck = new Deck();

thirdDeck.AddCards(4, () => new Ornithopter());//
thirdDeck.AddCards(4, () => new Memnite());//
thirdDeck.AddCards(2, () => new ThoughtMonitor());//
thirdDeck.AddCards(4, () => new MasterOfEtherium());//
thirdDeck.AddCards(4, () => new Frogmite());//
thirdDeck.AddCards(1, () => new SojournersCompanion());//
thirdDeck.AddCards(4, () => new SignalPest());//
thirdDeck.AddCards(2, () => new SteelOverseer());//

thirdDeck.AddCards(4, () => new CranialPlating());//
thirdDeck.AddCards(2, () => new SpringleafDrum());//
thirdDeck.AddCards(1, () => new SolRing());

thirdDeck.AddCards(4, () => new UrzasSaga());//
thirdDeck.AddCards(1, () => new GreatFurnace());//
thirdDeck.AddCards(4, () => new VaultOfWhispers());//
thirdDeck.AddCards(3, () => new MistvaultBridge());//
thirdDeck.AddCards(4, () => new SeatOfTheSynod());//
thirdDeck.AddCards(4, () => new DarksteelCitadel());//
thirdDeck.AddCards(4, () => new Glimmervoid());////
thirdDeck.AddCards(4, () => new Island());//


// deck2 (strategy 2) better because AWT = 4.985
var comparison2 = new CompareTwoDecks(
    thirdDeck
    , thirdDeck, () => StrategyVariables.OldMulligan = true, () => StrategyVariables.OldMulligan = false);
```

This is the final optimization attemp for the casual affinity deck. The starting 
deck is the following one:

Starting Optimization: with initial deck

 - 4    x Ornithopter
 - 4    x Memnite
 - 2    x Thought Monitor
 - 4    x Master of Etherium
 - 4    x Frogmite
 - 1    x Sojourner's Companion
 - 4    x Signal Pest
 - 2    x Steel Overseer
 - 4    x Cranial Plating
 - 2    x Springleaf Drum
 - 1    x Sol Ring
 - 4    x Urza's Saga
 - 1    x Great Furnace
 - 4    x Vault of Whispers
 - 3    x Mistvault Bridge
 - 4    x Seat of the Synod
 - 4    x Darksteel Citadel
 - 4    x Glimmervoid
 - 4    x Island
 
Total: 60 Total Cards

And then we activate the new mulligan strategy

```
// now retaylor the deck with the new strategy

StrategyVariables.OldMulligan = false;
```

We run again deck optimization against the optimal deck, with a wide trylist
and see which deck is really better.

Basically we taylored the deck first, then we taylored the strategy (just the
mulligan in this case) and then we retaylored the deck.

```

var tryList2 = new Deck();

tryList2.AddCards(2, () => new SteelOverseer());
tryList2.AddCards(2, () => new ThoughtMonitor());
tryList2.AddCards(4, () => new Thoughtcast());
tryList2.AddCards(1, () => new LotusPetal());
tryList2.AddCards(1, () => new MistvaultBridge());
tryList2.AddCards(2, () => new SpringleafDrum());
tryList2.AddCards(3, () => new SojournersCompanion());
tryList2.AddCards(2, () => new Island());

DeckOptimizer optimizer2 = new(thirdDeck, tryList2);
optimizer2.Run();
```

And here's the result:


 - Removed: Thought Monitor, Added:Springleaf Drum (AWT: 4,94864)
 - Removed: Mistvault Bridge, Added:Springleaf Drum (AWT: 4,9234)
 - Removed: Mistvault Bridge, Added:Steel Overseer (AWT: 4,91948)
 - Removed: Mistvault Bridge, Added:Steel Overseer (AWT: 4,91484)
 - Removed: Sojourner's Companion, Added:Island (AWT: 4,9074)

Ended Optimization: with final deck

 - 4    x Ornithopter
 - 4    x Memnite
 - 1    x Thought Monitor
 - 4    x Master of Etherium
 - 4    x Frogmite
 - 4    x Signal Pest
 - 4    x Steel Overseer
 - 4    x Cranial Plating
 - 4    x Springleaf Drum
 - 1    x Sol Ring
 - 4    x Urza's Saga
 - 1    x Great Furnace
 - 4    x Vault of Whispers
 - 4    x Seat of the Synod
 - 4    x Darksteel Citadel
 - 4    x Glimmervoid
 - 5    x Island
Total: 60 Total Cards


We went from an AWT of 5.07 to an AWT of 4.9 an improvement of 3.5%
Not much but noticeable. We improved again an already very good Deck
By improving the strategy we also needed to update the deck so we
got a cascade improvement.