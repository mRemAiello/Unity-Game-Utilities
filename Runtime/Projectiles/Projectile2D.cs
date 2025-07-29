using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("projectile", Title = "Projectile")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class Projectile2D : MonoBehaviour, IProjectile
    {
        [SerializeField, Group("projectile")] private float _thresholdToDestroy = 1f;

        //
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _target;
        [SerializeField, Group("debug"), ReadOnly] private float _moveSpeed;
        [SerializeField, Group("debug"), ReadOnly] private float _maxMoveSpeed;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _trajectoryStartPoint;
        [SerializeField, Group("debug"), ReadOnly] private float _trajectoryMaxRelativeHeight;
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
            Vector3 trajectoryRange = _target - _trajectoryStartPoint;

            // 
            if (trajectoryRange.x < 0)
            {
                _moveSpeed = -_moveSpeed;
            }

            //
            float nextX = transform.position.x + _moveSpeed * Time.deltaTime;
            float nextXNormalized = (nextX - _trajectoryStartPoint.x) / trajectoryRange.x;
            float nextYNormalized = _trajectoryCurve.Evaluate(nextXNormalized);
            float nextYCorrectionNormalized = _axisCorrectCurve.Evaluate(nextXNormalized);
            float nextYCorrectionAbsolute = nextYCorrectionNormalized * trajectoryRange.y;
            float nextY = _trajectoryStartPoint.y + nextYNormalized * _trajectoryMaxRelativeHeight + nextYCorrectionAbsolute;

            //
            Vector3 newPos = new(nextX, nextY, 0);
            CalculateNextProjectileSpeed(nextXNormalized);
            transform.position = newPos;
        }

        private void CalculateNextProjectileSpeed(float nextXNormalized)
        {
            float nextMoveSpeedNormalized = _projSpeedCurve.Evaluate(nextXNormalized);
            _moveSpeed = nextMoveSpeedNormalized * _maxMoveSpeed;
        }
    }
}