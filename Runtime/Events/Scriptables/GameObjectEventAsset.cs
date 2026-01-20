using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Components/GameObject")]
    public class GameObjectEventAsset : GameEventAsset<GameObject>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(GameObject param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}
