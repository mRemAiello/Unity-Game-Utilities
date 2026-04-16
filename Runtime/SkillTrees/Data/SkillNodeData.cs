using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Skill")]
    public abstract class SkillNodeData : ItemVisualData
    {
        [SerializeField, Group("Skill")] private int _cost = 1;
        [SerializeField, Group("Skill")] private List<SkillNodeData> _prerequisites = new();
        [SerializeField, Group("Skill")] private List<SkillEffectData> _effects = new();

        //
        public int Cost => _cost;
        public IReadOnlyList<SkillNodeData> Prerequisites => _prerequisites;
        public IReadOnlyList<SkillEffectData> Effects => _effects;

        //
        public virtual bool CanUnlock(ISkillContext context)
        {
            if (!IsAvailable(context))
                return false;

            // Check prerequisites
            foreach (var prereq in _prerequisites)
            {
                if (!IsSkillUnlocked(context, prereq.ID))
                    return false;
            }

            // Check currency
            if (context.TryGet<ISkillPointHandler>(out var points))
            {
                if (!points.HasEnough(_cost))
                    return false;
            }

            return true;
        }

        public virtual bool IsAvailable(ISkillContext context)
        {
            // Root node or at least one prerequisite unlocked
            if (_prerequisites == null || _prerequisites.Count == 0)
                return true;

            foreach (var prereq in _prerequisites)
            {
                if (IsSkillUnlocked(context, prereq.ID))
                    return true;
            }

            return false;
        }

        public virtual void OnUnlock(ISkillContext context)
        {
            foreach (var effect in _effects)
            {
                effect?.Apply(context);
            }
        }

        public virtual void OnLock(ISkillContext context)
        {
            foreach (var effect in _effects)
            {
                effect?.Remove(context);
            }
        }

        protected virtual bool IsSkillUnlocked(ISkillContext context, string skillID)
        {
            if (context.TryGet<ISkillStateProvider>(out var provider))
                return provider.IsUnlocked(skillID);

            return false;
        }
    }
}