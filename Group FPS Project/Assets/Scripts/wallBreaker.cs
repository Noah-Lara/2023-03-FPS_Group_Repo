using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBreaker : MonoBehaviour
{
    public GameObject Breaker;
    public GameObject Destroyed;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerScript.isSprinting)
        {
            Instantiate(Destroyed, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
