using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game;

var deck = new Deck();
var trylits = new Deck();

deck.AddCards(4, () => new Ornithopter());
deck.AddCards(4, () => new Memnite());
deck.AddCards(1, () => new ThoughtMonitor());
deck.AddCards(4, () => new MasterOfEtherium());
deck.AddCards(4, () => new Frogmite());
deck.AddCards(4, () => new SignalPest());
deck.AddCards(4, () => new SteelOverseer());

deck.AddCards(4, () => new CranialPlating());
deck.AddCards(4, () => new SpringleafDrum());
deck.AddCards(1, () => new SolRing());

deck.AddCards(4, () => new UrzasSaga());
deck.AddCards(1, () => new GreatFurnace());
deck.AddCards(4, () => new VaultOfWhispers());
deck.AddCards(4, () => new SeatOfTheSynod());
deck.AddCards(4, () => new DarksteelCitadel());
deck.AddCards(4, () => new Glimmervoid());
deck.AddCards(5, () => new Island());

trylits.AddCards(3, () => new ThoughtMonitor());
trylits.AddCards(4, () => new Thoughtcast());
trylits.AddCards(1, () => new LotusPetal());
trylits.AddCards(4, () => new SojournersCompanion());
trylits.AddCards(4, () => new AncientTomb());


DeckOptimizer optimizer = new(deck, trylits, 60000);
optimizer.Run();

//var deck2 = new Deck();
//var deck3 = new Deck();

//deck2.AddCards(4, () => new Ornithopter());
//deck2.AddCards(4, () => new Memnite());
//deck2.AddCards(4, () => new ThoughtMonitor());
//deck2.AddCards(4, () => new MasterOfEtherium());
//deck2.AddCards(4, () => new Frogmite());
//deck2.AddCards(4, () => new SignalPest());
//deck2.AddCards(3, () => new SteelOverseer());

//deck2.AddCards(4, () => new CranialPlating());
//deck2.AddCards(2, () => new SpringleafDrum());
//deck2.AddCards(1, () => new SolRing());

//deck2.AddCards(4, () => new UrzasSaga());
//deck2.AddCards(1, () => new GreatFurnace());
//deck2.AddCards(4, () => new VaultOfWhispers());
//deck2.AddCards(4, () => new SeatOfTheSynod());
//deck2.AddCards(4, () => new DarksteelCitadel());
//deck2.AddCards(4, () => new Glimmervoid());
//deck2.AddCards(5, () => new Island());



//deck3.AddCards(4, () => new Ornithopter());
//deck3.AddCards(4, () => new Memnite());
//deck3.AddCards(4, () => new ThoughtMonitor());
//deck3.AddCards(4, () => new MasterOfEtherium());
//deck3.AddCards(3, () => new Frogmite());
//deck3.AddCards(4, () => new SignalPest());
//deck3.AddCards(4, () => new SteelOverseer());

//deck3.AddCards(4, () => new CranialPlating());
//deck3.AddCards(2, () => new SpringleafDrum());
//deck3.AddCards(1, () => new SolRing());

//deck3.AddCards(4, () => new UrzasSaga());
//deck3.AddCards(1, () => new GreatFurnace());
//deck3.AddCards(3, () => new VaultOfWhispers());
//deck3.AddCards(4, () => new SeatOfTheSynod());
//deck3.AddCards(4, () => new DarksteelCitadel());
//deck3.AddCards(3, () => new Glimmervoid());
//deck3.AddCards(5, () => new Island());
//deck3.AddCards(1, () => new LotusPetal());
//deck3.AddCards(1, () => new AncientTomb());


//var comparison2 = new CompareTwoDecks(
//    deck2
//    , deck3, () => StrategyVariables.OldMulligan = false, () => StrategyVariables.OldMulligan = false);