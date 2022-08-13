using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SandBoxGeneral : MonoBehaviour
{


    [Button]
    private void Test()
    {
        var components = GetComponents<SandBoxGeneral>();
        Debug.Log(components.Length);
    }

}
