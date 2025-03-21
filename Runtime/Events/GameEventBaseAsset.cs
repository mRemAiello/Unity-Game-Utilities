using System.Collections.Generic;
using TriInspector;
using UnityEngine;

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