# MTG Mana Curve Simulation for Affinity

This is an attempt to do an almost real-world Magic: The Gathering simulation but
for the deck type "Affinity" or "Metalcraft". The reason is that such thing do not
exists yet, and Affinity decks are the ones with more complex curve mana ever.
So this is a Monte Carlo Simulation for Affinity Deck, that simulates almost a real
game.

The approach is an evolution of what Guilherme Braga did with Python, but this is
done in C# and **does a lot more** (really a lot more)

The game let you specify a deck among existing cards, and then it **run a Simulation**
**where there is no foe**. Let's it. Basically it is just playing alone **but following**
**the real Magic: The Gathering rules.**

Let me do an example. You Draw

 - Vault of Whispers
 - Sol Ring
 - Frogmite
 
This simulation plays at first turn in this order: Vault of Whispers, Sol Ring and
Frogmite.

It basically follows a **Greedy strategy** where it prioritize certain cards and
check if it can play the cards, and then play the cards, doing all the side effects.

When playing lands, if you need blue mana tries to prioritze blue lands etc.

In example if you draw a

 - Urza's Saga
 
Before playing it, the algorithm check if there is enough mana for it. That means
there are already 2 mana in game (Sol ring countin etc.) or 1 mana in play and
1 mana that is going to be played (in hand). Then each turn Lore counter are added
to Urza's Saga, and the effects take place, it can be used as land the first turn
the second turn creates a Construct token, and the third turn the same before being
sacrificed. In this way the Urza's Saga will be played when it is possible
to create 2 construct tokens. Which is the optimal play for it. 
Before sacrificing Urza saga another artifact is searched (Sol Ring, Springleaf Drum
or Signal Pest).

The approach I followed, is the simplest possible:
 
 - No foe
 - Mostly greedy, with some planning (for Atog and Steel Overseer)
 - Just some of the cards for affinity are implemented
 - I tried to avoid over-engineering the code 
 
But despite that, the resulting code is incredibly complex


## The first results

Running the simulation several times yielded this highly optimized deck that if not
blocked would wind at turn 5 99,9% of times. Of course the simulation cannot keep
into account "controlling" effects. But this is the starting mana curve base.
**The average winning turn is 5,07.**

**Creatures (25)**:

 - 4    x Ornithopter
 - 4    x Memnite
 - 4    x Signal Pest
 - 4    x Frogmite
 - 2    x Steel Overseer
 - 2    x Thought Monitor
 - 4    x Master of Etherium
 - 1    x Sojourner's Companion

**Artifacts (7)**:

 - 4    x Cranial Plating
 - 2    x Springleaf Drum
 - 1    x Sol Ring

**Lands (28)**:

 - 4    x Urza's Saga
 - 1    x Great Furnace
 - 4    x Vault of Whispers
 - 3    x Mistvault Bridge
 - 4    x Seat of the Synod
 - 4    x Darksteel Citadel
 - 4    x Glimmervoid
 - 4    x Island
 
 ## The improved results (Casual deck)
 
 After tayloring the deck I taylored the strategy (mostly improving the mulligan), 
 after that I re-runned the deck optimization and I found this one, slightly different
 
 **Creatures (25)**:
 
 - 4    x Ornithopter
 - 4    x Memnite
 - 1    x Thought Monitor
 - 4    x Master of Etherium
 - 4    x Frogmite
 - 4    x Signal Pest
 - 4    x Steel Overseer
 
 **Artifacts (9)**:
 
 - 4    x Cranial Plating
 - 4    x Springleaf Drum
 - 1    x Sol Ring
 
 **Lands (26)**:
 
 - 4    x Urza's Saga
 - 1    x Great Furnace
 - 4    x Vault of Whispers
 - 4    x Seat of the Synod
 - 4    x Darksteel Citadel
 - 4    x Glimmervoid
 - 5    x Island
 
## The Vintage Prototype

I know there should be added also Moxes but the optimizer improved the deck further by adding
Tolarian Academy and Ancient tomb. There was some debate whethever the Ancient Tomb should
be added to affinity. The simulation removes all the doubts: Ancient tomb makes an improvement
but by a very small margin. Also note that this deck was tested with
10 millions simulations for each Card Swap (so it took 30 hours to be generated)

**Creatures (28)**

 - 4    x Ornithopter
 - 4    x Memnite
 - 1    x Thought Monitor
 - 3    x Sojourner's Companion
 - 4    x Master of Etherium
 - 4    x Frogmite
 - 4    x Signal Pest
 - 4    x Steel Overseer

**Artifacts (10)**

 - 4    x Cranial Plating
 - 4    x Springleaf Drum
 - 1    x Sol Ring
 - 1    x Lotus Petal

 **Lands (22)**

 - 4    x Urza's Saga
 - 4    x Vault of Whispers
 - 4    x Seat of the Synod
 - 4    x Darksteel Citadel
 - 1    x Island
 - 1    x Tolarian Academy
 - 4    x Ancient Tomb

 **This deck is better by a 1.2% margin over the last version With an AWT of 4,839**
 
 ## The sideboard consideration (for Casual deck)
 
 Swapping out cards for sideboard, makes the deck worse. However we need the Sideboard
 so I tried to figure out how many cards we can swap out. Let's it: I runned the deck
 optimization "reversed". It swaps in place holder cards and remove the cards that makes
 the deck worse (but by the minimum amount possible). So here's a list of cards that can
 be removed with minimal impact. NOTE: you must follow strictly this order of removal:
 
 Swapping cards for an artifact (Pithing needle, welding jar, Tormod's crypt etc.)
 
 - 1) Remove: Glimmervoid 		(AWT: 4,948566666666666)
 - 2) Remove: Thought Monitor 	(AWT: 4,9844)
 - 3) Remove: Island 			(AWT: 5,0307)
 - 4) Remove: Steel Overseer 	(AWT: 5,076733333333333)
 - 5) Remove: Glimmervoid 		(AWT: 5,134333333333333)
 - 6) Remove: Steel Overseer	(AWT: 5,186233333333333)
 - 7) Remove: Frogmite			(AWT: 5,256633333333333)
 - 8) Remove: Glimmervoid		(AWT: 5,324666666666666)
 - 9) Remove: Ornithopter		(AWT: 5,3957)
 - 10)Remove: Frogmite			(AWT: 5,496833333333333)
 
 Swapping cards for a non-Artifact
 
 - 1) Remove: Glimmervoid (AWT: 4,9482333333333335)
 - 2) Remove: Steel Overseer (AWT: 4,993766666666667)
 - 3) Remove: Glimmervoid (AWT: 5,0376)
 - 4) Remove: Frogmite  (AWT: 5,0998)
 - 5) Remove: Frogmite  (AWT: 5,1700333333333335)
 - 6) Remove: Thought Monitor  (AWT: 5,239533333333333)
 - 7) Remove: Vault of Whispers  (AWT: 5,311366666666666)
 - 8) Remove: Springleaf Drum  (AWT: 5,3952333333333335)
 - 9) Remove: Steel Overseer  (AWT: 5,4757)
 - 10)Remove: Signal Pest  (AWT: 5,5757666666666665)
 
 You should note that each card you remove, makes the deck worse (higher AWT),
 and the worsening becomes worse for each consecutive card removed. 
 Also you note that the cards to be removed are different depending on the
 type of incoming cards. However there are common cards across the 2 lists:
 
 - Glimmervoid
 - Thought Monitor 
 - Steel Overseer
 - Glimmervoid
 - Frogmite
 
 
 I did also a Excell chart to visualize the worsening of AWT by increasing
 amount of sideboard added cards.
 
 [<img src="https://github.com/Darelbi/MTG-Mana-Curve-Simulation/blob/main/docs/sideboardAWT.png?raw=true">](https://github.com/Darelbi/MTG-Mana-Curve-Simulation/blob/main/docs/sideboardAWT.png)
 
 
 ## Other findings
 
  - The simulation shows that having the Lotus Petal is almost the same as not having
 it. The simulation shows the difference of AWT (Average Winning Turn) with lotus petal
 is **slightly** worse (~0,01).

 - The simulation also shows that Thoughtcast, and Atog, are useless

 - Unearth is unnecessary if you use welding jar.
 
 - Mistvault Bridge isn't that bad, it was included in the first version of the deck,
 only removed in the second version


## Running the Simulation

The code is c# and is pretty fast doing 1 simulation of 1 game, however to keep code
as simple as possible i avoided unecessary optimizations. So LINQ is all over the 
place querying for card properties. In example I do not keep an artifact counter
when counting Artifacts for Cranial Plating, instead I query all the cards in game
everytime someone asks for the creature power.

Also when the `DefaultStrategy` query the game status, a duplicate of the cards 
is returned (to avoid accidentaly change of game status, since I'm doing speedy
coding and in some place infact I modify the duplicate directly for convenience)

```
	public class ArtifactAddedPower : IEquippedPowerFeat
	{
		public int GetEquippedPower(Card source, IGameInteraction interaction)
		{
			return interaction.GetPlayCards().Count(x => x.Artifact);
		}
	}
```

The simulation basically starts from a deck, draws 7 random cards, do the mulligan
if certain conditions are not met, and then starts playing card in a realistic order
apparently following a good strategy.

Then the simulation is repeated 50000 times and statistics are collected.
Now, there is no agreement to which statistics makes a deck better, one idea though
is to count the average number of turns the decks takes to do 30 damage (yes it 
simulate combat phase, keeping into account equipment power, x/x creatures etc).
among 50000 games. I know the foe has 20 life, but we have to simulate in some 
way there are defending creatures and this is a really reasonable way. 

So we take as winning condition doing 30 damage and then we get statistics like

 - Games won at 1 turn: 0/50000
 - Games won at 2 turn: 0/50000
 - Games won at 3 turn: 0/50000
 - Games won at 4 turn: 4/50000
 
 Average winning turn 6.53255
 
But not only that, we may want to compare 2 decks that win on average at the same
turn, but compare their mana and pick the deck that spends more mana because
that means more powerfull cards played (maybe cards which effects depends on what
a foe does, a thing that this kind of simulation can't keep into account).

 - Average mana spent at turn 1: 2.333
 - Average mana spent at turn 2: 3.444
 
and so on.

## Taylor your deck

So what is this simulation good at? Computing mana curves and veryfing hypotesis.
In example, I want to verify if "Island" is better than "Mistvault Bridge".
I try the 2 decks one with 4 islands and one with 4 bridges, and I run the 
simulation and I see the one with better average winning turn. You may want
to check also all the combinations in between (in example 1 island and 3 bridges)

Another hypotesis to check is if Lotus Petal is worth adding (there are contrasting
opinions on the web, the simulation will find an answer easily).

You could also implement easily Placeholder cards that do nothign but have 
a manacost, the game will play those if it does not find any better cards.
this is to check the mana curve after N turns. It may be tremendously usefull.

Also we can make a hypotesis of "Continuity" in the cards domain. In reality
I do not think the hypotesis holds, but may help you to find a "local maximum"

That means go with a starting Deck, then try replace 1 card with another one
card from a try list. Do that for all the cards in the deck and for all the cards
in the try list, and keeps the deck with the better average winning turn try.

Then repeat the process until there is no more improvement.
This kind of stuff would require probably Hours to run

## Taylor the Strategy

Once you found the optimal deck, you can play around with StrategyVariables and see
how the strategy plays (with that deck). The strategy variables should also gives
hints at which behavior statistically pay you better during a real match. However 
notes that a strategy works better with a deck, so by Tayloring your strategy 
it is possible there is a better deck for that strategy, that means repeat The
operation:

TaylorDeck => Taylor strategy => Taylor Deck... etc.


## What the simulation does not

The simulation is without an opponent, so it cannot quantify the implicit value of
having cards that apply some kind of effects on enemy creatures or some kind of 
control, like milling or like Tormod's Crypt. Luckily the affinity deck is the kind
of deck that want to play "power" and the simulation is especially good at that.

There is no way the simulation will say is good having 4 Tormod's Crypt against a
Graveyard deck even if it is an obvious benefit. 

however you can put some place holders cards that do nothing, and quantify how
much power do you lose for having extra utility cards in the deck.

That's it, if you want to play Power mode, but have also utility cards, the Simulation
will tell you how much do you lose in Power thanks to utility cards.

Which may be a great advantage anyway.

A remark, I created the code around the affinity deck. So if you want to simulate
another kind of deck (i.e. a Green Aggro). You need to implement the missing cards
AND implement a brand new `GreenAggroStrategy` class implementing `IStrategy`.
Good Luck!

## what is not implemented

The deck was designed around blue/black/red affinity prototype, so it is missing
green and white mana implementation. (for now)

The deck is missing a real mana allocation strategy, which may matters for multi
colored decks (actually the mana is spent in a greedy way, colored mana first).
It is not a simple task doing a mana allocation strategy.

## contributing

You can contribute in any way you desider and I try to keep all usefull contributions

I.E. You could implement a card (or also just request me to implement that)

Refactorin is wellcome: I'm aware the code is not a good example of software engineering
but I developed it in hurry, I left a lot of "TODO" in the code that are good starting
points. But the improvements are not limited to TODOs.

AS LONG AS UNIT TESTS PASSES.

The code is heavily tested, I would never accept any change that breaks any unit tests.

## unit tests

Unit tests check that given some cards, those are played in a determined order, which
basically verify the Strategy of card selection is working.

The unit tests are important because says the code is working, there is no way you can
check manually the outcome of 50000 games.

Some tests verify the working of side effects by checking the total damage done or
some cards are into play (i.e. Construct tokens from Urza's Saga)