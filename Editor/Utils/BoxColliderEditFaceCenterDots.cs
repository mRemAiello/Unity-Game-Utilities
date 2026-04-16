using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameUtils.Editor
{
    /// <summary>
    /// While Unity’s built-in box collider edit tool is active, draws camera-facing 
    /// discs at each face center (X red, Y blue, Z green, alpha 1). 
    /// </summary>
    [InitializeOnLoad]
    static class BoxColliderEditFaceCenterDots
    {
        static readonly Color FaceColorX = new(1f, 0f, 0f, 1f);
        static readonly Color FaceColorY = new(0f, 1f, 0f, 1f);
        static readonly Color FaceColorZ = new(0f, 0f, 1f, 1f);
        const float DotSizeFactor = 0.09f;

        static Type _boxPrimitiveColliderToolType;

        static BoxColliderEditFaceCenterDots() => SceneView.duringSceneGui += OnSceneGUI;

        static void OnSceneGUI(SceneView view)
        {
            if (!IsBoxPrimitiveColliderToolActive())
                return;

            var targets = CollectSelectedBoxColliders();
            if (targets.Count == 0)
                return;

            var cam = view.camera;

            var prevZ = Handles.zTest;
            Handles.zTest = CompareFunction.Always;

            foreach (var box in targets)
            {
                if (box == null)
                    continue;
                DrawFaceCenterDots(box, cam);
            }

            Handles.zTest = prevZ;
        }

        static void DrawFaceCenterDots(BoxCollider box, Camera cam)
        {
            Transform t = box.transform;
            Vector3 c = box.center;
            Vector3 h = box.size * 0.5f;
            (Vector3 local, Color color)[] faces = {
            (c + new Vector3(h.x, 0f, 0f), FaceColorX),
            (c + new Vector3(-h.x, 0f, 0f), FaceColorX),
            (c + new Vector3(0f, h.y, 0f), FaceColorY),
            (c + new Vector3(0f, -h.y, 0f), FaceColorY),
            (c + new Vector3(0f, 0f, h.z), FaceColorZ),
            (c + new Vector3(0f, 0f, -h.z), FaceColorZ),
        };

            foreach (var (local, color) in faces)
            {
                Vector3 world = t.TransformPoint(local);
                float radius = HandleUtility.GetHandleSize(world) * DotSizeFactor * 0.75f;
                Vector3 normal = cam != null ? (cam.transform.position - world).normalized : Vector3.forward;
                if (normal.sqrMagnitude < 1e-10f)
                    normal = Vector3.up;
                Handles.color = color;
                Handles.DrawSolidDisc(world, normal, radius);
            }
        }

        static List<BoxCollider> CollectSelectedBoxColliders()
        {
            var set = new HashSet<BoxCollider>();
            foreach (var o in Selection.objects)
            {
                switch (o)
                {
                    case BoxCollider b:
                        set.Add(b);
                        break;
                    case GameObject go:
                        if (go.TryGetComponent(out BoxCollider b2))
                            set.Add(b2);
                        break;
                }
            }

            return new List<BoxCollider>(set);
        }

        static bool IsBoxPrimitiveColliderToolActive()
        {
            if (_boxPrimitiveColliderToolType == null)
                _boxPrimitiveColliderToolType = FindBoxPrimitiveColliderToolType();
            return _boxPrimitiveColliderToolType != null && ToolManager.activeToolType == _boxPrimitiveColliderToolType;
        }

        static Type FindBoxPrimitiveColliderToolType()
        {
            var t = Type.GetType("UnityEditor.BoxPrimitiveColliderTool, UnityEditor");
            if (t != null)
                return t;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetType("UnityEditor.BoxPrimitiveColliderTool");
                if (t != null)
                    return t;
            }

            return null;
        }
    }
}