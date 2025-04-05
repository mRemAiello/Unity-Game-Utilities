using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class GameEventBaseAsset : ScriptableObject, ILoggable
    {
        [SerializeField, PropertyOrder(0)] private bool _logEnabled = false;
        [SerializeField, PropertyOrder(99), TableList(AlwaysExpanded = true), ReadOnly] private List<EventTuple> _listeners;

        //
        public bool LogEnabled => _logEnabled;

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