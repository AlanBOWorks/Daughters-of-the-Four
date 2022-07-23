using Sirenix.OdinInspector;
using UnityEngine;

namespace SCharacterCreator.Bones
{
    public class UHumanoidBonesHolder : MonoBehaviour
    {
        [SerializeReference] 
        private IBasicHumanoidBonesHolder basicHumanoid;
        [SerializeReference]
        private IHumanoidBonesHolder fullHumanoid = new HumanoidBonesHolder();

        public IBasicHumanoidBonesHolder GetBasicBones() => basicHumanoid;
        public IHumanoidBonesHolder GetFullBones() => fullHumanoid;

        [Button]
        private void InjectBones(Animator animator)
        {
            if (fullHumanoid is HumanoidBonesHolder holder)
                UtilsBones.InjectBones(holder, animator);
            else
                UtilsBones.InjectBones(fullHumanoid, animator);


            if (basicHumanoid == fullHumanoid) return;
            UtilsBones.InjectBasicHumanoidBones(basicHumanoid,animator);
        }
    }
}
