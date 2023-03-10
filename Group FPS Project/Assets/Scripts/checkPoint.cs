using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    //Initalize Variables
    [SerializeField] Renderer model;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(flashMat());
        }
    }

    IEnumerator flashMat()
    {
        model.material.color = Color.green;
        gameManager.instance.checkPointMenu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
        gameManager.instance.checkPointMenu.SetActive(false);
    }
}
