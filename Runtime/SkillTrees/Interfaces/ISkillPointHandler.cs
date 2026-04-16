namespace GameUtils
{
    public interface ISkillPointHandler
    {
        bool HasEnough(int amount);
        void Spend(int amount);
        void Refund(int amount);
    }
}