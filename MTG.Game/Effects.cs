using MTG.Cards;
using MTG.Cards.Cards.Effects;

namespace MTG.Game
{
    // TODO: there are at least 1 million ways to organize better the code.
    public class Effects
    {
        private readonly List<IEffect> EffectsStack; //TODO: use dictionary

        public Effects()
        {
            EffectsStack = new List<IEffect>();
        }

        public void CardEnterPlay(Card card)
        {
            foreach(var effect in card.Effects)
            {
                EffectsStack.Add(effect);
            }
        }

        public void CardExitPlay(Card card)
        {
            foreach (var effect in card.Effects)
            {
                EffectsStack.Remove(effect);
            }
        }

        public int GetPowerIncreaseForCard(Card card, IGameInteraction interaction)
        {
            var powerEffects = EffectsStack
                .Where(x => x.GetType().IsAssignableTo(typeof(IPowerIncreaseEffect)))
                .Select( x => (IPowerIncreaseEffect) x)
                .Where( x => x.BenefitsOfPower(x.Owner, interaction, card)).ToList();

            if(!powerEffects.Any() ) { return 0; }

            return powerEffects.Select(x => x.GetPower(x.Owner, interaction, card)).Sum();
        }

        public int GetToughnessIncreaseForCard(Card card, IGameInteraction interaction)
        {
            var toughnessEffects = EffectsStack
                .Where(x => x.GetType().IsAssignableTo(typeof(IPowerIncreaseEffect)))
                .Select(x => (IToughnessIncreaseEffect)x)
                .Where(x => x.BenefitsOfToughness(x.Owner, interaction, card)).ToList();

            if (!toughnessEffects.Any()) { return 0; }

            return toughnessEffects.Select(x => x.GetToughness(x.Owner, interaction, card)).Sum();
        }

        internal void AddTemporaryEffects(List<IEffect> attacckEffects)
        {
            EffectsStack.AddRange(attacckEffects);
        }

        internal void RemoveTemporaryEffects(List<IEffect> attacckEffects)
        {
            foreach(var effect in attacckEffects)
                EffectsStack.Remove(effect); 
        }
    }
}
