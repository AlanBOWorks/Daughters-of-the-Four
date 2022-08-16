using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;

public class SandBoxGeneral : MonoBehaviour
{


    [Button, DisableInEditorMode]
    private void Test(float timer = 1)
    {
        UtilsScene.DoJustVisualTransition(timer, true, Action);

        void Action(bool isScreenShow)
        {
            if(isScreenShow) Debug.Log("Screen was shown");
            else Debug.Log("Finish animations");
        }
    }

}
