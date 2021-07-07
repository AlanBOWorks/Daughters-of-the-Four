using Characters;
using UnityEngine;

namespace ___ProjectExclusive._Enemies
{
    [CreateAssetMenu(fileName = "_ Fight Preset _ N [Combat Preset]",
        menuName = "Combat/Fight Preset")]
    public class SEnemyFightPreset : ScriptableObject, ICharacterArchetypesData<SEnemyCharacterEntityVariable>
    {
        [SerializeField] private SEnemyCharacterEntityVariable frontLiner;
        [SerializeField] private SEnemyCharacterEntityVariable midLiner;
        [SerializeField] private SEnemyCharacterEntityVariable backLiner;

        public SEnemyCharacterEntityVariable FrontLiner => frontLiner;

        public SEnemyCharacterEntityVariable MidLiner => midLiner;

        public SEnemyCharacterEntityVariable BackLiner => backLiner;
    }
}
