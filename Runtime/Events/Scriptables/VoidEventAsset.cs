using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Void", order = 99)]
    public class VoidEventAsset : GameEventAssetBase
    {
        //
        [NonSerialized] protected List<EventTuple> _runtimeListeners = new();
        protected Action _onInvoked;

        //
        [ShowInInspector, Group("Debug"), TableList(AlwaysExpanded = true), ReadOnly] public List<EventTuple> RuntimeListeners => _runtimeListeners;

        //
        public override void ResetData()
        {
            _runtimeListeners = new List<EventTuple>();
        }

        public void AddListener(MonoBehaviour caller, Action action)
        {
            if (action == null)
            {
                this.LogWarning($"Attempted to add a null listener.", this);
                return;
            }

            //
            _runtimeListeners.Add(new EventTuple
            {
                CallerGameObject = caller.gameObject,
                CallerScript = caller,
                MethodName = action.Method.Name
            });

            //
            _onInvoked += action;
        }

        public void RemoveListener(MonoBehaviour caller, Action action)
        {
            // Deletes the listener and its reference from the runtime listeners list.
            _runtimeListeners.RemoveAll(tuple => tuple.CallerScript == caller && tuple.MethodName == action.Method.Name);
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
            this.Log($"{name} invoked", this);

            //
            _onInvoked?.Invoke();
        }
    }
}
