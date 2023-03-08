using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;
    [Header("-----Player Stats-----")]
    [Range(1, 15)] [SerializeField] float playerSpeed;
    [Range(1, 4)] [SerializeField] int jumpTimes;
    [Range(1, 15)] [SerializeField] int jumpSpeed;
    [Range(1, 70)] [SerializeField] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject cube;

    int jumpCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        respawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        //if (!gameManager.instance.isPaused)//Fix the bug when player can shoot or jump once after pausing
        //{
        //    movement();
        //    if (!isShooting && Input.GetButton("Shoot"))
        //    {
        //        StartCoroutine(shoot());
        //    }
        //}
    }
    //Movement settings
    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpCurrent = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move = move.normalized;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpCurrent < jumpTimes)
        {
            jumpCurrent++;
            playerVelocity.y = jumpSpeed;
        }
        playerVelocity.y -= gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        //Debug.Log(move);//TrackPlayer Movement Speed
    }

    //IEnumerator shoot()
    //{
    //    isShooting = true;

    //    RaycastHit hit;
    //    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
    //    {
    //        if (hit.collider.GetComponent<IDamage>() != null)
    //        {
    //            hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
    //        }
    //        // Instantiate(cube, hit.point, transform.rotation);//instantiate a cube where player is looking
    //    }
    //    yield return new WaitForSeconds(shootRate);
    //    isShooting = false;
    //}


    public void respawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
}
