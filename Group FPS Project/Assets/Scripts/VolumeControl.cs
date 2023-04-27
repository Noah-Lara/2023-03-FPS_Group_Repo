using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] public string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] public Slider slider;
    [SerializeField] private Toggle toggle;
    public float defaultSliderValue;
    private bool disableToggleEvent;
    private void Awake()
    {
        defaultSliderValue = slider.value;
        slider.onValueChanged.AddListener(HanleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChange);
    }    

    private void HandleToggleValueChange(bool enableSound)
    {
        if (disableToggleEvent)
            return;

        if (enableSound)
            slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
        else
            slider.value = slider.minValue;

    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
    private void HanleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, value);
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
