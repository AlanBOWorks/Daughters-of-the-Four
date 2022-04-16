using UnityEngine;

namespace CombatSystem.Entity
{
    public static class UtilsEntity
    {
        public static GameObject InstantiateProviderBody(CombatEntity entity)
        {
            InstantiateProviderBody(entity.Provider,out GameObject instantiatedGameObject);
            HandleInjections(in entity,in instantiatedGameObject);

            return instantiatedGameObject;
        }

        private static void InstantiateProviderBody(ICombatEntityProvider provider, 
            out GameObject instantiatedGameObject)
        {
            GameObject copyReference = provider.GetVisualPrefab();
            if (copyReference == null)
                instantiatedGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            else
                instantiatedGameObject = Object.Instantiate(copyReference);


            instantiatedGameObject.name = provider.GetProviderEntityName() + "(Clone) " + instantiatedGameObject.GetInstanceID();
        }

        public static void HandleInjections(in CombatEntity entity, in GameObject instantiatedGameObject)
        {
            var entityBody = instantiatedGameObject.GetComponent<ICombatEntityBody>();
            entity.Body = entityBody;
            entity.InstantiationReference = instantiatedGameObject;
        }
    }
}
