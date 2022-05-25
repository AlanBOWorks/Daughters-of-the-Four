using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Animations
{
    public interface ICombatEntityAnimator
    {
        void Injection(in CombatEntity user);

        void PerformInitialCombatAnimation();
        void PerformEndCombatAnimation();

        void OnRequestSequenceAnimation();
        void PerformActionAnimation(in CombatSkill skill, in CombatEntity onTarget);
        void ReceiveActionAnimation(in CombatSkill fromSkill, in CombatEntity fromPerformer);
        void OnEndSequenceAnimation();
    }

    public interface IEntityAnimationsPackStructureRead<out T> :
        IEntityAnimationsStructureRead<ISkillArchetypeStructureRead<T>>
    {
        T InitialAnimationType { get; }
    }

    public interface IEntityAnimationsStructureRead<out T> 
    {
        T AnimationPerformType { get; }
        T AnimationReceiveType { get; }
    }
}
