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
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ATTRIBUTES_NAME + "Class")]
    [DeclareBoxGroup("class", Title = "Class")]
    public class ClassData : ItemVisualData
    {
        [SerializeField, Group("class"), TableList] private List<AttributeValuePair> _attributes;

        /// <summary>
        /// Gets the attribute list used by runtime class instances.
        /// </summary>
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
            _attributes.RemoveAll(pair => pair?.Data == null || !blueprintSet.Contains(pair.Data));

            // Reorder the remaining attributes to match blueprint ordering.
            _attributes.Sort((first, second) =>
            {
                var firstIndex = blueprintAttributes.IndexOf(first.Data);
                var secondIndex = blueprintAttributes.IndexOf(second.Data);
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
                if (pair?.Data != null)
                {
                    existingAttributes.Add(pair.Data);
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
    }
}
