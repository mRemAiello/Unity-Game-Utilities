using UnityEngine;

namespace GameUtils
{
    public class RuntimeAchievement
    {
        private readonly AchievementData _data;
        private int _currentValue;
        private bool _isCompleted;

        public AchievementData Data => _data;
        public int CurrentValue => _currentValue;
        public bool IsCompleted => _isCompleted;

        public RuntimeAchievement(AchievementData data)
        {
            _data = data;
            _currentValue = 0;
            _isCompleted = false;
        }

        public void Complete()
        {
            if (!_isCompleted)
            {
                _isCompleted = true;
                _currentValue = _data.TargetValue;
                AchievementManager.Instance?.AchievementCompleted(this);
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
            if (_isCompleted)
            {
                return;
            }

            switch (_data.Type)
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

        private bool CheckCondition(int value)
        {
            return _data.Condition switch
            {
                AchievementCondition.Equal => value == _data.TargetValue,
                AchievementCondition.Greater => value > _data.TargetValue,
                AchievementCondition.Less => value < _data.TargetValue,
                AchievementCondition.GreaterOrEqual => value >= _data.TargetValue,
                AchievementCondition.LessOrEqual => value <= _data.TargetValue,
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

        public void ResetProgress()
        {
            _currentValue = 0;
            CheckCompletition();
        }

        public bool CheckCompletition()
        {
            bool isCompleted = _currentValue >= _data.TargetValue;
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
