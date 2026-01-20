using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Components/Vector3")]
    public class Vector3EventAsset : GameEventAsset<Vector3>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(Vector3 param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
