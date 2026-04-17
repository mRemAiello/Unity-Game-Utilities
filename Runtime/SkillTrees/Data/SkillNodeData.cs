using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Skill")]
    public abstract class SkillNodeData : ItemVisualData
    {
        [SerializeField, Group("Skill")] private CurrencyData _currency;
        [SerializeField, Group("Skill")] private int _costPerLevel = 1;
        [SerializeField, Group("Skill")] private int _maxLevel = 1;
        [SerializeField, Group("Skill")] private List<SkillEffectData> _effects = new();

        //
        public CurrencyData Currency => _currency;
        public int CostPerLevel => _costPerLevel;
        public int MaxLevel => _maxLevel;
        public IReadOnlyList<SkillEffectData> Effects => _effects;

        //
        public int GetCostForLevel(int level) => _costPerLevel * level;

        public virtual bool CanLevelUp(int currentLevel)
        {
            if (currentLevel >= _maxLevel)
                return false;

            // Check currency
            if (_currency != null && CurrencyManager.InstanceExists)
            {
                int cost = GetCostForLevel(currentLevel + 1);
                if (CurrencyManager.Instance.GetCurrencyAmount(_currency) < cost)
                    return false;
            }

            return true;
        }

        public virtual void OnLevelUp(ISkillContext context, int newLevel)
        {
            foreach (var effect in _effects)
            {
                effect?.Apply(context, newLevel);
            }
        }

        public virtual void OnLevelDown(ISkillContext context, int newLevel)
        {
            foreach (var effect in _effects)
            {
                effect?.Remove(context, newLevel);
            }
        }


    }
}