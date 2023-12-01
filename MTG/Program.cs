using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game;
using MTG.Game.Strategies;

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

//var comparison = new CompareTwoDecks(
//    firstDeck
//    , secondDeck, () => StrategyVariables.OldMulligan = true, () => StrategyVariables.OldMulligan = true);


////////// TAYLOR THE STRATEGY
///
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
//var comparison2 = new CompareTwoDecks(
//    thirdDeck
//    , thirdDeck, () => StrategyVariables.OldMulligan = true, () => StrategyVariables.OldMulligan = false);

// now retaylor the deck with the new strategy

StrategyVariables.OldMulligan = false;

var tryList2 = new Deck();

tryList2.AddCards(2, () => new SteelOverseer());
tryList2.AddCards(2, () => new ThoughtMonitor());
tryList2.AddCards(4, () => new Thoughtcast());
tryList2.AddCards(1, () => new LotusPetal());
tryList2.AddCards(1, () => new MistvaultBridge());
tryList2.AddCards(2, () => new SpringleafDrum());
tryList2.AddCards(3, () => new SojournersCompanion());
tryList2.AddCards(2, () => new Island());

//DeckOptimizer optimizer2 = new(thirdDeck, tryList2);
//optimizer2.Run();


var fourthDeck = new Deck();

fourthDeck.AddCards(4, () => new Ornithopter());//
fourthDeck.AddCards(4, () => new Memnite());//
fourthDeck.AddCards(1, () => new ThoughtMonitor());//
fourthDeck.AddCards(4, () => new MasterOfEtherium());//
fourthDeck.AddCards(4, () => new Frogmite());//
fourthDeck.AddCards(4, () => new SignalPest());//
fourthDeck.AddCards(4, () => new SteelOverseer());//

fourthDeck.AddCards(4, () => new CranialPlating());//
fourthDeck.AddCards(4, () => new SpringleafDrum());//
fourthDeck.AddCards(1, () => new SolRing());

fourthDeck.AddCards(4, () => new UrzasSaga());//
fourthDeck.AddCards(1, () => new GreatFurnace());//
fourthDeck.AddCards(4, () => new VaultOfWhispers());//
fourthDeck.AddCards(4, () => new SeatOfTheSynod());//
fourthDeck.AddCards(4, () => new DarksteelCitadel());//
fourthDeck.AddCards(4, () => new Glimmervoid());////
fourthDeck.AddCards(5, () => new Island());//

var sideBoardFind = new Deck();
sideBoardFind.AddCards(10, () => new Placeholder1());

DeckSideboardFinder deckSideboardFinder = new(fourthDeck, sideBoardFind);
deckSideboardFinder.Run();