using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [Range(1, 100)] [SerializeField] int HP;
    [Range(1, 15)] [SerializeField] float playerSpeed;
    [Range(2,5)] [SerializeField] float sprintMod;
    [Range(1, 4)] [SerializeField] int jumpTimes;
    [Range(1, 15)] [SerializeField] int jumpSpeed;
    [Range(1, 70)] [SerializeField] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] MeshFilter gunModel;
    [SerializeField] MeshRenderer gunMaterial;

    [Header("-----Gun Stats-----")]
    [SerializeField] int zoomMax;
    [SerializeField] int speedZoomIn;
    [SerializeField] int speedZoomOut;


    int jumpCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;
    int HPOriginal;
    int selectedGun;
    public bool isSprinting;
    float zoomOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOriginal = HP;
        playerHpUiUpdate();
        respawnPlayer();
        zoomOrig = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        zoomCamera();
        selectGun();

        if (!gameManager.instance.isPaused)//Fix the bug when player can shoot or jump once after pausing
        { 
            if (!isShooting && Input.GetButton("Shoot"))
            {
                StartCoroutine(shoot());
            }
        }
    }
    //Movement settings
    void movement()
    {
        Sprint();

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

    void Sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }

    void zoomCamera()
    {
        if (Input.GetButton("Zoom"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomMax, Time.deltaTime * speedZoomIn);
        }
        else if (Camera.main.fieldOfView <= zoomOrig)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomOrig, Time.deltaTime * speedZoomOut);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
            }
            // Instantiate(cube, hit.point, transform.rotation);//instantiate a cube where player is looking
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }


    public void respawnPlayer()
    {
        HP = HPOriginal;
        playerHpUiUpdate();
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        playerHpUiUpdate();
        StartCoroutine(gameManager.instance.playerHit());

        if (HP <= 0)
        {
            gameManager.instance.playerDead();
        }
    }
    public void playerHpUiUpdate()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOriginal;
    }

    public void gunPickup(gunStats gunStat)
    {
        gunList.Add(gunStat);
        
        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;
        shootRate = gunStat.shootRate;

        gunModel.sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
