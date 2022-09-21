using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ULoadCombatAnimator : MonoBehaviour
{
    [Title("References")]
    [SerializeField] private RectTransform[] rayRotationObjects;

    [Title("Params")] 
    [SerializeField] private float rayRotationSpeed = 4;

    private void Update()
    {
        RotateRays();
    }

    private void RotateRays()
    {
        float deltaStep = rayRotationSpeed * Time.deltaTime;
        for (var i = 0; i < rayRotationObjects.Length; i++)
        {
            var rectTransform = rayRotationObjects[i];
            var rotation = new Vector3(0,0,deltaStep * (i+1));
            rectTransform.Rotate(rotation);
        }
    }
}
