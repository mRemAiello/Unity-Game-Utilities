using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class GameEventBaseAsset : ScriptableObject, ILoggable
    {
        [SerializeField, PropertyOrder(0)] private bool _logEnabled = false;
        [SerializeField, PropertyOrder(99), TableList(AlwaysExpanded = true), ReadOnly] private List<EventTuple> _listeners = new();

        //
        public bool LogEnabled => _logEnabled;

        //
        public IReadOnlyList<EventTuple> Listeners => _listeners;
        protected List<EventTuple> MutableListeners => _listeners;

        private void OnValidate()
        {
            _listeners ??= new List<EventTuple>();
        }
    }
}