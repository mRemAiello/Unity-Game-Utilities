using System;

namespace GameUtils
{
    [Serializable]
    public class StatusEffectSaveData
    {
        public string ID;
        public int Duration;
        public int Intensity;
        public string SourcePath;
        public string TargetPath;
    }
}