using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] float windSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pusBackInput(transform.forward * windSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
