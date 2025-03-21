using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class PlaceInCircleUI : MonoBehaviour
    {
        // 
        [SerializeField] private float _radius = 1.0f;

        //
        [SerializeField] private GameObject[] _items;

        [Button]
        public void Refresh()
        {
            if (_items == null)
                return;

            // 
            float angle = 360.0f / _items.Length;

            // 
            for (int i = 0; i < _items.Length; i++)
            {
                // 
                float theta = i * angle * Mathf.Deg2Rad;

                // 
                float x = _radius * Mathf.Cos(theta);
                float y = _radius * Mathf.Sin(theta);

                // 
                Vector3 position = new(x, y, 0);
                _items[i].transform.position = position;
            }
        }
    }
}