using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtils
{
    [DeclareBoxGroup("Data")]
    [DeclareBoxGroup("UI")]
    [DeclareBoxGroup("Visuals")]
    [DeclareBoxGroup("Events")]
    [DeclareBoxGroup("Debug")]
    public class RuntimeSkillNode : MonoBehaviour, IPointerClickHandler, ILoggable
    {
        [SerializeField, Required, Group("Data")] private SkillNodeData _data;
        [SerializeField, Group("Data")] private List<RuntimeSkillNode> _prerequisiteNodes = new();
        [SerializeField, Group("Events")] private ClickSkillEventAsset _onClicked;
        [SerializeField, Group("Events")] private ChangeSkillStateEventAsset _onStateChanged;
        [SerializeField, ReadOnly, Group("Debug")] private bool _logEnabled = false;
        [SerializeField, ReadOnly, Group("Debug")] private SkillNodeState _state = SkillNodeState.Locked;

        //
        public bool LogEnabled => _logEnabled;
        public SkillNodeData Data => _data;
        public SkillNodeState State => _state;
        public IReadOnlyList<RuntimeSkillNode> PrerequisiteNodes => _prerequisiteNodes;

        //
        public void SetState(SkillNodeState newState)
        {
            if (_state == newState)
                return;

            _state = newState;
            ApplyVisualState();
            _onStateChanged?.Invoke(this, _state);
        }

        public void RefreshState(ISkillContext context)
        {
            if (context.TryGet<ISkillStateProvider>(out var provider) && provider.IsUnlocked(_data.ID))
            {
                SetState(SkillNodeState.Unlocked);
            }
            else if (_data.IsAvailable(context))
            {
                SetState(SkillNodeState.Available);
            }
            else
            {
                SetState(SkillNodeState.Locked);
            }
        }

        //
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _onClicked?.Invoke(this);
        }

        //
        public virtual void Init() { }
        protected virtual void ApplyVisualState() { }
    }
}
