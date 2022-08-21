using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils_Project;

public class SandBoxGeneral : MonoBehaviour, ISceneLoadFirstLastCallListener
{


    [Button, DisableInEditorMode]
    private void Test(float timer = 1)
    {
        UtilsScene.DoJustVisualTransition(timer, true, 1, this);
    }

    public void OnStartTransition()
    {
        Debug.Log("Screen was shown");
    }

    public void OnFinishTransition()
    {
        Debug.Log("Finish animations");
    }
}
