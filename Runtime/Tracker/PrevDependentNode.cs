using System;

namespace GameUtils
{

    public class PrevDependentNode : BaseDependentNode
    {
        private ITrackerNode _previousNode;

        public override bool Changed => _previousNode.Changed;

        public PrevDependentNode(Action onChangedCallback, ITrackerNode previousNode) : base(onChangedCallback)
        {
            _previousNode = previousNode;
        }
    }
}