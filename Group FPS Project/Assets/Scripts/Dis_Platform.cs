using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dis_Platform : MonoBehaviour
{
    [SerializeField] float fading_Time;
    [SerializeField] float flash_Time;
    [SerializeField] Renderer model;
    [SerializeField] int times_toFlash;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(flashMat());
        }

        IEnumerator Fading()
        {
            yield return new WaitForSeconds(fading_Time);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(fading_Time * 2);
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        IEnumerator flashMat()
        {
            for (int i = 0; i < times_toFlash; i++)
            {
                model.material.color = Color.red;
                yield return new WaitForSeconds(flash_Time);
                model.material.color = Color.white;
                yield return new WaitForSeconds(flash_Time);
            }
            StartCoroutine(Fading());
        }
    }
}
