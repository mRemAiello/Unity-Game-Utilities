using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Debug")]
    public abstract class GameEventAssetBase : ItemIdentifierData, ILoggable
    {
        [SerializeField, Group("Debug")] private bool _logEnabled = false;

        //
        public bool LogEnabled => _logEnabled;

        //
        public abstract void ResetData();
    }
}