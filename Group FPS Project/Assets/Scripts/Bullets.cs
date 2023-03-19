using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    [SerializeField] int timer;

    bool isHit;


    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHit)
        {
            isHit = true;
            gameManager.instance.playerScript.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
