using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

public class EyeRaycast : MonoBehaviour
{

    private Transform _cameraTransform;
    [SerializeField] private const float RayLength = 50f;

    private GazeTarget _currentGazedObject;

    [SerializeField] private Reticle _reticle;

    [SerializeField, Tooltip("In seconds")]
    float m_loadingTime;

    [SerializeField]
    FloatUnityEvent m_onLoad;

    float m_elapsedTime = 0;

    void Start()
    {
        // Retrieve transform of the camera component
        // (which is on the same GameObject as this script)
        _cameraTransform = GetComponent<Camera>().transform;
#if DEVELOPMENT_BUILD
        Debug.Log("Ich bin im Development-Modus");
#endif
    }

    void Update()
    {
        PerformEyeRaycast();
    }


    private void PerformEyeRaycast()
    {
        // Create a ray that points forwards from the camera.
        // Could also use Camera.main.transform.position
        var fwd = _cameraTransform.forward;
        var ray = new Ray(_cameraTransform.position, fwd);

        // Only visible in editor
        Debug.DrawRay(_cameraTransform.position, fwd * RayLength, Color.green);

        RaycastHit hit;
        // Do the Raycast forwards to see if we hit an interactive item
        if (Physics.Raycast(ray, out hit, RayLength) && hit.collider != null)
        {
            // Debug.Log("Hit " + hit.point);

            // Something was hit, set reticle to the hit position.
            if (_reticle)
                _reticle.SetPosition(hit);

            // Check the tag before using GetComponent - that's expensive
            if (hit.transform.CompareTag("GazeTarget"))
            {
                // Even more efficient would be:
                // Get hit.transform.gameObject and use that to compare.
                // Only get the GazeTarget component when needed!

                // Get the GazeTarget of the target GameObject
                var interactible = hit.collider.GetComponent<GazeTarget>();

                // If we hit a different object than before, deactivate
                // the last interactible
                if (interactible != _currentGazedObject)
                {
                    // Send GazeOut event to previous interactible
                    DeactiveateLastInteractible();
                }

                // If we hit an interactive item and it's not the same as
                // the last interactive item, then call OnGazeEntered
                if (interactible)
                {
                    m_elapsedTime += Time.deltaTime;
                    m_onLoad.Invoke(m_elapsedTime / m_loadingTime);
                    if (m_elapsedTime > m_loadingTime)
                    {
                        // Send GazeEntered event to new interactible
                        interactible.OnGazeEntered(hit.point);
                    }
                    
                }

                _currentGazedObject = interactible;

            }
        }
        else
        {
            // Nothing was hit, deactivate the last interactive item.
            DeactiveateLastInteractible();

            // Position the reticle at default distance.
            if (_reticle)
                _reticle.SetPosition();
        }
    }


    private void DeactiveateLastInteractible()
    {
        if (_currentGazedObject == null)
            return;
        m_elapsedTime = 0;
        m_onLoad.Invoke(m_elapsedTime / m_loadingTime);
        _currentGazedObject.OnGazeOut();
        _currentGazedObject = null;
    }
}
