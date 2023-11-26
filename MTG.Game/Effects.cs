using MTG.Cards;
using MTG.Cards.Cards.Effects;

namespace MTG.Game
{
    // TODO: there are at least 1 million ways to organize better the code.
    public class Effects
    {
        private readonly List<IEffect> EffectsStack; //TODO: use dictionary

        private readonly List<IEffect> UntilEndOfTurnEffectsStack; //TODO: use dictionary

        public Effects()
        {
            EffectsStack = new List<IEffect>();
            UntilEndOfTurnEffectsStack = new List<IEffect>();
        }

        public List<IEffect> AllEffects()
        {
            return EffectsStack.ToList().Concat(UntilEndOfTurnEffectsStack).ToList();
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
            var powerEffects = AllEffects()
                .Where(x => x.GetType().IsAssignableTo(typeof(IPowerIncreaseEffect)))
                .Select( x => (IPowerIncreaseEffect) x)
                .Where( x => x.BenefitsOfPower(x.Owner, interaction, card)).ToList();

            if(!powerEffects.Any() ) { return 0; }

            return powerEffects.Select(x => x.GetPower(x.Owner, interaction, card)).Sum();
        }

        public int GetToughnessIncreaseForCard(Card card, IGameInteraction interaction)
        {
            var toughnessEffects = AllEffects()
                .Where(x => x.GetType().IsAssignableTo(typeof(IPowerIncreaseEffect)))
                .Select(x => (IToughnessIncreaseEffect)x)
                .Where(x => x.BenefitsOfToughness(x.Owner, interaction, card)).ToList();

            if (!toughnessEffects.Any()) { return 0; }

            return toughnessEffects.Select(x => x.GetToughness(x.Owner, interaction, card)).Sum();
        }

        public void AddAttackPhaseEffects(List<IEffect> attacckEffects)
        {
            EffectsStack.AddRange(attacckEffects);
        }

        public void AddUntilEndOfTurnEffects(IEffect effect)
        {
            UntilEndOfTurnEffectsStack.Add(effect);
        }

        public void RemoveAttackPhaseEffects(List<IEffect> attacckEffects)
        {
            foreach(var effect in attacckEffects)
                EffectsStack.Remove(effect); 
        }

        public void RemoveUntilEndOfTurnEffects()
        {
            UntilEndOfTurnEffectsStack.Clear();
        }
    }
}
