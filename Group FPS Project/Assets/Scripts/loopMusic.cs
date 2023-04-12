using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopMusic : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playAudio());
    }

    IEnumerator playAudio()
    {
        yield return null;

        for (int i = 0; i < audioClips.Length; i++)
        {
            audioSource.clip = audioClips[i];




            audioSource.Play();


            while (audioSource.isPlaying)
            {
                yield return null;
            }

        }


    }
}
