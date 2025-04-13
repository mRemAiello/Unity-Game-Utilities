using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    public class EventManager : MonoBehaviour, ILoggable
    {
        [SerializeField] private bool _logEnabled = true;
        [SerializeField] private List<EventCollectionData> _eventDatabase;

        //
        public bool LogEnabled => _logEnabled;

        /// <summary>
        /// Get the Event Base Asset by the name
        /// </summary>
        /// <param name="eventName">The name of the desired Event Base Asset</param>
        /// <returns>The found Event Base Asset (if any)</returns>
        public virtual GameEventBaseAsset GetEventBaseAssetByName(string eventName)
        {
            foreach (var database in _eventDatabase)
            {
                if (database.EventAssets.TryGetValue(eventName, out GameEventBaseAsset baseEvent))
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
        }
    }
}