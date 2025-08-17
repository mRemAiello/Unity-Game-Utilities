using System;

namespace GameUtils
{
    public abstract class BaseDependentNode : ITrackerNode
    {
        private Action _onChangedCallback;

        public abstract bool Changed { get; }

        public BaseDependentNode(Action onChangedCallback)
        {
            _onChangedCallback = onChangedCallback;
        }

        public void Check()
        {
            if (Changed)
            {
                _onChangedCallback();
            }
        }

        public void ForceInvoke() => _onChangedCallback();
    }
}