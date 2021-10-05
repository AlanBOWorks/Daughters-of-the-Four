using UnityEngine;

namespace __ProjectExclusive
{
    public class UDontDestroyOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

    }
}
