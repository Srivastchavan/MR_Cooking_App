using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleQuad : MonoBehaviour
{
    private GazeTarget _gazeTargetComponent;

    public UnityEvent CallFunction;

    bool triggered = true;
    void Awake()
    {
        _gazeTargetComponent = GetComponent<GazeTarget>();
    }

    private void OnEnable()
    {
        _gazeTargetComponent.GazeEntered += OnGazeEntered;
        _gazeTargetComponent.GazeOut += OnGazeOut;
    }

    private void OnDisable()
    {
        _gazeTargetComponent.GazeEntered -= OnGazeEntered;
        _gazeTargetComponent.GazeOut -= OnGazeOut;
    }

    public void OnGazeEntered(object sender, Vector3 impactPoint)
    {
        Debug.Log("Impact point: " + impactPoint);

        if (triggered)
        {
            CallFunction.Invoke();
            triggered = false;
        }
        // Change material of GameObject's renderer to "over" material
        

    }

    //Handle the OnGazeOut event
    public void OnGazeOut()
    {
        // Change material to "normal" material
        triggered = true;

    }
}

