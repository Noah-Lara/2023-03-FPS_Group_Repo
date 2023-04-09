using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    [SerializeField] int timer;
    [SerializeField] GameObject hitEffect;
    

    bool isHit;


    // Start is called before the first frame update
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHit)
        {
            isHit = true;
            gameManager.instance.playerScript.takeDamage(damage);
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
