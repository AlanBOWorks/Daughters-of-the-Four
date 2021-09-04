using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    public class UGameTheme : MonoBehaviour
    {
        [SerializeField,HideInPlayMode] 
        private SGameTheme theme = null;


        private void Awake()
        {
            theme.InjectInSingleton();
        }


    }
}
