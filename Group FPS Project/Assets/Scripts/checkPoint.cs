using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    //Initalize Variables
    [SerializeField] Renderer model;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
