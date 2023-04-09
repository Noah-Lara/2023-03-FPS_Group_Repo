using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seekingProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, gameManager.instance.player.transform.position, speed * Time.deltaTime);
    }
}
