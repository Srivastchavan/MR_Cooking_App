using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : MonoBehaviour
{
    private GazeTarget _gazeTargetComponent;
    private voiceController _voiceController;

    

    // Start is called before the first frame update
    void Awake()
    {
        _gazeTargetComponent = GetComponent<GazeTarget>();
        _voiceController = GameObject.FindGameObjectWithTag("voiceController").GetComponent<voiceController>();

    }

    // Update is called once per frame
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
        Debug.Log("Assistant impact point: " + impactPoint);
        // Change material of GameObject's renderer to "over" material
        _voiceController.StartListening();
    }

    //Handle the OnGazeOut event
    public void OnGazeOut()
    {
        // Change material to "normal" material
        _voiceController.StopListening();

    }

}
