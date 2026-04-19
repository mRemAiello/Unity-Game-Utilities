using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Numeric/Int")]
    public class IntEventAsset : GameEventAsset<int>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(int param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
