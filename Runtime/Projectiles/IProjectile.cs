using UnityEngine;

namespace GameUtils
{
    public interface IProjectile
    {
        void InitProjectile(Vector3 target, float moveSpeed, float trajectoryMaxHeight);
        void InitAnimationCurves(AnimationCurve trajectoryCurve, AnimationCurve axisCorrectCurve);
    }
}