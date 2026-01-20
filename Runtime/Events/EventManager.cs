using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-100)]
    public class GameEventDataManager //: //GenericDataManager<GameEventDataManager, GameEventAssetBase>, ILoggable
    {
        [SerializeField] private bool _logEnabled = true;

        //
        public bool LogEnabled => _logEnabled;

        /*
        [SerializeField] private List<EventCollectionData> _eventDatabase;



        //
        public virtual GameEventAssetBase GetEventBaseAssetByName(string eventName)
        {
            foreach (var database in _eventDatabase)
            {
                if (database.EventAssets.TryGetValue(eventName, out GameEventAssetBase baseEvent))
                {
                    return baseEvent;
                }
            }

            this.LogError($"No base event found with ID: {eventName}");
            return null;
        }

        public virtual GameEventAsset<T> GetEventAssetByName<T>(string eventName)
        {
            return GetEventBaseAssetByName(eventName) as GameEventAsset<T>;
        }

        public virtual void AddListenerToEventByName<T>(string eventName, UnityAction<T> call)
        {
            GetEventAssetByName<T>(eventName)?.AddListener(call);
        }

        public virtual void RemoveListenerToEventByName<T>(string eventName, UnityAction<T> call)
        {
            GetEventAssetByName<T>(eventName)?.RemoveListener(call);
        }

        public virtual void RemoveAllListerToEventByName<T>(string eventName)
        {
            GetEventAssetByName<T>(eventName)?.RemoveAllListeners();
        }

        public virtual void InvokeEventByName<T>(string eventName, T param)
        {
            GetEventAssetByName<T>(eventName)?.Invoke(param);
        }*/
    }
}
