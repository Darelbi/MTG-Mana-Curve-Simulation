using MTG.Cards.Cards.Feats;
using MTG.Cards.Cards.Lands.Abilities;
using MTG.Mana;

namespace MTG.Cards.Cards.Lands.Feats
{
    public class UrzasSagaLoreCounting : IOnEnterGameFeat, IOnBeginPhaseFeat
    {
        public void OnBeginPhase(Card source, IGameInteraction interaction)
        {
            source.LoreCounter++;
            TransformAccordinToLore(source, interaction);
        }

        public void OnEnterGame(Card source, IGameInteraction interaction)
        {
            source.LoreCounter = 1;
            TransformAccordinToLore(source, interaction);
        }

        private void TransformAccordinToLore(Card source, IGameInteraction interaction)
        {
            switch (source.LoreCounter)
            {
                case 1:
                    source.ManaSource = new Mana.Mana
                    {
                        Colorless = 1
                    };
                    break;

                case 2:
                    source.Abilities.Add(new UrzasSagaAbility(source));
                    break;

                case 3:
                    interaction.FindCardToPlayFromDeck(source,
                        x=> x.ManaCost != null 
                        //seek card with cost exactly 1 or 0
                        && x.Artifact
                        && x.ManaCost.ConvertedManaValue()<=1
                        && x.ManaCost.TotalColorlessValue() == x.ManaCost.ConvertedManaValue());
                    interaction.Sacrifice(source);
                    break;
            }
        }
    }
}
