using UnityEngine;
using VInspector;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "GD/Achievement", order = 99)]
    public class AchievementData : UniqueID
    {
        [Tab("General")]
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _uncompletedIcon;
        [SerializeField] private Sprite _completedIcon;

        [Tab("Identification")]
        [SerializeField] private string _eventName;
        [SerializeField] private string _id;
        [SerializeField] private string _steamId;
        [SerializeField] private string _epicId;
        [SerializeField] private string _playstationId;
        [SerializeField] private string _xboxId;
        [SerializeField] private string _androidId;
        [SerializeField] private string _iosId;
        [SerializeField] private string _appGalleryId;

        [Tab("Progress")]
        [SerializeField] private AchievementType _type;

        //
        [ShowIf("_type", AchievementType.Simple)]
        [SerializeField] private AchievementCondition _condition;
        [EndIf]

        //
        [SerializeField] private int _targetValue;
        [SerializeField] private int _currentValue;
        [SerializeField] private bool _isCompleted;

        //
        public string EventName => _eventName;
        public string Id => _id;
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