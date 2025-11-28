using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Event Collection", order = 99)]
    [DeclareBoxGroup("Events")]
    public class EventCollectionData : ScriptableObject
    {
        [SerializeField, Group("Events")] private SerializedDictionary<string, GameEventBaseAsset> _eventAssets;

        //
        public SerializedDictionary<string, GameEventBaseAsset> EventAssets => _eventAssets;

        public GameEventBaseAsset GetEventAsset(string key) => _eventAssets.FirstOrDefault(x => x.Key == key).Value;
    }
}