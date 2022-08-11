using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SandBoxGeneral : MonoBehaviour
{

    private GameObject[] _cached;

    [Button]
    private void HideScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        if(_cached == null)
            _cached = currentScene.GetRootGameObjects();
        foreach (var objectRoot in _cached)
        {
            objectRoot.SetActive(!objectRoot.activeSelf);
        }
    }

}
