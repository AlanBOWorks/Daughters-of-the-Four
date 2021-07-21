using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    public class UGameTheme : MonoBehaviour
    {
        [SerializeField,HideInPlayMode] 
        private SGameTheme theme = null;

        [DisableInEditorMode,ShowInInspector]
        private GameThemeSingleton _singleton = GameThemeSingleton.Instance; 


        private void Awake()
        {
            theme.InjectInSingleton();
        }


    }
}
