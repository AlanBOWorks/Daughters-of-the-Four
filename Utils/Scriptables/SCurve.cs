using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "N [Curve Holder]", menuName = "Utils/Scriptables/Curve Holder")]
    public class SCurve : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve curve = new AnimationCurve(
            new Keyframe(0,0), new Keyframe(1,1));

        public AnimationCurve GetCurve() => curve;
        public float Evaluate(float value) => curve.Evaluate(value);
    }
}
