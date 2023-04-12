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
    [SerializeField] float multiplier = 30f;
    [SerializeField] private Toggle toggle;
    private bool disableToggleEvent;
    private void Awake()
    {
        slider.onValueChanged.AddListener(HanleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChange);
    }

    private void HandleToggleValueChange(bool enableSound)
    {
        if (disableToggleEvent)
            return; 
        if (enableSound)
            slider.value = slider.maxValue;
        else
            slider.value = slider.minValue;

    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
    private void HanleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, MathF.Log10(value) * multiplier);
        disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
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