using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUtils
{
    [DeclareBoxGroup("Data")]
    [DeclareBoxGroup("Input")]
    [DeclareBoxGroup("Events")]
    [DeclareBoxGroup("Debug")]
    public class RuntimeSkillNode : MonoBehaviour, ILoggable
    {
        [SerializeField, Required, Group("Data")] private SkillNodeData _data;
        [SerializeField, Group("Data")] private List<RuntimeSkillNode> _prerequisiteNodes = new();
        [SerializeField, Group("Data")] private List<RuntimeSkillNode> _blockedByNodes = new();
        [SerializeField, Group("Input")] private InputActionReference _levelUpAction;
        [SerializeField, Group("Input")] private InputActionReference _levelDownAction;
        [SerializeField, Group("Events")] private ClickSkillEventAsset _onLevelUpRequest;
        [SerializeField, Group("Events")] private ClickSkillEventAsset _onLevelDownRequest;
        [SerializeField, Group("Events")] private ChangeSkillStateEventAsset _onStateChanged;
        [SerializeField, ReadOnly, Group("Debug"), PropertyOrder(99)] private bool _logEnabled = false;
        [SerializeField, ReadOnly, Group("Debug"), PropertyOrder(99)] private SkillNodeState _state = SkillNodeState.Locked;
        [SerializeField, ReadOnly, Group("Debug"), PropertyOrder(99)] private int _currentLevel;

        //
        private bool _isHovered;

        //
        public bool LogEnabled => _logEnabled;
        public SkillNodeData Data => _data;
        public SkillNodeState State => _state;
        public int CurrentLevel => _currentLevel;
        public IReadOnlyList<RuntimeSkillNode> PrerequisiteNodes => _prerequisiteNodes;
        public IReadOnlyList<RuntimeSkillNode> BlockedByNodes => _blockedByNodes;

        //
        private void OnEnable()
        {
            if (_levelUpAction != null)
            {
                _levelUpAction.action.Enable();
                _levelUpAction.action.performed += OnLevelUpPerformed;
            }

            if (_levelDownAction != null)
            {
                _levelDownAction.action.Enable();
                _levelDownAction.action.performed += OnLevelDownPerformed;
            }
        }

        private void OnDisable()
        {
            if (_levelUpAction != null)
            {
                _levelUpAction.action.performed -= OnLevelUpPerformed;
            }

            if (_levelDownAction != null)
            {
                _levelDownAction.action.performed -= OnLevelDownPerformed;
            }
        }

        //
        public void SetState(SkillNodeState newState, int level)
        {
            bool changed = _state != newState || _currentLevel != level;
            if (!changed)
                return;

            _state = newState;
            _currentLevel = level;
            ApplyVisualState();
            _onStateChanged?.Invoke(this, _state, _currentLevel);
        }

        public void RefreshState(ISkillContext context)
        {
            if (context.TryGet<ISkillStateProvider>(out var provider) && provider.IsUnlocked(_data.ID))
            {
                int level = provider.GetLevel(_data.ID);
                var state = level >= _data.MaxLevel ? SkillNodeState.Maxed : SkillNodeState.Unlocked;
                SetState(state, level);
            }
            else if (IsBlockedByExclusiveChoice())
            {
                SetState(SkillNodeState.Blocked, 0);
            }
            else if (ArePrerequisitesMet())
            {
                SetState(SkillNodeState.Available, 0);
            }
            else
            {
                SetState(SkillNodeState.Locked, 0);
            }
        }

        /// <summary>
        /// Returns true if any mutually exclusive sibling node is currently unlocked or maxed,
        /// making this node an unavailable choice.
        /// </summary>
        public bool IsBlockedByExclusiveChoice()
        {
            if (_blockedByNodes == null || _blockedByNodes.Count == 0)
                return false;

            foreach (var blocker in _blockedByNodes)
            {
                if (blocker != null && (blocker.State == SkillNodeState.Unlocked || blocker.State == SkillNodeState.Maxed))
                    return true;
            }

            return false;
        }

        public bool ArePrerequisitesMet()
        {
            if (_prerequisiteNodes == null || _prerequisiteNodes.Count == 0)
                return true;

            foreach (var prereqNode in _prerequisiteNodes)
            {
                if (prereqNode.State == SkillNodeState.Unlocked || prereqNode.State == SkillNodeState.Maxed)
                    return true;
            }

            return false;
        }

        //
        private void OnLevelUpPerformed(InputAction.CallbackContext ctx)
        {
            if (!_isHovered)
                return;

            // Only allow level up if currently available or unlocked (but not maxed)
            _onLevelUpRequest?.Invoke(this);
        }

        private void OnLevelDownPerformed(InputAction.CallbackContext ctx)
        {
            if (!_isHovered)
                return;

            // Only allow level down if currently unlocked
            _onLevelDownRequest?.Invoke(this);
        }

        public void SetHovered(bool hovered) => _isHovered = hovered;

        //
        public virtual void Init() { }
        protected virtual void ApplyVisualState() { }
    }
}
