using MTG.Cards.Cards.Feats;
using MTG.Cards.Cards.Lands.Feats;

namespace MTG.Cards.Cards.Lands
{
    //TODO: fill this class
    public class UrzasSaga : Card
    {
        public UrzasSaga()
        {
            CardName = "Urza's Saga";
            Land = true;
            Artifact = false;
            Enchantment = true;

            var loreCounting = new UrzasSagaLoreCounting();
            Features[typeof(IOnBeginPhaseFeat)] = Features[typeof(IOnEnterGameFeat)] = loreCounting;
        }
    }
}
