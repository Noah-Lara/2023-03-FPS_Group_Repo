using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Platform : MonoBehaviour
{
    [SerializeField] private List<Transform> Waypoints;
    [SerializeField] private float mspeed;
    private int currentWP;

    // Start is called before the first frame update
    private void Start()
    {
        if (Waypoints.Count <= 0)
        {
            return;
        }
        else
        {
            currentWP = 0;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {

        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentWP].transform.position,(mspeed * Time.deltaTime));

        if (Vector3.Distance(Waypoints[currentWP].transform.position, transform.position) <= 0)
        {
            currentWP++;
        }

        if (currentWP != Waypoints.Count)
        { 
            return; 
        }
        else
        {
            Waypoints.Reverse();
            currentWP = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
