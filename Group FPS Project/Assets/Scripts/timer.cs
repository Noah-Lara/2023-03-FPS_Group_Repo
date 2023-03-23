using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    [Header("----- Components -----")]
    public TextMeshProUGUI timerText;

    [Header("----- Timer Settings -----")]
    public float currentTime;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("0.00");
    }
}
