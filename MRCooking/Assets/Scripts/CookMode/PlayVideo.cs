using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 0.5f;

    [SerializeField]
    private VideoPlayer vPlayer;

    [SerializeField]
    private RawImage rImage;

    private void Awake()
    {
        StartCoroutine(startVideo());
    }

    private IEnumerator startVideo() 
    {
        vPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(waitTime);
        while (!vPlayer.isPrepared) {
            yield return waitForSeconds;
            break;
        }
        rImage.texture = vPlayer.texture;
        vPlayer.Play();
    }
    
}
