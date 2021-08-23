using UnityEngine;

namespace _Team
{

    public interface IStanceData<out T>
    {
        T AttackingStance { get; }
        T NeutralStance { get; }
        T DefendingStance { get; }
    }

    public interface IStanceElement<out T>
    {
        T GetCurrentStanceValue();
    }

    public interface IStanceProvider
    {
        EnumTeam.Stances CurrentStance { get; }
    }
}
