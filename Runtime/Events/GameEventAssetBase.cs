using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class GameEventAssetBase : ItemIdentifierData, ILoggable
    {
        [SerializeField, Group("debug")] private bool _logEnabled = false;

        [SerializeField, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] protected List<EventTuple> _runtimeListeners = new();

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button(ButtonSizes.Medium)]
        public virtual void ResetData()
        {
            _runtimeListeners = new List<EventTuple>();
        }
    }
}