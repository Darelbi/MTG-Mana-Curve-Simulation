using MTG.Cards.Cards.Artifacts;
using MTG.Cards.Cards.Creatures;
using MTG.Cards.Cards.Lands;
using MTG.Cards.Cards.Sorceries;
using MTG.Game;

var starting = new Deck();
var tryList = new Deck();


starting.AddCards(4, () => new Ornithopter());//
starting.AddCards(4, () => new Memnite());//
starting.AddCards(2, () => new ThoughtMonitor());//
starting.AddCards(4, () => new MasterOfEtherium());//
starting.AddCards(4, () => new Frogmite());//

starting.AddCards(4, () => new CranialPlating());//
starting.AddCards(2, () => new SpringleafDrum());//
starting.AddCards(4, () => new UrzasSaga());//
starting.AddCards(1, () => new SolRing());//
starting.AddCards(1, () => new LotusPetal());//

starting.AddCards(4, () => new VaultOfWhispers());//
starting.AddCards(4, () => new MistvaultBridge());//
starting.AddCards(4, () => new SeatOfTheSynod());//
starting.AddCards(4, () => new GreatFurnace());//
starting.AddCards(4, () => new Glimmervoid());//
starting.AddCards(4, () => new Island()); //
starting.AddCards(4, () => new SignalPest());//
starting.AddCards(2, () => new SteelOverseer());//


tryList.AddCards(3, () => new SojournersCompanion());
tryList.AddCards(3, () => new ThoughtMonitor());
tryList.AddCards(4, () => new Thoughtcast());
tryList.AddCards(4, () => new Island());
tryList.AddCards(4, () => new Mountain());
tryList.AddCards(4, () => new Atog());
tryList.AddCards(2, () => new SteelOverseer());
tryList.AddCards(2, () => new SpringleafDrum());
tryList.AddCards(4, () => new DarksteelCitadel());



DeckOptimizer optimizer = new DeckOptimizer(starting, tryList);
optimizer.Run();


//Deck deck = new();

//deck.AddCards(4, () => new Frogmite());
//deck.AddCards(4, () => new Ornithopter());
//deck.AddCards(4, () => new MyrEnforcer());
//deck.AddCards(4, () => new SojournersCompanion());
//deck.AddCards(4, () => new Memnite());

//deck.AddCards(4, () => new CranialPlating());
//deck.AddCards(4, () => new UrzasSaga());

//deck.AddCards(4, () => new MistvaultBridge());
//deck.AddCards(4, () => new SeatOfTheSynod());
//deck.AddCards(4, () => new VaultOfWhispers());
//deck.AddCards(4, () => new SolRing());
//deck.AddCards(4, () => new Island());
//deck.AddCards(4, () => new DarksteelCitadel());
//deck.AddCards(3, () => new Glimmervoid());

//int games = 1;
//for (int i = 0; i < games; i++)
//{
//    Console.Write("\n---GAME START---\n");
//    var grimoire = deck.GetGrimoire( new RandomCardDrawer());
//    var game = new Game(grimoire, new DefaultStrategy(), meStarting: true);
//    game.BeginGame();

//    for(int j=0; j<5; j++)
//    {
//        game.Turn();
//    }

//    Console.Write("\n---GAME END---\n");
//}
