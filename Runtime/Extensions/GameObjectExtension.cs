using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public static class GameObjectExtension
    {
        public static T GetComponentForced<T>(this GameObject gameObject) where T : Component
        {
            T obj = gameObject.GetComponent<T>() ?? gameObject.GetComponentInChildren<T>();
            return obj;
        }

        public static IReadOnlyList<GameTag> GetTags(this GameObject gameObject)
        {
            var taggable = GetTaggable(gameObject);
            return taggable?.Tags ?? Array.Empty<GameTag>();
        }

        public static bool HasAnyTag(this GameObject gameObject, List<GameTag> tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && taggable.HasAnyTag(tags);
        }

        public static bool HasAnyTag(this GameObject gameObject, params GameTag[] tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && TagManager.HasAny(taggable, tags);
        }

        public static bool HasAnyTag(this GameObject gameObject, params string[] tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && TagListHasAny(taggable.Tags, tags);
        }

        public static bool HasAllTag(this GameObject gameObject, List<GameTag> tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && taggable.HasAllTag(tags);
        }

        public static bool HasAllTag(this GameObject gameObject, params GameTag[] tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && TagManager.HasAll(taggable, tags);
        }

        public static bool HasAllTag(this GameObject gameObject, params string[] tags)
        {
            var taggable = GetTaggable(gameObject);
            return taggable != null && TagListHasAll(taggable.Tags, tags);
        }

        private static ITaggable GetTaggable(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            return gameObject.GetComponent<ITaggable>() ?? gameObject.GetComponentInChildren<ITaggable>();
        }

        private static bool TagListHasAny(IReadOnlyList<GameTag> tags, IReadOnlyList<string> tagIds)
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

        private static bool TagListHasAll(IReadOnlyList<GameTag> tags, IReadOnlyList<string> tagIds)
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

        //
        public static bool HasAnyTag(this GameObject gameObject, string tag) => HasAnyTag(gameObject, new[] { tag });
        public static bool HasAnyTag(this GameObject gameObject, GameTag tag) => HasAnyTag(gameObject, new[] { tag });
        public static bool HasAllTag(this GameObject gameObject, string tag) => HasAllTag(gameObject, new[] { tag });
        public static bool HasAllTag(this GameObject gameObject, GameTag tag) => HasAllTag(gameObject, new[] { tag });
    }
}
