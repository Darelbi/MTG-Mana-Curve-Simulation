This is the first attempt at a optimization of deck

Starting Optimization: with initial deck

4       x Ornithopter
4       x Memnite
4       x Myr Enforcer
4       x Master of Etherium
4       x Frogmite
4       x Cranial Plating
4       x Springleaf Drum
4       x Urza's Saga
1       x Sol Ring
3       x Thought Monitor
4       x Sojourner's Companion
4       x Vault of Whispers
4       x Mistvault Bridge
4       x Seat of the Synod
4       x Darksteel Citadel
4       x Glimmervoid

Removed: Myr Enforcer, Added:Thought Monitor
Removed: Sojourner's Companion, Added:Island
Removed: Sojourner's Companion, Added:Island
Removed: Glimmervoid, Added:Island
Removed: Springleaf Drum, Added:Island
Removed: Darksteel Citadel, Added:Island
Removed: Glimmervoid, Added:Thoughtcast
Removed: Thoughtcast, Added:Island

Ended Optimization: with final deck

4       x Ornithopter
4       x Memnite
3       x Myr Enforcer
4       x Master of Etherium
4       x Frogmite
4       x Cranial Plating
3       x Springleaf Drum
4       x Urza's Saga
1       x Sol Ring
4       x Thought Monitor
2       x Sojourner's Companion
4       x Vault of Whispers
4       x Mistvault Bridge
4       x Seat of the Synod
3       x Darksteel Citadel
2       x Glimmervoid
6       x Island

**On the best of 10.000 games for each possible swap combination
between the try list and the starting deck**

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