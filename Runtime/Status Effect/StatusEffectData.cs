using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("effect", Title = "Status Effect")]
    public abstract class StatusEffectData : ItemAssetBase
    {
        [SerializeField, Group("effect")] private int _order = 0;
        [SerializeField, Group("effect")] private StatusEffectStackType _stackType;
        [ShowIf(nameof(_stackType), StatusEffectStackType.Duration), SerializeField, Group("effect")] private int _maxDuration;

        //
        public int Order => _order;

        //
        public StatusEffectStackType StackType => _stackType;
        public int MaxDuration => _maxDuration;

        //
        public abstract void ApplyEffect(StatusEffect effect);
        public abstract void UpdateEffect(StatusEffect effect);
        public abstract void EndEffect(StatusEffect effect);
    }
}