using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(0)]
    public class StatusEffectManager : MonoBehaviour, ISaveable
    {
        [Tab("Events")]
        [SerializeField] private string _saveContext = "StatusEffects";
        [SerializeField] private StatusEffectEventAsset _onApplyEffect;
        [SerializeField] private StatusEffectEventAsset _onUpdateEffect;
        [SerializeField] private StatusEffectEventAsset _onEndEffect;

        [Tab("Debug")]
        [SerializeField, ReadOnly, TableList] private List<RuntimeStatusEffect> _statusEffects = new();

        [Tab("Tags")]
        [SerializeField, ReadOnly] private TagManager _tags = new();
        [SerializeField, ReadOnly] private TagManager _immunities = new();

        //
        public string SaveContext => _saveContext;
        public IReadOnlyList<RuntimeStatusEffect> StatusEffects => _statusEffects;
        public TagManager Tags => _tags;
        public TagManager Immunities => _immunities;

        //
        [Button(ButtonSizes.Medium)]
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

        [Button(ButtonSizes.Medium)]
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

        [Button(ButtonSizes.Medium)]
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

        [Button(ButtonSizes.Medium)]
        public void RemoveAllEffects(string ID, bool launchEndEvent = false)
        {
            var effectsToRemove = FindEffects(ID);
            foreach (var effect in effectsToRemove)
            {
                RemoveEffect(effect, launchEndEvent);
            }
            RefreshTags();
        }

        public IReadOnlyList<RuntimeStatusEffect> FindEffects(string ID)
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

        public void Save()
        {
            var saveData = new StatusEffectManagerSaveData
            {
                Effects = _statusEffects.Select(SerializeRuntimeEffect).ToList(),
                Tags = SerializeTagManager(_tags).ToList(),
                Immunities = SerializeTagManager(_immunities).ToList()
            };

            GameSaveManager.Instance.Save(SaveContext, "State", saveData);
        }

        public void Load()
        {
            _statusEffects.Clear();
            _tags.Clear();
            _immunities.Clear();

            var saveData = GameSaveManager.Instance.Load(SaveContext, "State", new StatusEffectManagerSaveData());
            if (saveData == null)
            {
                return;
            }

            foreach (var tag in saveData.Tags)
            {
                RestoreTag(_tags, tag);
            }

            foreach (var tag in saveData.Immunities)
            {
                RestoreTag(_immunities, tag);
            }

            foreach (var effectSave in saveData.Effects)
            {
                var data = StatusEffectDataManager.InstanceExists ? StatusEffectDataManager.Instance.GetEffect(effectSave.ID) : null;
                if (data == null)
                {
                    Debug.LogWarning($"StatusEffectData with ID '{effectSave.ID}' not found while loading status effects.");
                    continue;
                }

                var runtimeEffect = new RuntimeStatusEffect(effectSave.ID, ResolveObject(effectSave.SourcePath), ResolveObject(effectSave.TargetPath), data)
                {
                    Duration = effectSave.Duration,
                    Intensity = effectSave.Intensity
                };

                _statusEffects.Add(runtimeEffect);
                _onApplyEffect?.Invoke(runtimeEffect);
            }

            ReorderEffects();
            RefreshTags();
        }

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

        private IEnumerable<TagSaveData> SerializeTagManager(TagManager manager)
        {
            foreach (var kvp in manager.GetMap())
            {
                yield return new TagSaveData
                {
                    TagID = kvp.Key,
                    Value = kvp.Value?.Value ?? 0
                };
            }
        }

        private void RestoreTag(TagManager manager, TagSaveData tagData)
        {
            if (!GameTagManager.InstanceExists)
            {
                Debug.LogWarning("GameTagManager instance does not exist. Cannot restore tag state.");
                return;
            }

            if (GameTagManager.Instance.TryGetTag(tagData.TagID, out var tag))
            {
                manager.SetTagValue(tag, tagData.Value);
            }
            else
            {
                Debug.LogWarning($"Tag with ID '{tagData.TagID}' not found while loading status effects.");
            }
        }

        private StatusEffectSaveData SerializeRuntimeEffect(RuntimeStatusEffect effect)
        {
            return new StatusEffectSaveData
            {
                ID = effect.ID,
                Duration = effect.Duration,
                Intensity = effect.Intensity,
                SourcePath = GetHierarchyPath(effect.Source),
                TargetPath = GetHierarchyPath(effect.Target)
            };
        }

        private GameObject ResolveObject(string objectPath)
        {
            if (string.IsNullOrEmpty(objectPath))
            {
                return null;
            }

            return GameObject.Find(objectPath);
        }

        private string GetHierarchyPath(GameObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            var current = obj.transform;
            var path = current.name;

            while (current.parent != null)
            {
                current = current.parent;
                path = $"{current.name}/{path}";
            }

            return path;
        }

        //
        public RuntimeStatusEffect FindEffect(string ID) => _statusEffects.FirstOrDefault(x => x.ID.Equals(ID));
        public RuntimeStatusEffect FindEffect(StatusEffectData data) => FindEffect(data.ID);
        public IReadOnlyList<RuntimeStatusEffect> FindEffects(StatusEffectData data) => FindEffects(data.ID);
        public bool HasEffect(string ID) => FindEffects(ID).Count > 0;
        public bool HasEffect(StatusEffectData data) => FindEffects(data.ID).Count > 0;
        public bool HasEffect<T>() where T : StatusEffectData => _statusEffects.Any(x => x.Data is T);
        private void ReorderEffects() => _statusEffects = _statusEffects.OrderBy(item => item.Duration).ToList();
    }
}