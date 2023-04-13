using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonSound : MonoBehaviour, ISelectHandler
{
    public AudioSource aud;
    [SerializeField] AudioClip audClip;

    public void OnSelect(BaseEventData eventData)
    {
        aud.PlayOneShot(audClip);
    }

}
