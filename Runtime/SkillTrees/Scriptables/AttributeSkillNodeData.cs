using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Attribute")]
    [CreateAssetMenu(menuName = GUConstants.SKILL_TREE_NAME + "/Attribute Skill Node")]
    public class AttributeSkillNodeData : SkillNodeData
    {
        [SerializeField, Group("Attribute")] private AttributeData _attribute;
        [SerializeField, Group("Attribute")] private ModifierType _modifierType = ModifierType.Fixed;
        [SerializeField, Group("Attribute")] private float _valuePerLevel = 1f;
        [SerializeField, Group("Attribute")] private ClassData _requiredClass;

        //
        public AttributeData Attribute => _attribute;
        public ModifierType ModifierType => _modifierType;
        public float ValuePerLevel => _valuePerLevel;
        public ClassData RequiredClass => _requiredClass;

        //
        public virtual bool CanApplyToContext(ISkillContext context, out RuntimeAttribute runtimeAttribute)
        {
            runtimeAttribute = null;
            if (_attribute == null)
                return false;
            if (!context.TryGet<RuntimeClass>(out var runtimeClass))
                return false;
            if (_requiredClass != null && runtimeClass.ClassData != _requiredClass)
                return false;
            if (!runtimeClass.TryGetAttribute(_attribute, out runtimeAttribute))
                return false;

            // All checks passed, this skill node can be applied to the context.
            return true;
        }

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
            if (!CanApplyToContext(context, out var runtimeAttribute))
                return;

            // Add a new modifier for this level-up. 
            // The modifier is non-permanent and will be removed on level-down.
            Modifier modifier;
            if (_modifierType == ModifierType.Fixed)
            {
                modifier = new ModifierFixed(this, _valuePerLevel, 0f, isPermanent: false);
            }
            else
            {
                modifier = new ModifierPercentage(this, _valuePerLevel, 0f, isPermanent: false);
            }
            runtimeAttribute.AddModifier(modifier);
        }

        private void RemoveAttributeModifier(ISkillContext context)
        {
            if (!CanApplyToContext(context, out var runtimeAttribute))
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
