using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioClip defaultSong;



    private AudioSource track1;
    private AudioSource track2;
    private bool isPlayingTrack;
    public static audioManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        track1 = gameObject.AddComponent<AudioSource>();
        track2 = gameObject.AddComponent<AudioSource>();
        isPlayingTrack = true;

        SwapTrack(defaultSong);

    }
    public void SwapTrack(AudioClip newClip)
    {

        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));

        isPlayingTrack = !isPlayingTrack;
    }


    private IEnumerator FadeTrack(AudioClip newClip)
    {

        float timeToFade = 1.25f;
        float timeElapsed = 0;

        if (isPlayingTrack)
        {
            track2.clip = newClip;
            track2.Play();

            while (timeElapsed < timeToFade)
            {
                track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track1.Stop();
        }
        else
        {
            track1.clip = newClip;
            track1.Play();

            while (timeElapsed < timeToFade)
            {
                track1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track2.Stop();
        }

    }

}
