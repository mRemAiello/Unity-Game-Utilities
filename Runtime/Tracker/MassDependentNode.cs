using System;

namespace GameUtils
{
    public class MassDependentNode : BaseDependentNode
    {
        private ITrackerNode[] _dependencies;

        public override bool Changed
        {
            get
            {
                for (int i = 0; i < _dependencies.Length; i++)
                {
                    if (_dependencies[i].Changed)
                        return true;
                }

                return false;
            }
        }

        public MassDependentNode(Action onChangedCallback, ITrackerNode[] dependencies) : base(onChangedCallback)
        {
            _dependencies = dependencies;
        }
    }
}