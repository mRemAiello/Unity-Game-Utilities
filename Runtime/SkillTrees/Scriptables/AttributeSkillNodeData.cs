using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Attribute")]
    [CreateAssetMenu(menuName = GUConstants.SKILL_TREE_NAME + "/Attribute Skill Node")]
    public class AttributeSkillNodeData : SkillNodeData
    {
        [SerializeField, Group("Attribute")] private AttributeData _attribute;
        [SerializeField, Group("Attribute")] private float _valuePerLevel = 1f;
        [SerializeField, Group("Attribute")] private ClassData _requiredClass;

        //
        public AttributeData Attribute => _attribute;
        public float ValuePerLevel => _valuePerLevel;
        public ClassData RequiredClass => _requiredClass;

        //
        public override void OnLevelUp(ISkillContext context, int newLevel)
        {
            base.OnLevelUp(context, newLevel);
            ApplyAttributeModifier(context);
        }

        public override void OnLevelDown(ISkillContext context, int newLevel)
        {
            base.OnLevelDown(context, newLevel);
            RemoveAttributeModifier(context);
        }

        private void ApplyAttributeModifier(ISkillContext context)
        {
            if (_attribute == null)
                return;
            if (!context.TryGet<RuntimeClass>(out var runtimeClass))
                return;
            if (_requiredClass != null && runtimeClass.ClassData != _requiredClass)
                return;
            if (!runtimeClass.TryGetAttribute(_attribute, out var runtimeAttribute))
                return;

            // Add a new modifier for this level-up. 
            // The modifier is non-permanent and will be removed on level-down.
            var modifier = new ModifierFixed(this, _valuePerLevel, 0f, isPermanent: false);
            runtimeAttribute.AddModifier(modifier);
        }

        private void RemoveAttributeModifier(ISkillContext context)
        {
            if (_attribute == null)
                return;
            if (!context.TryGet<RuntimeClass>(out var runtimeClass))
                return;
            if (_requiredClass != null && runtimeClass.ClassData != _requiredClass)
                return;
            if (!runtimeClass.TryGetAttribute(_attribute, out var runtimeAttribute))
                return;

            // Remove one modifier associated with this skill node. 
            // This assumes that each level-up adds one modifier, so we remove one per level-down.
            foreach (var modifier in runtimeAttribute.GetModifiersBySource(this))
            {
                runtimeAttribute.RemoveModifier(modifier, includePermanent: false);
                break; // remove one modifier per level-down call
            }
        }
    }
}
