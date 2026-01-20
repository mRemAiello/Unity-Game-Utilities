using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Components/Transform")]
    public class TransformEventAsset : GameEventAsset<Transform>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(Transform param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
