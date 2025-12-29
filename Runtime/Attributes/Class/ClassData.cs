using System.Collections.Generic;
using TriInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ATTRIBUTES_NAME + "Class")]
    [DeclareBoxGroup("class", Title = "Class")]
    public class ClassData : ItemAssetBase
    {
        [SerializeField, Group("class"), TableList] private List<AttributeValuePair> _attributes;

        //
        public IReadOnlyList<AttributeValuePair> Attributes => _attributes;

        [Button]
        public void PopulateAttributes()
        {
#if UNITY_EDITOR
            if (_attributes == null)
            {
                _attributes = new List<AttributeValuePair>();
            }

            var existingAttributes = new HashSet<AttributeData>();
            foreach (var pair in _attributes)
            {
                if (pair?.Data != null)
                {
                    existingAttributes.Add(pair.Data);
                }
            }

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
