using UnityEngine;

namespace SharedLibrary
{

    [CreateAssetMenu(fileName = "N_Curve [Variable]",
        menuName = "Variable/Animation Curve")]
    public class AnimationCurveVariable : SerializedScriptableVariable<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve curveData = new AnimationCurve(
            new Keyframe(0,0), new Keyframe(1,1));

        public override AnimationCurve Data => curveData;
    }
}
