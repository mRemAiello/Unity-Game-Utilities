namespace GameUtils
{
    public abstract class SkillEffectData : ItemIdentifierData
    {
        public abstract void Apply(ISkillContext context, int level);
        public abstract void Remove(ISkillContext context, int level);
    }
}