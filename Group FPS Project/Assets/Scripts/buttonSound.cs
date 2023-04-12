using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public AudioSource aud;
    [SerializeField] AudioClip audClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        aud.PlayOneShot(audClip);
    }

    public void OnSelect(BaseEventData eventData)
    {
        aud.PlayOneShot(audClip);
    }

}
