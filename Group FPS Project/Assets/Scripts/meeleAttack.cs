using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meeleAttack : MonoBehaviour
{
    public int damage;
   
    //[SerializeField] GameObject hitEffect;

    bool isHit;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            gameManager.instance.playerScript.takeDamage(damage);
        }

        //Instantiate(hitEffect, transform.position, transform.rotation);
        
    }
}
