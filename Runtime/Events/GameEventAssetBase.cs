using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public abstract class GameEventAssetBase : ItemIdentifierData, ILoggable
    {
        [SerializeField, Group("debug")] private bool _logEnabled = false;

        //
        public bool LogEnabled => _logEnabled;

        //
        public abstract void ResetData();
    }
}