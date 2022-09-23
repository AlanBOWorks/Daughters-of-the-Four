using UnityEngine;

namespace Utils
{
    public class UDeActive : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }

    }
}
