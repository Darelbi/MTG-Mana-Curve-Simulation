using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game;

var starting = new Deck();
var tryList = new Deck();


starting.AddCards(4, () => new Ornithopter());
starting.AddCards(4, () => new Memnite());
starting.AddCards(2, () => new ThoughtMonitor());
starting.AddCards(4, () => new MasterOfEtherium());
starting.AddCards(4, () => new Frogmite());

starting.AddCards(4, () => new CranialPlating());
starting.AddCards(2, () => new SpringleafDrum());
starting.AddCards(4, () => new UrzasSaga());
starting.AddCards(1, () => new SolRing());
starting.AddCards(1, () => new LotusPetal());

starting.AddCards(4, () => new VaultOfWhispers());
starting.AddCards(4, () => new MistvaultBridge());
starting.AddCards(4, () => new SeatOfTheSynod());
starting.AddCards(4, () => new GreatFurnace());
starting.AddCards(4, () => new Glimmervoid());
starting.AddCards(4, () => new Island()); 
starting.AddCards(4, () => new SignalPest());
starting.AddCards(2, () => new SteelOverseer());


tryList.AddCards(3, () => new SojournersCompanion());
tryList.AddCards(3, () => new ThoughtMonitor());
tryList.AddCards(4, () => new Thoughtcast());
tryList.AddCards(4, () => new Island());
tryList.AddCards(4, () => new Mountain());
tryList.AddCards(4, () => new Atog());
tryList.AddCards(2, () => new SteelOverseer());
tryList.AddCards(2, () => new SpringleafDrum());
tryList.AddCards(4, () => new DarksteelCitadel());



//DeckOptimizer optimizer = new DeckOptimizer(starting, tryList);
//optimizer.Run();

/////////////// DECKS COMPARISON

var firstDeck = new Deck();
var secondDeck = new Deck();


firstDeck.AddCards(4, () => new Ornithopter());//
firstDeck.AddCards(4, () => new Memnite());//
firstDeck.AddCards(1, () => new MyrEnforcer()); //
firstDeck.AddCards(4, () => new MasterOfEtherium());//
firstDeck.AddCards(4, () => new Frogmite());//

firstDeck.AddCards(4, () => new CranialPlating());//
firstDeck.AddCards(2, () => new SpringleafDrum());//
firstDeck.AddCards(4, () => new UrzasSaga());//
firstDeck.AddCards(1, () => new SolRing());//
firstDeck.AddCards(1, () => new LotusPetal());//
firstDeck.AddCards(1, () => new SojournersCompanion());//

firstDeck.AddCards(4, () => new VaultOfWhispers());//
firstDeck.AddCards(4, () => new MistvaultBridge());//
firstDeck.AddCards(4, () => new SeatOfTheSynod());//
firstDeck.AddCards(4, () => new DarksteelCitadel());//
firstDeck.AddCards(4, () => new Glimmervoid());//
firstDeck.AddCards(4, () => new Island());//
firstDeck.AddCards(4, () => new SignalPest());////
firstDeck.AddCards(2, () => new SteelOverseer());



secondDeck.AddCards(4, () => new Ornithopter());//
secondDeck.AddCards(4, () => new Memnite());//
secondDeck.AddCards(2, () => new ThoughtMonitor());//
secondDeck.AddCards(4, () => new MasterOfEtherium());//
secondDeck.AddCards(4, () => new Frogmite());//
secondDeck.AddCards(1, () => new SojournersCompanion());//

secondDeck.AddCards(4, () => new CranialPlating());//
secondDeck.AddCards(2, () => new SpringleafDrum());//
secondDeck.AddCards(4, () => new UrzasSaga());//
secondDeck.AddCards(1, () => new SolRing());

secondDeck.AddCards(1, () => new GreatFurnace());//
secondDeck.AddCards(4, () => new VaultOfWhispers());//
secondDeck.AddCards(3, () => new MistvaultBridge());//
secondDeck.AddCards(4, () => new SeatOfTheSynod());//
secondDeck.AddCards(4, () => new DarksteelCitadel());//
secondDeck.AddCards(4, () => new Glimmervoid());////
secondDeck.AddCards(4, () => new Island());//
secondDeck.AddCards(4, () => new SignalPest());//
secondDeck.AddCards(2, () => new SteelOverseer());//

var comparison = new CompareTwoDecks(firstDeck
    , secondDeck);