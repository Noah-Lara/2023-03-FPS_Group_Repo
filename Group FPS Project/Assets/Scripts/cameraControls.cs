using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [Range(200, 400)][SerializeField] int sensHor;
    [Range(200, 400)] [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        //makes cursor invisible
        Cursor.visible = false;
        //keeps cursor inside the game
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        //xRotation += mouseY;

        //clamp the camera rotation
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        //rotate the camera on the X-axis 
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player on its Y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
