using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpPickUp : MonoBehaviour
{
    [SerializeField] powerUpPickUp powerUp;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
