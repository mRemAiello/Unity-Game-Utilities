using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Components/Quaternion")]
    public class QuaternionEventAsset : GameEventAsset<Quaternion>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(Quaternion param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
