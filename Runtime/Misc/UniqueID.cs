using UnityEditor;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public abstract class UniqueID : ScriptableObject
    {
        [Tab("Settings")]
        [SerializeField, ReadOnly] private string _id = "";

        private void OnValidate()
        {
#if UNITY_EDITOR
            // Imposto l'ID
            if (string.IsNullOrEmpty(_id))
            {
                _id = GUID.Generate().ToString();
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public override bool Equals(object other)
        {
            // Se e' null o di tipo diverso, false
            if ((other == null) || !GetType().Equals(other.GetType()))
            {
                return false;
            }

            var obj = other as UniqueID;
            return ID.Equals(obj.ID);
        }

        public static bool AreEquals(UniqueID firstElement, UniqueID secondElement)
        {
            // If both are not null and have matching IDs
            if (firstElement != null && secondElement != null)
            {
                return firstElement.ID == secondElement.ID;
            }

            // Otherwise, return false
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Load()
        {
        }

        // Getters
        public string ID => _id;
    }
}