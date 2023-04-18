using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPickup : MonoBehaviour
{
    playerController player;
    [SerializeField] float amount;

    [Header("-----Audio-----")]
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            gameManager.instance.playerScript.recoverStamina(amount);
            Destroy(gameObject);

        }
    }
}
