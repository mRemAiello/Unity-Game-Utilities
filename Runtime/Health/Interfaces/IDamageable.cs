namespace GameUtils
{
    public interface IDamageable
    {
        bool IsAlive { get; }
        void Damage(DamageInfo damageInfo);
    }
}