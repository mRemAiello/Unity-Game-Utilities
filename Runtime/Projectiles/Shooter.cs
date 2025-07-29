using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("projectile", Title = "Projectile")]
    [DeclareBoxGroup("curves", Title = "Curves")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class Shooter : MonoBehaviour
    {
        [SerializeField, Group("projectile")] private GameObject _projPrefab;
        [SerializeField, Group("projectile")] private Transform _target;
        [SerializeField, Group("projectile")] private float _shootRate;
        [SerializeField, Group("projectile")] private float _projMoveSpeed;
        [SerializeField, Group("projectile")] private float _projMaxHeight;

        //
        [SerializeField, Group("curves")] private AnimationCurve _trajectoryCurve;
        [SerializeField, Group("curves")] private AnimationCurve _axisCorrectionCurve;

        //
        [SerializeField, Group("debug"), ReadOnly] private float _shootTimer;

        //
        void Update()
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer > 0)
                return;

            //
            _shootTimer = _shootRate;
            GameObject projGameObject = Instantiate(_projPrefab, transform.position, Quaternion.identity);
            if (projGameObject.TryGetComponent(out IProjectile projectile))
            {
                projectile.InitProjectile(_target.position, _projMoveSpeed, _projMaxHeight);
                projectile.InitAnimationCurves(_trajectoryCurve, _axisCorrectionCurve);
            }
        }
    }
}