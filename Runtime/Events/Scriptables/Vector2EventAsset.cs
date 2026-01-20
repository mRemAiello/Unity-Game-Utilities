using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Components/Vector2")]
    public class Vector2EventAsset : GameEventAsset<Vector2>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(Vector2 param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
