using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("projectile", Title = "Projectile")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class Projectile2D : MonoBehaviour, IProjectile
    {
        [SerializeField, Group("projectile")] private float _thresholdToDestroy = 1f;
        [SerializeField, Group("projectile")] private ProjectileVisual2D _projectileVisual;

        //
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _target;
        [SerializeField, Group("debug"), ReadOnly] private float _moveSpeed;
        [SerializeField, Group("debug"), ReadOnly] private float _maxMoveSpeed;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _trajectoryStartPoint;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _trajectoryRange;
        [SerializeField, Group("debug"), ReadOnly] private float _trajectoryMaxRelativeHeight;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _projectileMoveDir;
        [SerializeField, Group("debug"), ReadOnly] private float _nextYTrajectoryPosition;
        [SerializeField, Group("debug"), ReadOnly] private float _nextXTrajectoryPosition;
        [SerializeField, Group("debug"), ReadOnly] private float _nextPositionYCorrectionAbsolute;
        [SerializeField, Group("debug"), ReadOnly] private float _nextPositionXCorrectionAbsolute;
        [SerializeField, Group("debug"), ReadOnly] private AnimationCurve _trajectoryCurve;
        [SerializeField, Group("debug"), ReadOnly] private AnimationCurve _axisCorrectCurve;
        [SerializeField, Group("debug"), ReadOnly] private AnimationCurve _projSpeedCurve;

        //
        void Start()
        {
            _trajectoryStartPoint = transform.position;
        }

        public void InitProjectile(Vector3 target, float maxMoveSpeed, float trajectoryMaxHeight)
        {
            _target = target;
            _maxMoveSpeed = maxMoveSpeed;

            //
            float xDistanceToTarget = _target.x - transform.position.x;
            _trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * trajectoryMaxHeight;

            //
            _projectileVisual.SetTarget(_target);
        }

        public void InitAnimationCurves(AnimationCurve trajectoryCurve, AnimationCurve axisCorrectCurve, AnimationCurve projSpeedCurve)
        {
            _trajectoryCurve = trajectoryCurve;
            _axisCorrectCurve = axisCorrectCurve;
            _projSpeedCurve = projSpeedCurve;
        }

        void Update()
        {
            //
            UpdateProjectilePosition();
            if (Vector3.Distance(transform.position, _target) < _thresholdToDestroy)
            {
                Destroy(gameObject);
                // TODO: Evento
            }
        }

        private void UpdateProjectilePosition()
        {
            _trajectoryRange = _target - _trajectoryStartPoint;
            if (Mathf.Abs(_trajectoryRange.normalized.x) < Mathf.Abs(_trajectoryRange.normalized.y))
            {
                // Projectile will be curved on the X axis
                if (_trajectoryRange.y < 0)
                {
                    // Target is located under shooter
                    _moveSpeed = -_moveSpeed;
                }

                UpdatePositionWithXCurve();
            }
            else
            {
                // Projectile will be curved on the Y axis
                if (_trajectoryRange.x < 0)
                {
                    // Target is located behind shooter
                    _moveSpeed = -_moveSpeed;
                }

                UpdatePositionWithYCurve();
            }
        }

        private void UpdatePositionWithXCurve()
        {
            if (_trajectoryRange.y != 0)
            {
                float nextY = transform.position.y + _moveSpeed * Time.deltaTime;
                float nextPositionYNormalized = (nextY - _trajectoryStartPoint.y) / _trajectoryRange.y;

                //
                float nextXNormalized = _trajectoryCurve.Evaluate(nextPositionYNormalized);
                _nextXTrajectoryPosition = nextXNormalized * _trajectoryMaxRelativeHeight;

                //
                float nextPositionXCorrectionNormalized = _axisCorrectCurve.Evaluate(nextPositionYNormalized);
                _nextPositionXCorrectionAbsolute = nextPositionXCorrectionNormalized * _trajectoryRange.x;

                //
                if (_trajectoryRange.x > 0 && _trajectoryRange.y > 0)
                {
                    _nextXTrajectoryPosition = -_nextXTrajectoryPosition;
                }

                //
                if (_trajectoryRange.x < 0 && _trajectoryRange.y < 0)
                {
                    _nextXTrajectoryPosition = -_nextXTrajectoryPosition;
                }

                //
                float nextPositionX = _trajectoryStartPoint.x + _nextXTrajectoryPosition + _nextPositionXCorrectionAbsolute;
                Vector3 newPos = new(nextPositionX, nextY, 0);
                CalculateNextProjectileSpeed(nextPositionYNormalized);

                //
                _projectileMoveDir = newPos - transform.position;
                transform.position = newPos;
            }
            else
            {
                // Fallback to linear movement when there's no vertical range.
                _nextXTrajectoryPosition = 0f;
                _nextPositionXCorrectionAbsolute = 0f;
                Vector3 dir = (_target - transform.position).normalized;
                Vector3 newPos = transform.position + dir * (_moveSpeed * Time.deltaTime);
                _projectileMoveDir = newPos - transform.position;
                transform.position = newPos;
            }
        }

        private void UpdatePositionWithYCurve()
        {
            if (_trajectoryRange.x != 0)
            {
                //
                float nextX = transform.position.x + _moveSpeed * Time.deltaTime;
                float nextXNormalized = (nextX - _trajectoryStartPoint.x) / _trajectoryRange.x;
                float nextYNormalized = _trajectoryCurve.Evaluate(nextXNormalized);
                float nextYCorrectionNormalized = _axisCorrectCurve.Evaluate(nextXNormalized);
                float nextYCorrectionAbsolute = nextYCorrectionNormalized * _trajectoryRange.y;
                _nextYTrajectoryPosition = nextYNormalized * _trajectoryMaxRelativeHeight;
                _nextPositionYCorrectionAbsolute = nextYCorrectionAbsolute;
                float nextY = _trajectoryStartPoint.y + _nextYTrajectoryPosition + _nextPositionYCorrectionAbsolute;

                //
                Vector3 newPos = new(nextX, nextY, 0);
                CalculateNextProjectileSpeed(nextXNormalized);

                //
                _projectileMoveDir = newPos - transform.position;
                transform.position = newPos;
            }
            else
            {
                // Fallback to linear movement when there's no horizontal range.
                _nextYTrajectoryPosition = 0f;
                _nextPositionYCorrectionAbsolute = 0f;
                Vector3 dir = (_target - transform.position).normalized;
                Vector3 newPos = transform.position + dir * (_moveSpeed * Time.deltaTime);
                _projectileMoveDir = newPos - transform.position;
                transform.position = newPos;
            }
        }

        private void CalculateNextProjectileSpeed(float nextXNormalized)
        {
            float nextMoveSpeedNormalized = _projSpeedCurve.Evaluate(nextXNormalized);
            _moveSpeed = nextMoveSpeedNormalized * _maxMoveSpeed;
        }

        //
        public Vector3 GetProjectileMoveDir() => _projectileMoveDir;
        public float GetNextYTrajectoryPosition() => _nextYTrajectoryPosition;
        public float GetNextPositionYCorrectionAbsolute() => _nextPositionYCorrectionAbsolute;
        public float GetNextXTrajectoryPosition() => _nextXTrajectoryPosition;
        public float GetNextPositionXCorrectionAbsolute() => _nextPositionXCorrectionAbsolute;
    }
}