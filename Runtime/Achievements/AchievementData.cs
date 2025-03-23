using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    [DeclareBoxGroup("progress", Title = "Progress")]
    [CreateAssetMenu(menuName = "GD/Achievements/Base")]
    public class AchievementData : UniqueID
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
        [SerializeField, Group("graphics")] private string _name;
        [SerializeField, Group("graphics")] private string _description;
        [SerializeField, Group("graphics")] private Sprite _uncompletedIcon;
        [SerializeField, Group("graphics")] private Sprite _completedIcon;

        //
        [SerializeField, Group("progress")] private AchievementType _type;
        [SerializeField, ShowIf(nameof(_type), AchievementType.Simple), Group("progress")] private AchievementCondition _condition;
        [SerializeField, Group("progress")] private int _targetValue;
        [SerializeField, Group("progress")] private int _currentValue;
        [SerializeField, Group("progress")] private bool _isCompleted;

        //
        public string EventName => _eventName;
        public string SteamId => _steamId;
        public string EpicId => _epicId;
        public string PlaystationId => _playstationId;
        public string XboxId => _xboxId;
        public string AndroidId => _androidId;
        public string IosId => _iosId;
        public string AppGalleryId => _appGalleryId;
        public string Name => _name;
        public string Description => _description;
        public Sprite UncompletedIcon => _uncompletedIcon;
        public Sprite CompletedIcon => _completedIcon;
        public AchievementType Type => _type;
        public int TargetValue => _targetValue;
        public int CurrentValue => _currentValue;
        public bool IsCompleted => _isCompleted;

        //
        public void Complete()
        {
            if (!_isCompleted)
            {
                _isCompleted = true;
                AchievementManager.Instance?.AchievementCompleted(this);
                _currentValue = _targetValue;
            }
        }

        public void Uncomplete()
        {
            if (_isCompleted)
            {
                _isCompleted = false;
                AchievementManager.Instance?.AchievementUncompleted(this);
            }
        }

        public void UpdateState(int value)
        {
            if (!_isCompleted)
            {
                switch (_type)
                {
                    case AchievementType.Simple:
                        if (CheckCondition(value))
                        {
                            Complete();
                        }
                        break;

                    case AchievementType.Progress:
                        IncrementProgress(value);
                        break;

                    default:
                        break;
                }
            }
        }

        private bool CheckCondition(int value)
        {
            return _condition switch
            {
                AchievementCondition.Equal => value == _targetValue,
                AchievementCondition.Greater => value > _targetValue,
                AchievementCondition.Less => value < _targetValue,
                AchievementCondition.GreaterOrEqual => value >= _targetValue,
                AchievementCondition.LessOrEqual => value <= _targetValue,
                _ => false,
            };
        }

        public void IncrementProgress(int value)
        {
            _currentValue += value;

            CheckCompletition();
        }

        public void SetProgress(int value)
        {
            _currentValue = value;

            CheckCompletition();
        }

        [Button]
        public void ResetProgress()
        {
            _currentValue = 0;

            CheckCompletition();
        }

        public bool CheckCompletition()
        {
            bool isCompleted = _currentValue >= _targetValue;

            if (isCompleted)
            {
                Complete();
            }
            else if (_isCompleted)
            {
                Uncomplete();
            }

            return isCompleted;
        }
    }
}