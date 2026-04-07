using UnityEngine;
using UnityEditor;

namespace DungeonsOfSurvival
{
    public class NormalizeAnimationTimePrecise
    {
        [MenuItem("Assets/Normalize Animation To 1s (Precise)", false, 2002)]
        private static void NormalizeSelectedAnimations()
        {
            Object[] selectedObjects = Selection.objects;
            int modifiedCount = 0;

            foreach (Object obj in selectedObjects)
            {
                if (obj is AnimationClip clip)
                {
                    NormalizeClip(clip);
                    modifiedCount++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Normalized {modifiedCount} AnimationClip(s) (precise).");
        }

        private static void NormalizeClip(AnimationClip clip)
        {
            float maxTime = GetMaxTime(clip);
            if (maxTime <= 0f)
                return;

            float scale = 1f / maxTime;

            Undo.RecordObject(clip, "Normalize Animation Time Precise");

            // Curve float
            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in curveBindings)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                ScaleCurve(curve, scale);
                ForceLastKeyToOne(curve);
                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }

            // Curve object reference
            var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            foreach (var binding in objectBindings)
            {
                var keys = AnimationUtility.GetObjectReferenceCurve(clip, binding);

                for (int i = 0; i < keys.Length; i++)
                    keys[i].time *= scale;

                if (keys.Length > 0)
                    keys[^1].time = 1f;

                AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
            }

            // Animation Events
            var events = AnimationUtility.GetAnimationEvents(clip);
            for (int i = 0; i < events.Length; i++)
                events[i].time *= scale;

            if (events.Length > 0)
                events[^1].time = 1f;

            AnimationUtility.SetAnimationEvents(clip, events);

            EditorUtility.SetDirty(clip);
        }

        private static float GetMaxTime(AnimationClip clip)
        {
            float maxTime = 0f;

            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in curveBindings)
            {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                if (curve.length > 0)
                    maxTime = Mathf.Max(maxTime, curve.keys[^1].time);
            }

            var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            foreach (var binding in objectBindings)
            {
                var keys = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                if (keys.Length > 0)
                    maxTime = Mathf.Max(maxTime, keys[^1].time);
            }

            var events = AnimationUtility.GetAnimationEvents(clip);
            foreach (var e in events)
                maxTime = Mathf.Max(maxTime, e.time);

            return maxTime;
        }

        private static void ScaleCurve(AnimationCurve curve, float scale)
        {
            for (int i = 0; i < curve.length; i++)
            {
                var key = curve.keys[i];
                key.time *= scale;
                curve.MoveKey(i, key);
            }
        }

        private static void ForceLastKeyToOne(AnimationCurve curve)
        {
            if (curve.length == 0)
                return;

            var lastIndex = curve.length - 1;
            var key = curve.keys[lastIndex];
            key.time = 1f;
            curve.MoveKey(lastIndex, key);
        }

        [MenuItem("Assets/Normalize Animation To 1s (Precise)", true)]
        private static bool ValidateNormalize()
        {
            foreach (Object obj in Selection.objects)
            {
                if (obj is AnimationClip)
                    return true;
            }
            return false;
        }
    }
}