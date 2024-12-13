using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class StatusEffectManager : MonoBehaviour
    {
        [Tab("Events")]
        [SerializeField] private StatusEffectEventAsset _onApplyEffect;
        [SerializeField] private StatusEffectEventAsset _onUpdateEffect;
        [SerializeField] private StatusEffectEventAsset _onEndEffect;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private List<StatusEffect> _statusEffects = new();

        //
        public List<StatusEffect> StatusEffects => _statusEffects;

        //
        [Button]
        public void ApplyEffect(GameObject source, GameObject target, StatusEffectData data, int amount)
        {
            // TODO: Fixare qui
            StatusEffect statusEffect = new()
            {
                ID = data.ID,
                Source = source,
                Target = target,
                Amount = amount,
                Data = data
            };

            //
            //if (data.CanStack)
            {
                _statusEffects.Add(statusEffect);
            }
            //else
            {
                
            }

            //
            ReorderEffects();
        }

        [Button]
        public void UpdateEffect()
        {
            foreach (var effect in _statusEffects)
            {
                if (effect.Amount > 0)
                {
                    effect.Data.UpdateEffect(effect);
                    _onUpdateEffect?.Invoke(effect);

                    //
                    effect.Amount--;
                }

                //
                if (effect.Amount <= 0)
                {
                    effect.Data.EndEffect(effect);
                    _onEndEffect?.Invoke(effect);
                }
            }

            //
            _statusEffects.RemoveAll(item => item.Amount == 0);
        }

        [Button]
        public void RemoveEffect(StatusEffect effect, bool launchEndEvent = false)
        {
            _statusEffects.Remove(effect);
            if (launchEndEvent)
            {
                effect.Data.EndEffect(effect);
                _onEndEffect?.Invoke(effect);
            }

            //
            ReorderEffects();
        }

        [Button]
        public void RemoveAllEffects(string ID, bool launchEndEvent = false)
        {
            var effectsToRemove = FindEffects(ID);
            foreach (var effect in effectsToRemove)
            {
                RemoveEffect(effect, launchEndEvent);
            }
        }

        public List<StatusEffect> FindEffects(string ID)
        {
            List<StatusEffect> effects = new();
            foreach (StatusEffect effect in _statusEffects)
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
        public bool HasEffect(string ID) => FindEffects(ID).Count > 0;
        public bool HasEffect(StatusEffectData data) => FindEffects(data.ID).Count > 0;
        private void ReorderEffects() => _statusEffects = _statusEffects.OrderBy(item => item.Amount).ToList();
    }
}