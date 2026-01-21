using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("log", Title = "Log")]
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Void", order = 99)]
    public class VoidEventAsset : ScriptableObject, ILoggable
    {
        [SerializeField, Group("log")] private bool _logEnabled = false;
        [SerializeField, Group("debug"), TableList(AlwaysExpanded = true), ReadOnly] protected List<EventTuple> _runtimeListeners = new();

        // private
        protected Action _onInvoked;

        //
        public bool LogEnabled => _logEnabled;

        //
        public void AddListener(Action action)
        {
            if (action == null)
                return;

            //
            _runtimeListeners.Add(new EventTuple
            {
                Caller = action.Target != null ? action.Target.ToString() : "Static",
                MethodName = action.Method.Name,
                ClassName = action.Method.DeclaringType?.Name
            });

            //
            _onInvoked += action;
        }

        public void RemoveListener(Action action)
        {
            // Deletes the listener and its reference from the runtime listeners list.
            _runtimeListeners.RemoveAll(tuple => tuple.Caller == action.Target?.ToString() && tuple.MethodName == action.Method.Name);

            //
            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            _runtimeListeners.Clear();
            _onInvoked = null;
        }

        [Button(ButtonSizes.Medium)]
        public void Invoke()
        {
            // Gestisce l'invocazione dell'evento con tracciamento opzionale.
            this.Log($"[VoidEventAsset] Invoked", this);

            //
            _onInvoked?.Invoke();
        }
    }
}
