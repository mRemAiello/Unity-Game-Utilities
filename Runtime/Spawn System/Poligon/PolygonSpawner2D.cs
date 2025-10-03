using System.Collections.Generic;
using UnityEngine;
using TriInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameUtils
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonSpawner2D : MonoBehaviour
    {
        [Header("Prefabs (weighted)")]
        public List<PrefabEntry> prefabs = new();

        [Header("Placement")]
        public int count = 100;
        public float minDistance = 1f;
        public int seed = 12345;
        public int maxSamplingAttempts = 100000;

        [Header("Transform randomization")]
        public Vector2 uniformScaleRange = new(0.8f, 1.2f);
        public bool randomizeRotationZ = true;
        public Vector2 rotationZRange = new(0f, 360f);

        [Header("Physics / Overlap")]
        public bool avoidPhysicsOverlap = false;
        public LayerMask overlapCheckLayers = ~0;
        public float overlapCheckRadius = 0.25f;

        [Header("Parenting")]
        public Transform parentContainer;
        public string containerName = "Generated_Instances";

        [Header("Gizmos / Preview")]
        public bool showPreview = false;
        public Color previewColor = new(0.2f, 1f, 0.2f, 0.7f);
        public int previewMaxPoints = 2000;

        // Internals
        private PolygonCollider2D _poly;
        private Vector2[] _worldPolyCache;

        private void OnValidate()
        {
            if (uniformScaleRange.x < 0f)
                uniformScaleRange.x = 0f;
            if (uniformScaleRange.y < uniformScaleRange.x)
                uniformScaleRange.y = uniformScaleRange.x;
            if (rotationZRange.y < rotationZRange.x)
                rotationZRange.y = rotationZRange.x;
            if (count < 0)
                count = 0;
            if (minDistance < 0f)
                minDistance = 0f;
            if (maxSamplingAttempts < count * 10)
                maxSamplingAttempts = Mathf.Max(count * 10, 1000);

            //
            CachePolygon();
        }

        private void Reset()
        {
            _poly = GetComponent<PolygonCollider2D>();
            CachePolygon();
        }

        private void CachePolygon()
        {
            _poly = _poly != null ? _poly : GetComponent<PolygonCollider2D>();
            if (_poly == null)
                return;

            var pts = _poly.points;
            _worldPolyCache = new Vector2[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                _worldPolyCache[i] = transform.TransformPoint(pts[i]);
            }
        }

        [Button]
        public void Generate()
        {
            if (!IsSetupValid(out string reason))
            {
                Debug.LogError($"[PolygonSpawner2D] Setup non valido: {reason}", this);
                return;
            }

            CachePolygon();

            // Random deterministico
            Random.InitState(seed);

            Bounds b = ComputeBounds(_worldPolyCache);
            float minDistSq = minDistance * minDistance;

            var placed = new List<Vector2>(count);
            int attempts = 0;

            while (placed.Count < count && attempts < maxSamplingAttempts)
            {
                attempts++;

                // Rejection sampling nel bounding box
                float x = Random.Range(b.min.x, b.max.x);
                float y = Random.Range(b.min.y, b.max.y);
                var p = new Vector2(x, y);

                if (!PointInPolygon(p, _worldPolyCache))
                    continue;

                // Distanza minima da tutti i punti posizionati
                bool ok = true;
                for (int i = 0; i < placed.Count; i++)
                {
                    if ((placed[i] - p).sqrMagnitude < minDistSq)
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                    continue;

                // Overlap fisico opzionale
                if (avoidPhysicsOverlap)
                {
                    var hit = Physics2D.OverlapCircle(p, overlapCheckRadius, overlapCheckLayers);
                    if (hit != null)
                        continue;
                }

                placed.Add(p);
            }

            if (placed.Count < count)
            {
                Debug.LogWarning($"[PolygonSpawner2D] Generati {placed.Count}/{count} punti (aumenta maxSamplingAttempts o riduci minDistance/Count).", this);
            }

            // Istanze
            var container = ResolveContainer(true);

#if UNITY_EDITOR
            Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "Generate Instances");
#endif

            for (int i = 0; i < placed.Count; i++)
            {
                var prefab = PickPrefab();
                if (prefab == null) continue;

#if UNITY_EDITOR
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, container);
                if (instance == null) instance = Instantiate(prefab, container);
                Undo.RegisterCreatedObjectUndo(instance, "Spawn Instance");
#else
            GameObject instance = Instantiate(prefab, container);
#endif

                instance.name = $"{prefab.name}_Spawned_{i:D4}";
                instance.transform.position = placed[i];

                // Rotazione Z casuale
                if (randomizeRotationZ)
                {
                    float z = Random.Range(rotationZRange.x, rotationZRange.y);
                    instance.transform.rotation = Quaternion.Euler(0f, 0f, z);
                }

                // Scala uniforme casuale
                float s = Random.Range(uniformScaleRange.x, uniformScaleRange.y);
                instance.transform.localScale = new Vector3(s, s, 1f);
            }

            Debug.Log($"[PolygonSpawner2D] Istanze generate: {placed.Count}. Tentativi: {attempts}.", this);
        }

        [Button]
        public void ClearGenerated()
        {
            Transform container = ResolveContainer(false);
            if (container == null)
                return;

            // Cancella tutti i figli
            var toDestroy = new List<GameObject>();
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                toDestroy.Add(container.GetChild(i).gameObject);
            }

#if UNITY_EDITOR
            Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "Clear Generated Instances");
            foreach (var go in toDestroy)
            {
                Undo.DestroyObjectImmediate(go);
            }
#else
        foreach (var go in toDestroy)
        {
            DestroyImmediate(go);
        }
#endif
        }

        private Transform ResolveContainer(bool createIfMissing)
        {
            if (parentContainer != null)
                return parentContainer;

            Transform found = transform.Find(containerName);
            if (found != null)
            {
                parentContainer = found;
                return parentContainer;
            }

            if (!createIfMissing)
                return null;

            var go = new GameObject(string.IsNullOrEmpty(containerName) ? "Generated_Instances" : containerName);
            go.transform.SetParent(transform, worldPositionStays: false);
            parentContainer = go.transform;
            return parentContainer;
        }

        private GameObject PickPrefab()
        {
            if (prefabs == null || prefabs.Count == 0)
                return null;

            // Random ponderato
            float total = 0f;
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i].prefab != null && prefabs[i].weight > 0f)
                    total += prefabs[i].weight;
            }
            if (total <= 0f)
                return null;

            float r = Random.value * total;
            float c = 0f;
            for (int i = 0; i < prefabs.Count; i++)
            {
                var e = prefabs[i];
                if (e.prefab == null || e.weight <= 0f) continue;
                c += e.weight;
                if (r <= c)
                    return e.prefab;
            }
            return prefabs[^1].prefab;
        }

        private bool IsSetupValid(out string reason)
        {
            CachePolygon();
            if (_poly == null)
            {
                reason = "Manca PolygonCollider2D.";
                return false;
            }
            if (_worldPolyCache == null || _worldPolyCache.Length < 3)
            {
                reason = "Il poligono deve avere almeno 3 punti.";
                return false;
            }
            if (prefabs == null || prefabs.Count == 0)
            {
                reason = "Nessun prefab assegnato.";
                return false;
            }
            reason = null;
            return true;
        }

        private static Bounds ComputeBounds(Vector2[] poly)
        {
            if (poly == null || poly.Length == 0) return new Bounds(Vector3.zero, Vector3.zero);
            Vector2 min = poly[0];
            Vector2 max = poly[0];
            for (int i = 1; i < poly.Length; i++)
            {
                min = Vector2.Min(min, poly[i]);
                max = Vector2.Max(max, poly[i]);
            }
            var b = new Bounds((min + max) * 0.5f, max - min);
            return b;
        }

        // Ray casting point-in-polygon
        public static bool PointInPolygon(Vector2 p, Vector2[] poly)
        {
            bool inside = false;
            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];

                bool intersect = ((pi.y > p.y) != (pj.y > p.y)) &&
                                 (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y + Mathf.Epsilon) + pi.x);
                if (intersect) inside = !inside;
            }
            return inside;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showPreview || _worldPolyCache == null || _worldPolyCache.Length < 3) return;

            Gizmos.color = previewColor;

            Bounds b = ComputeBounds(_worldPolyCache);
            int shown = 0;
            int attempts = 0;
            int maxAttempts = Mathf.Min(previewMaxPoints * 50, 100000);

            // semplice anteprima di punti validi (non rispetta minDistance, è solo per visibilità area)
            while (shown < previewMaxPoints && attempts < maxAttempts)
            {
                attempts++;
                float x = Random.Range(b.min.x, b.max.x);
                float y = Random.Range(b.min.y, b.max.y);
                var p = new Vector2(x, y);
                if (PointInPolygon(p, _worldPolyCache))
                {
                    Gizmos.DrawSphere(p, 0.03f);
                    shown++;
                }
            }
        }
    }
}
