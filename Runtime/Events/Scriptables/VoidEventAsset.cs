using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Void", order = 99)]
    public class VoidEventAsset : GameEventAssetBase
    {
        protected Action _onInvoked;

        //
        public void AddListener(MonoBehaviour caller, Action action)
        {
            if (!TryRegisterListener(caller, action))
                return;

            _onInvoked += action;
        }

        public void RemoveListener(MonoBehaviour caller, Action action)
        {
            if (!TryUnregisterListener(caller, action))
                return;

            _onInvoked -= action;
        }

        [Button(ButtonSizes.Medium)]
        public void RemoveAllListeners()
        {
            ClearRuntimeListeners();
            _onInvoked = null;
        }

        public override void ResetData()
        {
            ClearRuntimeListeners();
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
