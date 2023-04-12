using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.instance.aud.PlayOneShot(gameManager.instance.audClip);
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameManager.instance.aud.PlayOneShot(gameManager.instance.audClip);
    }

}
