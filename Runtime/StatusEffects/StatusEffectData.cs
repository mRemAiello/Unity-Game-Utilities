using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("status-effect", Title = "StatusEffect")]
    public abstract class StatusEffectData : ItemAssetBase
    {
        [SerializeField, Group("status-effect")] private bool _isVisible = true;
        [SerializeField, Group("status-effect")] private int _order = 0;
        [SerializeField, Group("status-effect")] private StatusEffectStackType _stackType;
        [ShowIf(nameof(_stackType), StatusEffectStackType.Duration), SerializeField, Group("status-effect")] private int _maxDuration;
        [SerializeField, Group("status-effect")] private List<GameTag> _tags = new();

        //
        public int Order => _order;

        //
        public bool IsVisible => _isVisible;
        public StatusEffectStackType StackType => _stackType;
        public int MaxDuration => _maxDuration;
        public IReadOnlyList<GameTag> Tags => _tags;

        //
        public abstract void ApplyEffect(RuntimeStatusEffect effect);
        public abstract void UpdateEffect(RuntimeStatusEffect effect);
        public abstract void EndEffect(RuntimeStatusEffect effect);
    }
}