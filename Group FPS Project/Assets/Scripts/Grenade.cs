using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] GameObject explosion;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }

}
