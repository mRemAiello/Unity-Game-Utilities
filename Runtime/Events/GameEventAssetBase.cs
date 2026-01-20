using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("log", Title = "Log")]
    public class GameEventAssetBase : ScriptableObject, ILoggable
    {
        [SerializeField, Group("log"), PropertyOrder(0)] private bool _logEnabled = false;
        
        //
        [SerializeField, PropertyOrder(2), Group("debug"), TableList(AlwaysExpanded = true, HideAddButton = true, HideRemoveButton = true), ReadOnly] 
        private List<EventTuple> _listeners = new();

        //
        public bool LogEnabled => _logEnabled;
        public IReadOnlyList<EventTuple> Listeners => _listeners;
        protected List<EventTuple> MutableListeners => _listeners;

        //
        private void OnValidate()
        {
            _listeners ??= new List<EventTuple>();
        }
    }
}