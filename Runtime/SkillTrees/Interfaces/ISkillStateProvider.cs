using System.Collections.Generic;

namespace GameUtils
{
    public interface ISkillStateProvider
    {
        bool IsUnlocked(string skillID);
        IReadOnlyCollection<string> GetUnlockedSkillIDs();
    }
}