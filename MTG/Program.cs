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


DeckOptimizer optimizer = new(deck, trylits);
optimizer.Run();