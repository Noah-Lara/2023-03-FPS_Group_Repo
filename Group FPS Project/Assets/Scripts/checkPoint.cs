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
            StartCoroutine(flashmat());
        }
    }

    // Creates a flash and response
    IEnumerator flashmat()
    {
        model.material.color = Color.red;
        gameManager.instance.checkpointMenu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
        gameManager.instance.checkpointMenu.SetActive(false);
    }
}
