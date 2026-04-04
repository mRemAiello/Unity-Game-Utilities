using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("StatusEffect")]
    public abstract class StatusEffectData : ItemVisualData
    {
        [SerializeField, Group("StatusEffect")] private bool _isVisible = true;
        [SerializeField, Group("StatusEffect")] private int _order = 0;
        [SerializeField, Group("StatusEffect")] private StatusEffectStackType _stackType;
        [ShowIf(nameof(_stackType), StatusEffectStackType.Duration), SerializeField, Group("StatusEffect")] private int _maxDuration;

        //
        public int Order => _order;

        //
        public bool IsVisible => _isVisible;
        public StatusEffectStackType StackType => _stackType;
        public int MaxDuration => _maxDuration;

        //
        public abstract void ApplyEffect(RuntimeStatusEffect effect);
        public abstract void UpdateEffect(RuntimeStatusEffect effect);
        public abstract void EndEffect(RuntimeStatusEffect effect);
    }
}
