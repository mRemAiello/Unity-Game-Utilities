using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Numeric/Float")]
    public class FloatEventAsset : GameEventAsset<float>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(float param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
