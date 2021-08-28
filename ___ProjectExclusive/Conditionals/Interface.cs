using Characters;

namespace CombatConditions
{
    public interface ICondition
    {
        bool CanApply(CombatingEntity target, float checkValue);
    }

    public interface IConditionHolder
    {
        ConditionParam GetCondition();
    }

    /// <summary>
    /// Used only for checking the user (not applies checks to targets). <br></br>
    /// Unlike <seealso cref="IConditionHolder"/> which do check user and target
    /// </summary>
    public interface IUserConditionHolder
    {
        UserOnlyConditionParam GetUserCondition();
    }
}
