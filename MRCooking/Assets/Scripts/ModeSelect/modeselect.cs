using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class modeselect : MonoBehaviour
{


    GameObject dialog = null;

    [SerializeField]
    public static string mode =  "CookMode";

    void Awake()
    {
        CheckPermissionOrRequestBase();

    }

    private bool CheckPermissionOrRequestBase()
    {
        NativeCamera.Permission resultOfCheckPermission =
                    NativeCamera.CheckPermission();

        DebugPrinter.Print("CheckCameraPermissionOrRequest");

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            dialog = new GameObject();
         }
         if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            dialog = new GameObject();
         }
#endif

        switch (resultOfCheckPermission)
        {
            case NativeCamera.Permission.Granted:

                DebugPrinter.Print("Camera Permission Granted");

                return true;

            case NativeCamera.Permission.ShouldAsk:

                NativeCamera.Permission resultOfRequestPermission;

                do
                {
                    resultOfRequestPermission =
                        NativeCamera.RequestPermission();

                    DebugPrinter.Print(2);
                }
                while (resultOfRequestPermission
                    == NativeCamera.Permission.ShouldAsk);

                if (resultOfRequestPermission
                    == NativeCamera.Permission.Granted)
                {
                    DebugPrinter.Print(3.1f);

                    return true;
                }
                else
                {
                    DebugPrinter.Print(3.2f);

                    return false;
                }

            case NativeCamera.Permission.Denied:

                DebugPrinter.Print(4);

                return false;

            default:

                DebugPrinter.Print(5);

                return false;
        }


    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            return;
        }
        else if (dialog != null)
        {
            Destroy(dialog);
        }
#endif

        // Now you can do things with the microphone
    }



    public void GotoCookMode()
    {
       mode =  "CookMode";
       SceneManager.LoadScene("RecipeList");
    }

    public void GotoRecordMode()
    {
       mode =  "RecordMode";
       SceneManager.LoadScene("RecipeList");
    }  

}
