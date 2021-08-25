using _Team;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Characters
{
    public class PositionalStructureBase<T> : IStance<T>, IStanceElement<T>
    {
        public static IStanceProvider NeutralProvisionalProvider = new ProvisionalProvider();

        public PositionalStructureBase(IStanceProvider stanceProvider)
        {
            _stanceProvider = stanceProvider;
        }

        public PositionalStructureBase(IStanceProvider stanceProvider, T attackingStance, T neutralStance, T defendingStance)
        {
            AttackingStance = attackingStance;
            NeutralStance = neutralStance;
            DefendingStance = defendingStance;
            _stanceProvider = stanceProvider;
        }

        public PositionalStructureBase(IStanceProvider stanceProvider, IStanceData<T> data)
        : this(stanceProvider,data.AttackingStance,data.NeutralStance,data.DefendingStance)
        {}

        public void Injection(IStanceProvider stanceProvider)
            => _stanceProvider = stanceProvider;

        private IStanceProvider _stanceProvider;
        [ShowInInspector]
        public T AttackingStance { get; set; }
        [ShowInInspector]
        public T NeutralStance { get; set; }
        [ShowInInspector]
        public T DefendingStance { get; set; }

        public T GetCurrentStanceValue()
            => UtilsTeam.GetElement(this, _stanceProvider.CurrentStance);

        private class ProvisionalProvider : IStanceProvider
        {
            public EnumTeam.Stances CurrentStance => EnumTeam.Stances.Neutral;
        }

        /// <summary>
        /// Creates a [<see cref="PositionalStructureBase"/>] with a provisional [<see cref="IStanceProvider"/>]
        /// that only returns(<see cref="EnumTeam.Stances.Neutral"/>).
        /// <br></br><br></br>
        /// Should be updated with an [<see cref="IStanceProvider"/>] by [<see cref="Injection"/>]
        /// </summary>
        public static PositionalStructureBase<T> GenerateProvisionalGeneric()
            => new PositionalStructureBase<T>(NeutralProvisionalProvider);
    }

    /// <summary>
    /// Inherits from <see cref="PositionalStructureBase{T}"/>, but contains a reference that can be apply to all
    /// (All: <seealso cref="EnumTeam.Stances"/>)
    /// </summary>
    public class PositionalStructureAll<T> : PositionalStructureBase<T>, IStanceAll<T>
    {
        public PositionalStructureAll(IStanceProvider stanceProvider) : base(stanceProvider)
        { }

        public PositionalStructureAll(IStanceProvider stanceProvider, T attackingStance, T neutralStance, T defendingStance) : base(stanceProvider, attackingStance, neutralStance, defendingStance)
        { }

        public PositionalStructureAll(IStanceProvider stanceProvider, IStanceData<T> data) : base(stanceProvider, data)
        { }

        public T InAllStances { get; set; }
    }
}
