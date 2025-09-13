using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(0)]
    public class StatusEffectManager : MonoBehaviour
    {
        [Tab("Events")]
        [SerializeField] private StatusEffectEventAsset _onApplyEffect;
        [SerializeField] private StatusEffectEventAsset _onUpdateEffect;
        [SerializeField] private StatusEffectEventAsset _onEndEffect;

        [Tab("Debug")]
        [SerializeField, ReadOnly, TableList] private List<RuntimeStatusEffect> _statusEffects = new();

        [Tab("Tags")]
        [SerializeField, ReadOnly] private TagManager _tags = new();
        [SerializeField, ReadOnly] private TagManager _immunities = new();

        //
        public List<RuntimeStatusEffect> StatusEffects => _statusEffects;
        public TagManager Tags => _tags;
        public TagManager Immunities => _immunities;

        //
        [Button]
        public void ApplyEffect(GameObject source, GameObject target, StatusEffectData data, int amount)
        {
            if (_immunities.HasAny(data.Tags.ToArray()))
                return;

            RuntimeStatusEffect effect = FindEffect(data.ID);
            if (effect == null)
            {
                effect = new RuntimeStatusEffect(data.ID, source, target, data);
                _statusEffects.Add(effect);
            }

            //
            if (data.StackType == StatusEffectStackType.Duration)
                effect.Duration = Mathf.Min(effect.Duration + amount, data.MaxDuration);

            //
            if (data.StackType == StatusEffectStackType.Intensity)
                effect.Intensity += amount;

            //
            ReorderEffects();
            RefreshTags();
        }

        [Button]
        public void UpdateEffect()
        {
            //
            var statusEffects = _statusEffects.Where(x => x.Data.StackType == StatusEffectStackType.Duration).ToList();

            //
            foreach (var effect in _statusEffects)
            {
                if (effect.Duration > 0)
                {
                    effect.Data.UpdateEffect(effect);
                    _onUpdateEffect?.Invoke(effect);

                    //
                    effect.Duration--;
                }

                //
                if (effect.Duration <= 0)
                {
                    effect.Data.EndEffect(effect);
                    _onEndEffect?.Invoke(effect);
                }
            }

            //
            _statusEffects.RemoveAll(item => item.Duration == 0);
            RefreshTags();
        }

        [Button]
        public void RemoveEffect(RuntimeStatusEffect effect, bool launchEndEvent = false)
        {
            _statusEffects.Remove(effect);
            if (launchEndEvent)
            {
                effect.Data.EndEffect(effect);
                _onEndEffect?.Invoke(effect);
            }

            //
            ReorderEffects();
            RefreshTags();
        }

        [Button]
        public void RemoveAllEffects(string ID, bool launchEndEvent = false)
        {
            var effectsToRemove = FindEffects(ID);
            foreach (var effect in effectsToRemove)
            {
                RemoveEffect(effect, launchEndEvent);
            }
            RefreshTags();
        }

        public List<RuntimeStatusEffect> FindEffects(string ID)
        {
            List<RuntimeStatusEffect> effects = new();
            foreach (RuntimeStatusEffect effect in _statusEffects)
            {
                if (effect.ID.Equals(ID))
                {
                    effects.Add(effect);
                }
            }

            //
            return effects;
        }

        //
        public RuntimeStatusEffect FindEffect(string ID) => _statusEffects.FirstOrDefault(x => x.ID.Equals(ID));
        public RuntimeStatusEffect FindEffect(StatusEffectData data) => FindEffect(data.ID);
        public List<RuntimeStatusEffect> FindEffects(StatusEffectData data) => FindEffects(data.ID);
        public bool HasEffect(string ID) => FindEffects(ID).Count > 0;
        public bool HasEffect(StatusEffectData data) => FindEffects(data.ID).Count > 0;
        public bool HasEffect<T>() where T : StatusEffectData => _statusEffects.Any(x => x.Data is T);
        private void RefreshTags()
        {
            _tags.Clear();
            foreach (var effect in _statusEffects)
            {
                foreach (var tag in effect.Data.Tags)
                {
                    int current = 0;
                    if (_tags.TryGetValue(tag.ID, out RuntimeTag runtimeTag))
                    {
                        current = runtimeTag.Value;
                    }
                    _tags.SetTagValue(tag, current + 1);
                }
            }
        }

        private void ReorderEffects() => _statusEffects = _statusEffects.OrderBy(item => item.Duration).ToList();
    }
}