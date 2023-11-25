This is the first optimization attempt, the AWT metrics stands for 
Average Winning Turn, which means most games are won in 5 turns,
some in 6:

Starting Optimization: with initial deck

 - 4       x Ornithopter
 - 4       x Memnite
 - 4       x Myr Enforcer
 - 4       x Master of Etherium
 - 4       x Frogmite
 - 4       x Cranial Plating
 - 4       x Springleaf Drum
 - 4       x Urza's Saga
 - 1       x Sol Ring
 - 3       x Thought Monitor
 - 4       x Sojourner's Companion
 - 4       x Vault of Whispers
 - 4       x Mistvault Bridge
 - 4       x Seat of the Synod
 - 4       x Darksteel Citadel
 - 4       x Glimmervoid

 - Removed: Myr Enforcer, Added:Island (AWT: 5,241466666666667)
 - Removed: Myr Enforcer, Added:Island (AWT: 5,211533333333334)
 - Removed: Sojourner's Companion, Added:Island (AWT: 5,204733333333333)
 - Removed: Myr Enforcer, Added:Island (AWT: 5,192966666666667)

Ended Optimization: with final deck

 - 4       x Ornithopter
 - 4       x Memnite
 - 1       x Myr Enforcer
 - 4       x Master of Etherium
 - 4       x Frogmite
 - 4       x Cranial Plating
 - 4       x Springleaf Drum
 - 4       x Urza's Saga
 - 1       x Sol Ring
 - 3       x Thought Monitor
 - 3       x Sojourner's Companion
 - 4       x Vault of Whispers
 - 4       x Mistvault Bridge
 - 4       x Seat of the Synod
 - 4       x Darksteel Citadel
 - 4       x Glimmervoid
 - 4       x Island

**Out of of 10.000 games for each possible swap combination, if a match is found**
**Another simulation of 30.000 games is done**

```
var starting = new Deck();
var tryList = new Deck();


starting.AddCards(4, () => new Ornithopter());
starting.AddCards(4, () => new Memnite());
starting.AddCards(4, () => new MyrEnforcer());
starting.AddCards(4, () => new MasterOfEtherium());
starting.AddCards(4, () => new Frogmite());

starting.AddCards(4, () => new CranialPlating());
starting.AddCards(4, () => new SpringleafDrum());
starting.AddCards(4, () => new UrzasSaga());
starting.AddCards(1, () => new SolRing());
starting.AddCards(3, () => new ThoughtMonitor());
starting.AddCards(4, () => new SojournersCompanion());

starting.AddCards(4, () => new VaultOfWhispers());
starting.AddCards(4, () => new MistvaultBridge());
starting.AddCards(4, () => new SeatOfTheSynod());
starting.AddCards(4, () => new DarksteelCitadel());
starting.AddCards(4, () => new Glimmervoid());

tryList.AddCards(8, () => new Island());
tryList.AddCards(1, () => new ThoughtMonitor());
tryList.AddCards(4, () => new Thoughtcast());


DeckOptimizer optimizer = new DeckOptimizer(starting, tryList);
optimizer.Run();
```

On the given deck, the thoughtcast gave no advantage whatsoever, and the only
change was a optimization in the mana curve by removing 4 myr enforcers and
adding 4 islands. The simulation makes sense overall, but probably since
Myr enforcer relies on affinity the simulation would end up with a different
result if I add more 0 artifacts, like welding jar.

The result is promising, but I need more implemented cards and a more advanced
playing strategy to check the results.

Mistvault bridge seems to play a role since it was not removed.

**A second simulation shows that Lotus Petal is worth adding:**


 - Removed: Thought Monitor, Added:Lotus Petal (AWT: 5,2338)
 - Removed: Springleaf Drum, Added:Island (AWT: 5,200833333333334)
 - Removed: Myr Enforcer, Added:Island (AWT: 5,1789)
 - Removed: Thought Monitor, Added:Island (AWT: 5,1654333333333335)
 - Removed: Myr Enforcer, Added:Island (AWT: 5,1627)

Ended Optimization: with final deck

 - 4       x Ornithopter
 - 4       x Memnite
 - 2       x Myr Enforcer
 - 4       x Master of Etherium
 - 4       x Frogmite
 - 4       x Cranial Plating
 - 3       x Springleaf Drum
 - 4       x Urza's Saga
 - 1       x Sol Ring
 - 1       x Thought Monitor
 - 4       x Sojourner's Companion
 - 4       x Vault of Whispers
 - 4       x Mistvault Bridge
 - 4       x Seat of the Synod
 - 4       x Darksteel Citadel
 - 4       x Glimmervoid
 - 1       x Lotus Petal
 - 4       x Island

In general this time out of 10 optimization attempts only
5 cards was changed. It is showed therefore also that lotus petal
is worth adding even in Affinity deck.


**Both the first attempt and the second attempt have the sum of**
 - Sojorouners' companion
 - Thought monitor
 - Myr Enforcer

**cards equal to 7.**
 I would run a simulation to try swapping these
and see if there are any differences. Probably the mana curve
is optimal with
 
 - 4  x Thought Monitor
 - 3  x Sojourner's Companion
