using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    [SerializeField] int timer;
    [SerializeField] GameObject hitEffect;
    

    bool isHit;


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (other.CompareTag("Player") && !isHit)
        {
            isHit = true;
            gameManager.instance.playerScript.takeDamage(damage);
        }
        //Damage for Clones
        else if (other.CompareTag("PClone") && damageable != null)
        {
            isHit = true;
            damageable.takeDamage(damage);
        }
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
