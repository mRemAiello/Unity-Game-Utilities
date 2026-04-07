namespace GameUtils
{
    public abstract class SkillEffectData : ItemIdentifierData
    {
        public abstract void Apply(ISkillContext context);
        public abstract void Remove(ISkillContext context);
    }
}