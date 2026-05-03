using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Nodes")]
    [DeclareBoxGroup("Events")]
    [DeclareBoxGroup("Debug")]
    [DefaultExecutionOrder(-100)]
    // TODO: ISaveable
    public class SkillTreeManager : Singleton<SkillTreeManager>, ISkillStateProvider
    {
        [SerializeField, Group("Nodes")] private List<RuntimeSkillNode> _nodes = new();
        [SerializeField, Group("Events")] private ClickSkillEventAsset _onLevelUpRequest;
        [SerializeField, Group("Events")] private ClickSkillEventAsset _onLevelDownRequest;
        [SerializeField, Group("Events")] private ChangeSkillStateEventAsset _onSkillChanged;
        [SerializeField, ReadOnly, Group("Debug")] private SerializedDictionary<string, int> _skillLevels = new();

        //
        private BaseSkillContext _context;

        //
        public string ID => "SkillTree";
        public IReadOnlyList<RuntimeSkillNode> Nodes => _nodes;

        //
        protected override void OnPostAwake()
        {
            _context = new BaseSkillContext();
            _context.Add<ISkillStateProvider>(this);

            Load();
            RefreshAllNodes();
        }

        private void OnEnable()
        {
            if (_onLevelUpRequest != null)
                _onLevelUpRequest.AddListener(this, OnLevelUpRequested);

            if (_onLevelDownRequest != null)
                _onLevelDownRequest.AddListener(this, OnLevelDownRequested);
        }

        private void OnDisable()
        {
            if (_onLevelUpRequest != null)
                _onLevelUpRequest.RemoveListener(this, OnLevelUpRequested);

            if (_onLevelDownRequest != null)
                _onLevelDownRequest.RemoveListener(this, OnLevelDownRequested);
        }

        //
        public bool TryLevelUp(RuntimeSkillNode node)
        {
            if (node == null || node.Data == null)
                return false;

            var data = node.Data;
            int currentLevel = GetLevel(data.ID);

            if (!node.ArePrerequisitesMet())
                return false;

            if (node.IsBlockedByExclusiveChoice())
                return false;

            if (!data.CanLevelUp(currentLevel))
                return false;

            // Spend currency
            int cost = data.GetCostForLevel(currentLevel + 1);
            if (data.Currency != null && CurrencyManager.InstanceExists)
            {
                if (!CurrencyManager.Instance.TryRemoveCurrency(data.Currency, cost))
                    return false;
            }

            // Apply level up
            int newLevel = currentLevel + 1;
            _skillLevels[data.ID] = newLevel;
            data.OnLevelUp(_context, newLevel);

            SaveSkill(data.ID);
            RefreshAllNodes();

            _onSkillChanged?.Invoke(node, node.State, newLevel);
            return true;
        }

        public bool TryLevelDown(RuntimeSkillNode node)
        {
            if (node == null || node.Data == null)
                return false;

            var data = node.Data;
            int currentLevel = GetLevel(data.ID);

            if (currentLevel <= 0)
                return false;

            // Check if any dependent node would become invalid
            if (currentLevel == 1 && HasDependentUnlockedNodes(node))
                return false;

            // Refund currency
            int refund = data.GetCostForLevel(currentLevel);
            if (data.Currency != null && CurrencyManager.InstanceExists)
                CurrencyManager.Instance.AddCurrency(data.Currency, refund);

            // Apply level down
            data.OnLevelDown(_context, currentLevel);
            int newLevel = currentLevel - 1;
            _skillLevels[data.ID] = newLevel;

            if (newLevel == 0)
                _skillLevels.Remove(data.ID);

            SaveSkill(data.ID);
            RefreshAllNodes();

            _onSkillChanged?.Invoke(node, node.State, newLevel);
            return true;
        }

        [Button(ButtonSizes.Medium)]
        public void ResetAll()
        {
            // Level down all nodes from highest to lowest to properly remove effects
            var unlockedNodes = _nodes
                .Where(n => GetLevel(n.Data.ID) > 0)
                .OrderByDescending(n => GetLevel(n.Data.ID))
                .ToList();

            foreach (var node in unlockedNodes)
            {
                int level = GetLevel(node.Data.ID);
                for (int i = level; i > 0; i--)
                {
                    // Refund currency
                    int refund = node.Data.GetCostForLevel(i);
                    if (node.Data.Currency != null && CurrencyManager.InstanceExists)
                        CurrencyManager.Instance.AddCurrency(node.Data.Currency, refund);

                    // Remove effects
                    node.Data.OnLevelDown(_context, i);
                }
            }

            // Clear all levels
            var ids = _skillLevels.Keys.ToList();
            _skillLevels.Clear();

            foreach (var id in ids)
                RemoveSavedSkill(id);

            RefreshAllNodes();
        }

        //
        public bool IsUnlocked(string skillID)
        {
            return _skillLevels.TryGetValue(skillID, out int level) && level > 0;
        }

        public int GetLevel(string skillID)
        {
            return _skillLevels.TryGetValue(skillID, out int level) ? level : 0;
        }

        public IReadOnlyCollection<string> GetUnlockedSkillIDs()
        {
            return _skillLevels.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();
        }

        //
        private void RefreshAllNodes()
        {
            foreach (var node in _nodes)
            {
                node.RefreshState(_context);
            }
        }

        private bool HasDependentUnlockedNodes(RuntimeSkillNode node)
        {
            foreach (var other in _nodes)
            {
                if (other == node) continue;
                if (GetLevel(other.Data.ID) <= 0) continue;

                foreach (var prereq in other.PrerequisiteNodes)
                {
                    if (prereq == node)
                    {
                        // Check if this was the only unlocked prerequisite
                        bool hasOtherUnlockedPrereq = other.PrerequisiteNodes
                            .Any(p => p != node && (p.State == SkillNodeState.Unlocked || p.State == SkillNodeState.Maxed));

                        if (!hasOtherUnlockedPrereq)
                            return true;
                    }
                }
            }

            return false;
        }

        //
        private void OnLevelUpRequested(RuntimeSkillNode node)
        {
            TryLevelUp(node);
        }

        private void OnLevelDownRequested(RuntimeSkillNode node)
        {
            TryLevelDown(node);
        }

        //
        private void SaveSkill(string skillID)
        {
            if (!GameSaveManager.InstanceExists)
                return;

            // Only save if level > 0 to avoid cluttering save with locked skills
            int level = GetLevel(skillID);
            GameSaveManager.Instance.Save(ID, skillID, level);
        }

        private void RemoveSavedSkill(string skillID)
        {
            if (!GameSaveManager.InstanceExists)
                return;

            // Only save if level > 0 to avoid cluttering save with locked skills
            GameSaveManager.Instance.RemoveKey<int>(ID, skillID);
        }

        public void Save()
        {
            if (!GameSaveManager.InstanceExists)
                return;

            foreach (var skillID in _skillLevels.Keys.ToList())
            {
                SaveSkill(skillID);
            }
        }

        public void Load()
        {
            if (!GameSaveManager.InstanceExists)
                return;

            // Clear current levels before loading to avoid stale data
            _skillLevels.Clear();
            foreach (var key in GameSaveManager.Instance.GetKeys())
            {
                if (!key.StartsWith(ID))
                    continue;

                string skillID = key.Replace($"{ID}-", "").Replace("-Int32", "");
                int level = GameSaveManager.Instance.Load(ID, skillID, 0);

                if (level > 0)
                    _skillLevels[skillID] = level;
            }
        }
    }
}