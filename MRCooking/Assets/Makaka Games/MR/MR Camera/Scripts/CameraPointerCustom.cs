/*
===================================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/o/all-unity-assets
===================================================================

Online Docs (Latest): https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org (in English or in Russian) 
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>

[HelpURL("https://makaka.org/unity-assets")]
public class CameraPointerCustom : MonoBehaviour
{
    [SerializeField]
    private Image imageReticleByDefault = null;

    [SerializeField]
    private Image imageReticleDetected = null;

    [SerializeField]
    private float _maxDistance = 10f;

    [SerializeField]
    private bool isDebugLogging = false;

    private GameObject _gazedAtObject = null;

    [SerializeField]
    private EventSystem eventSystem = null;

    /// <summary>
    /// Null Data Beacuse in VR we need only the fact of tap to screen
    /// </summary>
    private PointerEventData pointerEventData;

    private void Awake()
    {
        pointerEventData = new PointerEventData(eventSystem);
    }

    public void Update()
    {
        // Casts ray towards camera's forward direction,
        //to detect if a GameObject is being gazed at.
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position, transform.forward, out hit, _maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                if (isDebugLogging)
                {
                    DebugPrinter.Print("VR: GameObject detected.");
                }

                // New GameObject
                _gazedAtObject?.SendMessage(
                    "OnPointerExit",
                    pointerEventData,
                    SendMessageOptions.DontRequireReceiver);

                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject?.SendMessage(
                    "OnPointerEnter",
                    pointerEventData,
                    SendMessageOptions.DontRequireReceiver);

                SetReticleDetected(true);
            }
        }
        else
        {
            if (isDebugLogging)
            {
                DebugPrinter.Print("VR: !!! No GameObject detected.");
            }

            _gazedAtObject?.SendMessage(
                "OnPointerExit",
                pointerEventData,
                SendMessageOptions.DontRequireReceiver);

            _gazedAtObject = null;

            SetReticleDetected(false);
        }

#if UNITY_EDITOR

        // Checks for mouse touches.
        if (Input.GetMouseButtonDown(0))
        {

#else

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
         
#endif

        if (isDebugLogging)
        {
            DebugPrinter.Print("VR: OnPointerClick.");
        }

        _gazedAtObject?.SendMessage(
            "OnPointerClick",
            pointerEventData,
            SendMessageOptions.DontRequireReceiver);
        }
    }

    private void SetReticleDetected(bool isDetected)
    {
        imageReticleByDefault.enabled = !isDetected;
         
        imageReticleDetected.enabled = isDetected;
    }

    public void PrintTest(string message)
    {
        if (isDebugLogging)
        {
            DebugPrinter.Print(message);
        }
    }
}
