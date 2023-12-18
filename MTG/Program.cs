using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game;

var deck = new Deck();
var trylits = new Deck();

// OLD VERSION OF POWER DECK

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
deck.AddCards(3, () => new SojournersCompanion());
deck.AddCards(4, () => new VaultOfWhispers());
deck.AddCards(4, () => new SeatOfTheSynod());
deck.AddCards(4, () => new DarksteelCitadel());
deck.AddCards(1, () => new Island());
deck.AddCards(1, () => new TolarianAcademy());
deck.AddCards(4, () => new AncientTomb());
deck.AddCards(1, () => new LotusPetal());

// NEWLY IMPLEMENTED CARDS

trylits.AddCards(3, () => new ThoughtMonitor());
trylits.AddCards(1, () => new SojournersCompanion());
trylits.AddCards(4, () => new Thoughtcast());
trylits.AddCards(1, () => new ManaVault());
trylits.AddCards(1, () => new ManaCrypt());


DeckOptimizer optimizer = new(deck, trylits, 300000);
optimizer.Run();

/// FIND SIDEBOARD FOR MY DECK
/// 

var sideboard = new Deck();

sideboard.AddCards(8, () => new ArtifactPlaceholder1());

var sideboardfinder = new DeckSideboardFinder(deck, sideboard, 300000);
sideboardfinder.Run();

