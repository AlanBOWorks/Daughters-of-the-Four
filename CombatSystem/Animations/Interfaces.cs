using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;

namespace CombatSystem.Animations
{
    public interface ICombatEntityAnimator
    {
        void Injection(CombatEntity user);

        void PerformInitialCombatAnimation();
        void PerformEndCombatAnimation();

        void OnRequestSequenceAnimation();
        void PerformActionAnimation(ISkill skill, in CombatEntity onTarget);
        void ReceiveActionAnimation(ISkill fromSkill, CombatEntity fromPerformer);
        void ReceiveActionAnimation(IEffect fromEffect, CombatEntity fromPerformer);
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

    public interface IEntityAnimationsLayerStructureRead<out T>
    {
        T IdleAnimationType { get; }
        T ActionAnimationType { get; }
    }
}
