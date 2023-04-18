using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSwap : MonoBehaviour
{

    public AudioClip newTrack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.instance.SwapTrack(newTrack);
        }
    }
}
