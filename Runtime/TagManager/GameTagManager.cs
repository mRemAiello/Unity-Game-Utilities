using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Loads and exposes all <see cref="GameTag"/> assets for runtime usage.
    /// Provides fast lookup by ID and by concrete tag type.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameTagManager : GenericDataManager<GameTagManager, GameTag>
    {
        private readonly Dictionary<string, GameTag> _tagsById = new();
        private readonly Dictionary<Type, GameTag> _tagsByType = new();

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            _tagsById.Clear();
            _tagsByType.Clear();

            foreach (var tag in Items)
            {
                _tagsById[tag.ID] = tag;
                _tagsByType[tag.GetType()] = tag;
            }
        }

        public bool TryGetTag(string id, out GameTag tag) => _tagsById.TryGetValue(id, out tag);

        public bool TryGetTag<T>(out T tag) where T : GameTag
        {
            if (_tagsByType.TryGetValue(typeof(T), out var result))
            {
                tag = result as T;
                return tag != null;
            }

            tag = null;
            return false;
        }

        public T GetTag<T>() where T : GameTag
        {
            if (TryGetTag(out T tag))
            {
                return tag;
            }

            this.LogError($"Tag of type {typeof(T).Name} not found.");
            return null;
        }

        public IReadOnlyList<GameTag> GetAll() => Items.ToList();
    }
}
