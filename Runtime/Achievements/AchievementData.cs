using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Graphics")]
    [DeclareBoxGroup("Progress")]
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ACHIEVEMENT_NAME + "Achievement")]
    public class AchievementData : ItemVisualData
    {
        [SerializeField, Group("internal")] private string _eventName;
        [SerializeField, Group("internal")] private string _steamId;
        [SerializeField, Group("internal")] private string _epicId;
        [SerializeField, Group("internal")] private string _playstationId;
        [SerializeField, Group("internal")] private string _xboxId;
        [SerializeField, Group("internal")] private string _androidId;
        [SerializeField, Group("internal")] private string _iosId;
        [SerializeField, Group("internal")] private string _appGalleryId;

        //
        [SerializeField, Group("Graphics")] private Sprite _uncompletedIcon;
        [SerializeField, Group("Graphics")] private Sprite _completedIcon;

        //
        [SerializeField, Group("Progress")] private AchievementType _type;
        [SerializeField, ShowIf(nameof(_type), AchievementType.Simple), Group("Progress")] private AchievementCondition _condition;
        [SerializeField, Group("Progress")] private int _targetValue;

        //
        public string EventName => _eventName;
        public string SteamId => _steamId;
        public string EpicId => _epicId;
        public string PlaystationId => _playstationId;
        public string XboxId => _xboxId;
        public string AndroidId => _androidId;
        public string IosId => _iosId;
        public string AppGalleryId => _appGalleryId;
        public Sprite UncompletedIcon => _uncompletedIcon;
        public Sprite CompletedIcon => _completedIcon;
        public AchievementType Type => _type;
        public AchievementCondition Condition => _condition;
        public int TargetValue => _targetValue;
    }
}
