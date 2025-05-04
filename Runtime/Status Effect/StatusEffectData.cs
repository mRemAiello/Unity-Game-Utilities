using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("status-effect", Title = "Status Effect")]
    public abstract class StatusEffectData : ItemAssetBase
    {
        [SerializeField, Group("status-effect")] private bool _isVisible = true;
        [SerializeField, Group("status-effect")] private int _order = 0;
        [SerializeField, Group("status-effect")] private StatusEffectStackType _stackType;
        [ShowIf(nameof(_stackType), StatusEffectStackType.Duration), SerializeField, Group("status-effect")] private int _maxDuration;

        //
        public int Order => _order;

        //
        public bool IsVisible => _isVisible;
        public StatusEffectStackType StackType => _stackType;
        public int MaxDuration => _maxDuration;

        //
        public abstract void ApplyEffect(StatusEffect effect);
        public abstract void UpdateEffect(StatusEffect effect);
        public abstract void EndEffect(StatusEffect effect);
    }
}