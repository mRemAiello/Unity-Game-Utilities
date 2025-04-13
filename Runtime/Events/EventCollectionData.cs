using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.EVENT_NAME + "Event Collection")]
    [DeclareBoxGroup("Events")]
    public class EventCollectionData : ScriptableObject
    {
        [SerializeField, Group("Events")] private SerializedDictionary<string, GameEventBaseAsset> _eventAssets;

        //
        public SerializedDictionary<string, GameEventBaseAsset> EventAssets => _eventAssets;

        public GameEventBaseAsset GetEventAsset(string key) => _eventAssets.FirstOrDefault(x => x.Key == key).Value;
    }
}