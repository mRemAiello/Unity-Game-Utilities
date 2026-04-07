using UnityEngine;

namespace GameUtils
{
    public abstract class SkillNodeData : ItemVisualData
    {
        /*public virtual bool CanUnlock(ISkillContext context)
        {
            // Check prerequisites (logica base, override se serve)
            foreach (var prereq in prerequisites)
            {
                if (!IsSkillUnlocked(context, prereq.id))
                    return false;
            }

            // Check currency se disponibile
            if (context.TryGet<ISkillPointHandler>(out var points))
            {
                if (!points.HasEnough(cost))
                    return false;
            }

            return true;
        }

        public virtual bool IsAvailable(ISkillContext context)
        {
            // Nodo root o almeno un prerequisito sbloccato
            if (prerequisites == null || prerequisites.Count == 0)
                return true;

            foreach (var prereq in prerequisites)
            {
                if (IsSkillUnlocked(context, prereq.id))
                    return true;
            }

            return false;
        }

        public virtual void OnUnlock(ISkillContext context)
        {
            foreach (var effect in effects)
            {
                effect?.Apply(context);
            }
        }

        public virtual void OnLock(ISkillContext context)
        {
            foreach (var effect in effects)
            {
                effect?.Remove(context);
            }
        }

        protected virtual bool IsSkillUnlocked(ISkillContext context, string skillID)
        {
            if (context.TryGet<ISkillStateProvider>(out var provider))
                return provider.IsUnlocked(skillID);

            return false;
        }*/
    }
}