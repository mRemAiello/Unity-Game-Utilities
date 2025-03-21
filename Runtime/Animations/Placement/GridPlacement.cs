using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class GridPlacement : MonoBehaviour
    {
        [SerializeField] private int _rows = 2;
        [SerializeField] private int _columns = 2;
        [SerializeField] private float _spacing = 2f;

        [Button]
        public void PositionChildrenInGrid()
        {
            int currentRow = 0;
            int currentColumn = 0;

            // 
            foreach (Transform child in transform)
            {
                // 
                Vector3 newPosition = new(currentColumn * _spacing,child.position.y,currentRow * _spacing);

                //
                child.position = newPosition;

                //
                currentColumn++;
                if (currentColumn >= _columns)
                {
                    currentColumn = 0;
                    currentRow++;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            //
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    Vector3 gridPosition = new(col * _spacing,transform.position.y,row * _spacing);
                    Gizmos.DrawWireCube(gridPosition, new Vector3(1, 1, 1)); // Disegna un cubo wireframe per ogni punto
                }
            }
        }
    }
}