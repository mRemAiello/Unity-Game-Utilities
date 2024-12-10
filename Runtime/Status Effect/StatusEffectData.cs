using UnityEngine;
using UnityEngine.Localization;
using VInspector;

namespace GameUtils
{
    public abstract class StatusEffectData : UniqueID
    {
        [Tab("Graphics")]
        [SerializeField] private LocalizedString _effectName;
        [SerializeField] private LocalizedString _effectDescription;
        [SerializeField] private Sprite _icon;

        [Tab("Effects")]
        [SerializeField] private int _order = 0;

        //
        public string EffectName => _effectName.GetLocalizedString();
        public string EffectDescription => _effectDescription.GetLocalizedString();
        public Sprite Icon => _icon;
        public int Order => _order;

        //

        //
        public abstract void ApplyEffect(StatusEffect effect);
        public abstract void UpdateEffect(StatusEffect effect);
        public abstract void EndEffect(StatusEffect effect);
    }
}