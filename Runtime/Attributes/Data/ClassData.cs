using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameUtils
{
    /// <summary>
    /// Defines a class loadout by pairing attribute data with starting values.
    /// </summary>
    [CreateAssetMenu(menuName = GUConstants.ATTRIBUTES_NAME + "Class")]
    [DeclareBoxGroup("Class")]
    public class ClassData : ItemVisualData
    {
        [SerializeField, Group("Class")] private bool _loadAllAttributes = false;
        [SerializeField, Group("Class"), TableList] private List<AttributeValuePair> _attributes;

        //
        public bool LoadAllAttributes => _loadAllAttributes;
        public IReadOnlyList<AttributeValuePair> Attributes => _attributes;

        /// <summary>
        /// Aligns this class attribute list with the provided blueprint.
        /// </summary>
        [Button(ButtonSizes.Medium)]
        public void ApplyBlueprint(AttributeBlueprintData blueprint)
        {
#if UNITY_EDITOR
            // Abort early when no blueprint is provided from the inspector button.
            if (blueprint == null)
            {
                Debug.LogWarning($"[{nameof(ClassData)}] Cannot apply a null blueprint on '{name}'.", this);
                return;
            }

            // Ensure list operations are safe even on new assets.
            _attributes ??= new List<AttributeValuePair>();

            // Treat a missing blueprint list as empty to avoid null-reference errors.
            var blueprintSource = blueprint.Attributes ?? new List<AttributeData>();

            // Build lookup and ordering data from the blueprint while ignoring null entries.
            var blueprintAttributes = blueprintSource.Where(attribute => attribute != null).ToList();
            var blueprintSet = new HashSet<AttributeData>(blueprintAttributes);

            // Remove attributes that are not declared by the selected blueprint.
            _attributes.RemoveAll(pair => pair?.Attribute == null || !blueprintSet.Contains(pair.Attribute));

            // Reorder the remaining attributes to match blueprint ordering.
            _attributes.Sort((first, second) =>
            {
                var firstIndex = blueprintAttributes.IndexOf(first.Attribute);
                var secondIndex = blueprintAttributes.IndexOf(second.Attribute);
                return firstIndex.CompareTo(secondIndex);
            });

            Debug.Log($"[{nameof(ClassData)}] Applied blueprint '{blueprint.name}' on '{name}'.", this);
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Populates the class with any AttributeData assets not already present.
        /// </summary>
        [Button(ButtonSizes.Medium)]
        public void PopulateAttributes()
        {
#if UNITY_EDITOR
            // Ensure we can populate in-place even if the list has never been initialized.
            _attributes ??= new List<AttributeValuePair>();

            // Collect existing attributes to avoid duplicates.
            var existingAttributes = new HashSet<AttributeData>();
            foreach (var pair in _attributes)
            {
                if (pair?.Attribute != null)
                {
                    existingAttributes.Add(pair.Attribute);
                }
            }

            // Find all AttributeData assets in the project.
            string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(AttributeData).Name}");
            foreach (var guid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AttributeData attribute = AssetDatabase.LoadAssetAtPath<AttributeData>(path);
                if (attribute == null || existingAttributes.Contains(attribute))
                {
                    continue;
                }

                _attributes.Add(new AttributeValuePair(attribute, 0f));
                existingAttributes.Add(attribute);
            }

            Debug.Log($"[{nameof(ClassData)}] Populated '{name}' with {_attributes.Count} attributes.", this);
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Removes all attributes with a value of 0 from the class.
        /// </summary>
        [Button(ButtonSizes.Medium)]
        public void RemoveZeroValueAttributes()
        {
#if UNITY_EDITOR
            // Ensure the list exists before attempting to remove elements.
            if (_attributes == null)
            {
                Debug.LogWarning($"[{nameof(ClassData)}] No attributes to remove on '{name}'.", this);
                return;
            }

            // Track how many attributes we start with to report how many were removed.
            int initialCount = _attributes.Count;
            
            // Remove all attributes where the value is 0.
            _attributes.RemoveAll(pair => pair != null && pair.Value == 0f);
            
            int removedCount = initialCount - _attributes.Count;
            if (removedCount > 0)
            {
                Debug.Log($"[{nameof(ClassData)}] Removed {removedCount} zero-value attributes from '{name}'.", this);
                EditorUtility.SetDirty(this);
            }
            else
            {
                Debug.Log($"[{nameof(ClassData)}] No zero-value attributes found on '{name}'.", this);
            }
#endif
        }
    }
}
