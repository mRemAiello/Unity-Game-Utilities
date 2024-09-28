using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class GameEventBaseAsset : ScriptableObject
    {
        [SerializeField, ReadOnly] private List<EventTuple> _listeners;
        
        //
        public List<EventTuple> Listeners
        {
            get => _listeners;
            set => _listeners = value;
        }

        private void OnValidate()
        {
            Listeners = new List<EventTuple>();
        }
    }
}