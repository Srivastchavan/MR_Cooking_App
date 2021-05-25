using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoBtnControl : MonoBehaviour
{

    private Renderer _gameObjectRenderer;
    private GazeTarget _gazeTargetComponent;

    public UnityEvent CallFunction;

    bool triggered = true;

    void Awake()
    {
        _gazeTargetComponent = GetComponent<GazeTarget>();
        _gameObjectRenderer = GetComponent<Renderer>();
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

    //Handle the OnGazeEntered event
    public void OnGazeEntered(object sender, Vector3 impactPoint)
    {
        Debug.Log("Impact point: " + impactPoint);

        if (triggered)
        {
            CallFunction.Invoke();
            triggered = false;
        }
       

    }

    //Handle the OnGazeOut event
    public void OnGazeOut()
    {
       
        triggered = true;

    }
}
