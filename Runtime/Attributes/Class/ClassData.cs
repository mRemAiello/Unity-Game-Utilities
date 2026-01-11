using System.Collections.Generic;
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
        /// Populates the class with any AttributeData assets not already present.
        /// </summary>
        [Button(ButtonSizes.Medium)]
        public void PopulateAttributes()
        {
#if UNITY_EDITOR
            _attributes ??= new List<AttributeValuePair>();

            // Collect existing attributes to avoid duplicates
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

            EditorUtility.SetDirty(this);
#endif
        }
    }
}
