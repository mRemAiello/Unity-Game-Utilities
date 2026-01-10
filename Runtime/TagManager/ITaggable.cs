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

        public bool HasAnyTag(List<GameTag> tags) => HasAnyTag(tags?.ToArray());

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
            return TagListHasAny(Tags, tags);
        }

        public bool HasAllTag(string tag) => HasAllTag(new[] { tag });

        public bool HasAllTag(GameTag tag) => HasAllTag(new[] { tag });

        public bool HasAllTag(List<GameTag> tags) => HasAllTag(tags?.ToArray());

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
            return TagListHasAll(Tags, tags);
        }

        public static bool TagListHasAny(IReadOnlyList<GameTag> tags, IReadOnlyList<string> tagIds)
        {
            if (tags == null || tagIds == null || tagIds.Count == 0)
            {
                return false;
            }

            foreach (var tag in tags)
            {
                if (tag == null)
                {
                    continue;
                }

                foreach (var tagId in tagIds)
                {
                    if (!string.IsNullOrEmpty(tagId) && tag.ID == tagId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool TagListHasAll(IReadOnlyList<GameTag> tags, IReadOnlyList<string> tagIds)
        {
            if (tagIds == null || tagIds.Count == 0)
            {
                return true;
            }

            if (tags == null || tags.Count == 0)
            {
                return false;
            }

            foreach (var tagId in tagIds)
            {
                if (string.IsNullOrEmpty(tagId))
                {
                    return false;
                }

                var found = false;
                foreach (var tag in tags)
                {
                    if (tag != null && tag.ID == tagId)
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
