using System.Collections.Generic;
using TriInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameUtils
{
    /// <summary>
    /// Defines an ordered attribute blueprint that can be reused by ClassData assets.
    /// </summary>
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ATTRIBUTES_NAME + "Attribute Blueprint")]
    [DeclareBoxGroup("blueprint", Title = "Attribute Blueprint")]
    public class AttributeBlueprintData : ItemIdentifierData
    {
        [SerializeField, Group("blueprint"), TableList] private List<AttributeData> _attributes;

        /// <summary>
        /// Gets the ordered attribute list used by this blueprint.
        /// </summary>
        public IReadOnlyList<AttributeData> Attributes => _attributes;

        /// <summary>
        /// Populates the blueprint with any AttributeData assets not already present.
        /// </summary>
        [Button(ButtonSizes.Medium)]
        public void PopulateAttributes()
        {
#if UNITY_EDITOR
            // Ensure we can populate in-place even if the list has never been initialized.
            _attributes ??= new List<AttributeData>();

            // Track already assigned attributes to prevent duplicates.
            var existingAttributes = new HashSet<AttributeData>();
            foreach (var attribute in _attributes)
            {
                if (attribute != null)
                {
                    existingAttributes.Add(attribute);
                }
            }

            // Load every AttributeData asset and append only missing entries.
            string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(AttributeData).Name}");
            foreach (var guid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AttributeData attribute = AssetDatabase.LoadAssetAtPath<AttributeData>(path);
                if (attribute == null || existingAttributes.Contains(attribute))
                {
                    continue;
                }

                _attributes.Add(attribute);
                existingAttributes.Add(attribute);
            }

            Debug.Log($"[{nameof(AttributeBlueprintData)}] Populated '{name}' with {_attributes.Count} attributes.", this);
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
