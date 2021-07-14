using UnityEngine;

namespace Characters
{
    public class UCharacterSystemDeclarator : MonoBehaviour
    {
        private void Awake()
        {
            CharacterSystemSingleton.CharactersSpawner = new EntityHolderSpawner();
        }

        private void Start()
        {
            Destroy(this);
        }
    }
}
