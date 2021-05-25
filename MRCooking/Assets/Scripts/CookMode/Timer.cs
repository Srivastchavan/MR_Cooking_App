using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float setTime;
    public bool pause = false;
    public Text timeText;
    [SerializeField]
    private Material activeMat;
    [SerializeField]
    private Material inactiveMat;

    private Renderer bgRender;
    // Start is called before the first frame update
    void Start()
    {
        bgRender =  GetComponent<Renderer>();
        bgRender.material = inactiveMat;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (setTime > 0)
            {
                setTime -= Time.deltaTime;
                UpdateTimer(setTime);
                if (bgRender.material != activeMat)
                {
                    bgRender.material = activeMat;
                }
            }

            else
            {
                setTime = 0;
                pause = true;
                // Give end timer alert
                bgRender.material = activeMat;
            }
        }
        else if (bgRender.material != inactiveMat)
        {
            bgRender.material = inactiveMat;
        }
    }

    public void UpdateTimer(float newTime)
    {
        newTime += 1;
        float minute = Mathf.FloorToInt(newTime / 60);
        float second = Mathf.FloorToInt(newTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minute, second);
    }
}
