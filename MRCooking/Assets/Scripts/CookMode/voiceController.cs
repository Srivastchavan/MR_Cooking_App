using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using UnityEngine.Android;
using System.Text.RegularExpressions;
using TMPro;
public class voiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    CurrRecipe recipe;

    [SerializeField]
     TextMeshProUGUI uiText;

    [SerializeField]
    public Timer time;

    [SerializeField]
    public MyVideoPlayer vplayer;

    private void Start()
    {
        Setup(LANG_CODE);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;

        CheckPermission();

        OnFinalSpeechResult("set timer to 1 mintue");
    }

    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    #region Text to Speech

    public void StartSpeaking(string message)
    {
        TextToSpeech.instance.StartSpeak(message);
    }

    public void StopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started...");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stopped...");
    }

    #endregion

    #region Speech To Text

    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }

    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }
    
    void OnFinalSpeechResult(string result)
    {
        uiText.text = result.ToLower();
        result = result.ToLower();
        if (result.Contains("next") || result.Contains("after"))
        {
            recipe.Next();
        }

        else if(result.Contains("previous") || result.Contains("before"))
        {
            recipe.Previous();
        }

        
        if(result.Contains("set timer to"))
        {
            Regex regex_min = new Regex("[0-9]+ minutes?");
            Regex regex_sec = new Regex("[0-9]+ seconds?");
            //string result = "set timer to 2 minutes 20 seconds";
            Match match_min = regex_min.Match(result);
            Match match_sec = regex_sec.Match(result);
            int minutes = 0, seconds = 0;
            if (match_min.Success)
            {
                minutes = int.Parse(match_min.Groups[0].Value.Split(' ')[0]);
            }
            if (match_sec.Success)
            {
                seconds = int.Parse(match_sec.Groups[0].Value.Split(' ')[0]);
            }
            float inSec = minutes * 60 + seconds;
            if (inSec > 0)
            {
                time.setTime = inSec;
                time.pause = false;
            }
        }

        if(time.setTime > 0 ){
            if (result.Contains("pause timer") || result.Contains("pause the timer"))
            {
                time.pause = true;
            }
            
            else if(result.Contains("resume timer") || result.Contains("resume the timer"))
            {
                time.pause = false;
            }
            else if (result.Contains("start timer") || result.Contains("start the timer"))
            {
                time.pause = false;
            }

            else if (result.Contains("reset timer") || result.Contains("reset the timer"))
            {
                time.UpdateTimer(-1);
                time.pause = true;
            }
            else if (result.Contains("stop timer") || result.Contains("stop the timer"))
            {
                time.pause = true;
            }
        }

        if (result.Contains("play video") || result.Contains("resume video"))
        {
            vplayer.VideoPlay();
        }

        else if (result.Contains("pause video") || result.Contains("stop video"))
        {
            vplayer.VideoStop();
        }




    }

    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }

    #endregion


    void Setup(string code)
    {
        TextToSpeech.instance.Setting(code, 1, 1);
        SpeechToText.instance.Setting(code);
    }

}
