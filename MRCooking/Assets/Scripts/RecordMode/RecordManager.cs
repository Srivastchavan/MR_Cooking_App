
using Google.XR.Cardboard;
using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class RecordManager : MonoBehaviour
{
    [Header("Recording")]
    public int videoWidth = 1280;
    public int videoHeight = 720;
    public bool recordMicrophone;
    public Camera cam;
    public RecordSteps recStep;
    public GameObject sharebtn;
    public GameObject goBackbtn;

    private IMediaRecorder videoRecorder;
    private CameraInput cameraInput;
    private AudioInput audioInput;
    private AudioSource microphoneSource;
    private bool pressed;
    private const float MaxRecordingTime = 7200f; // seconds
    private string savePath;

    private IEnumerator Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Wait for 2 seconds
        //StopXR(); // This will be removed 
        //yield return new WaitForSeconds(5);
        // Start microphone
        microphoneSource = gameObject.AddComponent<AudioSource>();
        microphoneSource.mute =
        microphoneSource.loop = true;
        microphoneSource.bypassEffects =
        microphoneSource.bypassListenerEffects = false;
        microphoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
        microphoneSource.Play();
        // Start recording
        var frameRate = 30;
        var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
        var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
        var recordingClock = new RealtimeClock();
        videoRecorder = new MP4Recorder(
            videoWidth,
            videoHeight,
            frameRate,
            sampleRate,
            channelCount,
            recordingPath => {
                Debug.Log($"Saved recording to: {recordingPath}");
                var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
                Handheld.PlayFullScreenMovie($"{prefix}{recordingPath}");
                savePath = $"{prefix}{recordingPath}";
                sharebtn.gameObject.SetActive(true);
                goBackbtn.gameObject.SetActive(true);
            }
        );
        // Create recording inputs
        cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
        audioInput = recordMicrophone ? new AudioInput(videoRecorder, recordingClock, microphoneSource, true) : null;
        // Unmute microphone
        microphoneSource.mute = audioInput == null;
    }

    private void OnDestroy()
    {
        // Stop microphone
        microphoneSource.Stop();
        Microphone.End(null);
    }

    public void StopRecording()
    {
        // Stop recording
        audioInput?.Dispose();
        cameraInput.Dispose();
        videoRecorder.Dispose();
        videoRecorder = null;
        //var savePath = await videoRecorder.FinishWriting();
        // Mute microphone
        microphoneSource.mute = true;
        Debug.Log("Stopped Recording!");
        
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void ShareVideo()
    {
        string title = recStep.newRecipe.Name;
        new NativeShare().AddFile(savePath)
        .SetSubject("Cooking Recording").SetText(title + " by Teja")
        .SetCallback((result, shareTarget) => ExitApp())
        .Share();
        recStep.Text.text = savePath;
    }

    // This function will be shifted to Previous Scene 
    private void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");
        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");
    }
    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed.");
        }
        else
        {
            Debug.Log("XR initialized.");

            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }

    public void GoToRecipeList()
    {
        EnterVR();
        SceneManager.LoadScene("ModeSelect");
    }
    private void EnterVR()
    {
        StartCoroutine(StartXR());
        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }
    }
}
