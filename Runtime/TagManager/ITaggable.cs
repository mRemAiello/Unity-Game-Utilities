using System;
using System.Collections.Generic;

namespace GameUtils
{
    public interface ITaggable
    {
        public IReadOnlyList<GameTag> Tags { get; }

        public IReadOnlyList<GameTag> GetTags() => Tags ?? Array.Empty<GameTag>();

        public bool HasAnyTag(string tag) => HasAnyTag(new[] { tag });

        public bool HasAnyTag(GameTag tag) => HasAnyTag(new[] { tag });

        public bool HasAnyTag(params GameTag[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return false;
            }

            return TagManager.HasAny(this, tags);
        }

        public bool HasAnyTag(params string[] tags)
        {
            var currentTags = Tags;
            if (currentTags == null || tags == null || tags.Length == 0)
            {
                return false;
            }

            foreach (var currentTag in currentTags)
            {
                if (currentTag == null)
                {
                    continue;
                }

                foreach (var tagId in tags)
                {
                    if (!string.IsNullOrEmpty(tagId) && currentTag.ID == tagId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasAllTag(string tag) => HasAllTag(new[] { tag });

        public bool HasAllTag(GameTag tag) => HasAllTag(new[] { tag });

        public bool HasAllTag(params GameTag[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return true;
            }

            return TagManager.HasAll(this, tags);
        }

        public bool HasAllTag(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return true;
            }

            var currentTags = Tags;
            if (currentTags == null || currentTags.Count == 0)
            {
                return false;
            }

            foreach (var tagId in tags)
            {
                if (string.IsNullOrEmpty(tagId))
                {
                    return false;
                }

                var found = false;
                foreach (var currentTag in currentTags)
                {
                    if (currentTag != null && currentTag.ID == tagId)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
