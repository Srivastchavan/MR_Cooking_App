using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BtnScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent action;

    private bool gazedAt;

    public void OnPointerEnter(PointerEventData d)
    {
        gazedAt = true;
    }

    public void OnPointerExit(PointerEventData d)
    {
        gazedAt = false;
    }

    void Update()
    {
        if (gazedAt)
        {
            Debug.Log("Gazed!");
        }
    }
}