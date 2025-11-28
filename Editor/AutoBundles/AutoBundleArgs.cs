using System.Collections.Generic;

namespace UnityEditor.GameUtils
{
    public class AutoBundleArgs
    {
        public string CurrentPath;
        public int CurrentDepth;
        public int MaxDepth;
        public List<string> Result;
        public List<string> ExcludedFolders;
        public List<string> MergedFolders;
    }
}