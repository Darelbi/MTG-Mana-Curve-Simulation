# MTG Mana Curve Simulation for Affinity

This is an attempt to do an almost real-world Magic: The Gathering simulation but
for the deck type "Affinity" or "Metalcraft". The reason is that such thing do not
exists yet, and Affinity decks are the ones with more complex curve mana ever.

The approach is an evolution of what Guilherme Braga did with Python, but this is
done in C#. 

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
the second turn creates a Construct token, and the third turn idem before being
sacrificed.

The approach I followed, is the simplest possible:
 
 - No foe
 - Greedy strategy VS real planning
 - Just some of the cards for affinity are implemented
 - I tried to avoid over-engineering the code 
 
But despite that, the resulting code is incredibly complex


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
simulate combat phase, keeping into account equipment power, */* creatures etc).
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


## What the simulation does not (yet)