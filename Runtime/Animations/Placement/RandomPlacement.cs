using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class RandomPlacement : MonoBehaviour
    {
        //
        public Vector3 placementArea = new(10, 0, 10);
        public float maxAttempts = 10;

        [Button]
        public void PositionChildrenRandomly()
        {
            Collider[] colliders = new Collider[4];

            // 
            foreach (Transform child in transform)
            {
                bool placed = false;
                int attempts = 0;

                while (!placed && attempts < maxAttempts)
                {
                    //
                    float x = Random.Range(-placementArea.x / 2, placementArea.x / 2);
                    float z = Random.Range(-placementArea.z / 2, placementArea.z / 2);
                    Vector3 randomPosition = new(x, child.position.y, z);

                    //
                    child.position = randomPosition;
                    attempts++;

                    //
                    if (Physics.OverlapSphereNonAlloc(child.position, child.GetComponent<Collider>().bounds.extents.magnitude, colliders) == 0)
                    {
                        placed = true;
                    }
                }

                if (!placed)
                {
                    Debug.LogWarning($"{child.name} non ha trovato una posizione senza collisioni dopo {maxAttempts} tentativi.");
                }
            }
        }

        private void OnDrawGizmos()
        {
            //
            Gizmos.color = Color.green;

            //
            Gizmos.DrawWireCube(transform.position, new Vector3(placementArea.x, 0.1f, placementArea.z));
        }
    }
}