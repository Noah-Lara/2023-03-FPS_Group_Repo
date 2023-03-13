using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBreaker : MonoBehaviour
{
    public GameObject smash;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerScript.isSprinting)
        {
            Destroy(gameObject);
        }
    }
}
