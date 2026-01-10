using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("internal", Title = "Internal")]
    public abstract class ItemIdentifierData : ScriptableObject
    {
        [SerializeField, Group("internal"), ReadOnly] private string _id = "";

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                RegenerateID();
            }
        }

        [Button(ButtonSizes.Medium)]
        private void RegenerateID()
        {
#if UNITY_EDITOR
            _id = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
#endif
        }

        public override bool Equals(object other)
        {
            // 
            if ((other == null) || !GetType().Equals(other.GetType()))
            {
                return false;
            }

            var obj = other as ItemIdentifierData;
            return ID.Equals(obj.ID);
        }

        public static bool AreEquals(ItemIdentifierData firstElement, ItemIdentifierData secondElement)
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

        // Getters
        public string ID => _id;
    }
}