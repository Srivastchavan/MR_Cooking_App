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
using UnityEngine.Events;

using Google.XR.Cardboard;

/// <summary>
/// Initializes Cardboard XR Plugin.
/// </summary>

[HelpURL("https://makaka.org/unity-assets")]
public class CardboardStartupCustom : MonoBehaviour
{

#if UNITY_EDITOR

    [Space]
    [SerializeField]
    private UnityEvent OnRuntimeStartedInEditor = null;

#endif

    [Space]
    [SerializeField]
    private UnityEvent OnRuntimeStarted = null;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        // Configures the app to not shut down the screen and sets the brightness to maximum.
        // Brightness control is expected to work only in iOS, see:
        // https://docs.unity3d.com/ScriptReference/Screen-brightness.html.
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;

        OnRuntimeStarted.Invoke();

#if UNITY_EDITOR

        OnRuntimeStartedInEditor.Invoke();

#endif

    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            Application.Quit();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }
    }
}
