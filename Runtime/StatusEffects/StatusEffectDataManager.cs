using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Centralized loader for <see cref="StatusEffectData"/> assets with utilities to query by ID, visibility or tags.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class StatusEffectDataManager : GenericDataManager<StatusEffectDataManager, StatusEffectData>
    {
        private readonly Dictionary<string, StatusEffectData> _effectsById = new();
        private readonly Dictionary<GameTag, List<StatusEffectData>> _effectsByTag = new();
        private readonly List<StatusEffectData> _visibleEffects = new();

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            _effectsById.Clear();
            _effectsByTag.Clear();
            _visibleEffects.Clear();

            foreach (var effect in Items)
            {
                _effectsById[effect.ID] = effect;

                if (effect.IsVisible)
                {
                    _visibleEffects.Add(effect);
                }

                foreach (var tag in effect.Tags)
                {
                    if (!_effectsByTag.TryGetValue(tag, out var list))
                    {
                        list = new List<StatusEffectData>();
                        _effectsByTag[tag] = list;
                    }

                    list.Add(effect);
                }
            }
        }

        public bool TryGetEffect(string id, out StatusEffectData data) => _effectsById.TryGetValue(id, out data);

        public StatusEffectData GetEffect(string id)
        {
            if (TryGetEffect(id, out var data))
            {
                return data;
            }

            this.LogError($"StatusEffect with ID '{id}' not found.");
            return null;
        }

        public IReadOnlyList<StatusEffectData> GetEffectsWithTag(GameTag tag) =>
            _effectsByTag.TryGetValue(tag, out var results) ? results : Array.Empty<StatusEffectData>();

        public IReadOnlyList<StatusEffectData> GetVisibleEffects() => _visibleEffects;
    }
}
