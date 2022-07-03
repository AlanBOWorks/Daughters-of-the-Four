using UnityEngine;

namespace Utils
{
    public static class UtilsInstantiation 
    {

        public static T InstantiationComponent<T>(T element, Transform onParent,
            out Transform instantiationElement)
        where T : Object
        {
            var instantiation = Object.Instantiate(element, onParent);
                instantiationElement = null;
            if(instantiation is Component component)
                instantiationElement = component.transform;
            else if (instantiation is GameObject gameObject)
            {
                instantiationElement = gameObject.transform;
            }
           

            return instantiation;
        }
        public static T InstantiationSpacialRandomness<T>(T element, Transform onParent, float positionMagnitude)
        where T : Object
        {
            return InstantiationSpacialRandomness(element, onParent, positionMagnitude, out _);
        }

        public static T InstantiationSpacialRandomness<T>(T element, Transform onParent, float positionMagnitude,
            out Transform instantiationElement)
            where T : Object
        {
            var instantiation =
                InstantiationComponent(element, onParent, out instantiationElement);

            Vector3 targetLocalPosition =
                Random.insideUnitSphere * Random.Range(0, positionMagnitude);
            instantiationElement.localPosition = targetLocalPosition;

            return instantiation;
        }

        private const float MinScaleSize = 0.01f;
        public static T InstantiationSpacialRandomness<T>(T element, Transform onParent,
            float positionMagnitude, float scaleMagnitudeOffset)
            where T : Object
        {
            return InstantiationSpacialRandomness(element, onParent, positionMagnitude, scaleMagnitudeOffset, out _);
        }

        public static T InstantiationSpacialRandomness<T>(T element, Transform onParent,
            float positionMagnitude, float scaleMagnitudeOffset, out Transform instantiationElement)
            where T : Object
        {
            var instantiation =
                InstantiationSpacialRandomness(element, onParent, positionMagnitude, out instantiationElement);
            float targetScaleModifier = Random.Range(1 - scaleMagnitudeOffset, 1 + scaleMagnitudeOffset);

            Vector3 targetLocalScale = instantiationElement.localScale * targetScaleModifier;
            instantiationElement.localScale = targetLocalScale;

            return instantiation;
        }


        /// <summary>
        /// Same as [<seealso cref="InstantiationSpacialRandomness{T}(T,UnityEngine.Transform,float)"/>] with rotation Randomness
        /// </summary>
        public static T InstantiationTransformRandomness<T>(T element, Transform onParent,
        float positionMagnitude, float scaleMagnitudeOffset)
        where T : Object
        {
            var instantiation = InstantiationSpacialRandomness(
                element, onParent, 
                positionMagnitude, 
                scaleMagnitudeOffset,
                out var instantiationElement);

            Quaternion targetRotation = Random.rotation;
            instantiationElement.localRotation = targetRotation;

            return instantiation;
        }

        public static T InstantiationComponent<T>(T element, Vector3 onPoint, Quaternion withRotation,
            out Transform instantiationElement)
        where T : Object
        {
            var instantiation = Object.Instantiate(element, onPoint, withRotation);
            instantiationElement = null;
            if (instantiation is Component component)
                instantiationElement = component.transform;
            else if (instantiation is GameObject gameObject)
            {
                instantiationElement = gameObject.transform;
            }


            return instantiation;
        }
        public static T InstantiationSpacialRandomness<T>(T element, Vector3 onPoint, float positionMagnitude)
        where T : Object
        {
            return InstantiationSpacialRandomness(element, onPoint, positionMagnitude, out _);
        }

        public static T InstantiationSpacialRandomness<T>(T element, Vector3 onPoint, float positionMagnitude,
            out Transform instantiationElement)
            where T : Object
        {
            Vector3 targetLocalPosition = Random.Range(0, positionMagnitude) *
                Random.insideUnitSphere  + onPoint;

            var instantiation =
                InstantiationComponent(element, targetLocalPosition, Quaternion.identity, out instantiationElement);

           

            return instantiation;
        }

        public static T InstantiationSpacialRandomness<T>(T element, Vector3 onPoint,
            float positionMagnitude, float scaleMagnitudeOffset)
            where T : Object
        {
            return InstantiationSpacialRandomness(element, onPoint, positionMagnitude, scaleMagnitudeOffset, out _);
        }

        public static T InstantiationSpacialRandomness<T>(T element, Vector3 onPoint,
            float positionMagnitude, float scaleMagnitudeOffset, out Transform instantiationElement)
            where T : Object
        {
            var instantiation =
                InstantiationSpacialRandomness(element, onPoint, positionMagnitude, out instantiationElement);
            float targetScaleModifier = Random.Range(1 - scaleMagnitudeOffset, 1 + scaleMagnitudeOffset);

            Vector3 targetLocalScale = instantiationElement.localScale * targetScaleModifier;
            instantiationElement.localScale = targetLocalScale;

            return instantiation;
        }


        /// <summary>
        /// Same as [<seealso cref="InstantiationSpacialRandomness{T}(T,Vector3,float)"/>] with rotation Randomness
        /// </summary>
        public static T InstantiationTransformRandomness<T>(T element, Vector3 onPoint,
        float positionMagnitude, float scaleMagnitudeOffset)
        where T : Object
        {
            var instantiation = InstantiationSpacialRandomness(
                element, onPoint,
                positionMagnitude,
                scaleMagnitudeOffset,
                out var instantiationElement);

            Quaternion targetRotation = Random.rotation;
            instantiationElement.localRotation = targetRotation;

            return instantiation;
        }
    }
}
