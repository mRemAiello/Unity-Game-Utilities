using System.Collections.Generic;

namespace GameUtils
{
    public interface ISkillStateProvider
    {
        bool IsUnlocked(string skillID);
        int GetLevel(string skillID);
        IReadOnlyCollection<string> GetUnlockedSkillIDs();
    }
}