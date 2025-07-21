using System;

namespace GameUtils
{
    internal class PrevDependentNodeWithValue<T> : ITrackerNode<T>
    {
        private Action<T> _onChangedCallback1;
        private Action<T, T> _onChangedCallback2;
        private ITrackerNode<T> _previousNode;

        public bool Changed => _previousNode.Changed;
        public T PreviousValue => _previousNode.PreviousValue;
        public T CurrentValue => _previousNode.CurrentValue;

        //
        public PrevDependentNodeWithValue(Action<T> onChangedCallback1, Action<T, T> onChangedCallback2, ITrackerNode previousNode)
        {
            if (previousNode is ITrackerNode<T> valueNode)
            {
                _onChangedCallback1 = onChangedCallback1;
                _onChangedCallback2 = onChangedCallback2;
                _previousNode = valueNode;
            }
            else
            {
                throw new InvalidOperationException($"Previous node does not cache value or value is not {typeof(T)}.");
            }
        }

        public void Check()
        {
            if (_previousNode.Changed)
            {
                _onChangedCallback1?.Invoke(_previousNode.CurrentValue);
                _onChangedCallback2?.Invoke(_previousNode.PreviousValue, _previousNode.CurrentValue);
            }
        }

        public void ForceInvoke()
        {
            _onChangedCallback1?.Invoke(_previousNode.CurrentValue);
            _onChangedCallback2?.Invoke(_previousNode.PreviousValue, _previousNode.CurrentValue);
        }
    }
}