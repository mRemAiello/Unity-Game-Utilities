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
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _trajectoryStartPoint;
        [SerializeField, Group("debug"), ReadOnly] private float _trajectoryMaxRelativeHeight;
        [SerializeField, Group("debug"), ReadOnly] private AnimationCurve _trajectoryCurve;
        [SerializeField, Group("debug"), ReadOnly] private AnimationCurve _axisCorrectCurve;

        //
        public void InitProjectile(Vector3 target, float moveSpeed, float trajectoryMaxHeight)
        {
            _target = target;
            _moveSpeed = moveSpeed;

            //
            float xDistanceToTarget = _target.x - transform.position.x;
            _trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * trajectoryMaxHeight; 
        }

        public void InitAnimationCurves(AnimationCurve trajectoryCurve, AnimationCurve axisCorrectCurve)
        {
            _trajectoryCurve = trajectoryCurve;
            _axisCorrectCurve = axisCorrectCurve;
        }

        void Start()
        {
            _trajectoryStartPoint = transform.position;
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
            float nextPositionX = transform.position.x + _moveSpeed * Time.deltaTime;
            float nextPositionXNormalized = (nextPositionX - _trajectoryStartPoint.x) / trajectoryRange.x;

            //
            float nextPositionYNormalized = _trajectoryCurve.Evaluate(nextPositionXNormalized);
            float nextPositionY = _trajectoryStartPoint.y + nextPositionYNormalized * _trajectoryMaxRelativeHeight;

            //
            transform.position = new Vector3(nextPositionX, nextPositionY, 0);
        }
    }
}