using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class GameEventBaseAsset : ScriptableObject, ILoggable
    {
        [SerializeField] private bool _logEnabled = false;
        [SerializeField, Group("debug"), ReadOnly] private List<EventTuple> _listeners;

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