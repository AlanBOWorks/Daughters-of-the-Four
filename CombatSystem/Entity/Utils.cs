using CombatSystem._Core;
using UnityEngine;

namespace CombatSystem.Entity
{
    public static class UtilsEntity
    {
        public static GameObject InstantiateProviderBody(CombatEntity entity)
        {
            InstantiateProviderBody(entity.Provider,out GameObject instantiatedGameObject);
            HandleInjections(entity,instantiatedGameObject);

            return instantiatedGameObject;
        }

        private static void InstantiateProviderBody(ICombatEntityProvider provider, 
            out GameObject instantiatedGameObject)
        {
            GameObject copyReference = provider.GetVisualPrefab();
            if (copyReference == null)
            {
                instantiatedGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                instantiatedGameObject.AddComponent<UCombatEntityBody>();
            }
            else
                instantiatedGameObject = Object.Instantiate(copyReference);


            instantiatedGameObject.name = provider.GetProviderEntityName() + "(Clone) " + instantiatedGameObject.GetInstanceID();
        }

        public static void HandleInjections(CombatEntity entity, GameObject instantiatedGameObject)
        {
            var entityBody = instantiatedGameObject.GetComponent<ICombatEntityBody>();
            entity.Body = entityBody;
            entity.InstantiationReference = instantiatedGameObject;
        }
    }

    public sealed class UtilsCombatEntity
    {
        public static void DoSequenceFinish(CombatEntity onEntity)
        {
            CombatSystemSingleton.EventsHolder.OnEntityFinishSequence(onEntity, false);
        }
    }
}
