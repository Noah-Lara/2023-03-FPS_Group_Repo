using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] private Toggle toggle;
    [SerializeField] float multiplier = 40f;
    float defaultSliderValue;
    private bool disableToggleEvent;
    private void Awake()
    {
        defaultSliderValue = slider.value;
        slider.onValueChanged.AddListener(HanleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChange);
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }

    private void HandleToggleValueChange(bool enableSound)
    {
        if (disableToggleEvent)
            return;

        if (enableSound)
            slider.value = defaultSliderValue;
        else
            slider.value = .0001f;

    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
    private void HanleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        disableToggleEvent = true;
        toggle.isOn = slider.value > .0001f;
        disableToggleEvent = false;
    }
  

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
