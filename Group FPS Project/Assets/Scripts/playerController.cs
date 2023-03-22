using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IPhysics
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [Range(1, 100)] [SerializeField] int HP;
    [Range(1, 15)] [SerializeField] float playerSpeed;
    [Range(2,5)] [SerializeField] float sprintMod;
    [Range(1, 10)] [SerializeField] int drainRate;
    [Range(1, 4)] [SerializeField] int jumpTimes;
    [Range(1, 15)] [SerializeField] int jumpSpeed;
    [Range(1, 70)] [SerializeField] int gravity;
    [SerializeField] int pushBackResolve;
    [Range(1, 100)] [SerializeField] int stamina;

    [Header("-----Power-Up Stats-----")]
    [SerializeField] List<powerUpStats> spellList = new List<powerUpStats>();
    [SerializeField] float spellShootRate;
    [SerializeField] int spellShootDist;
    [SerializeField] int spellShootDamage;
    [SerializeField] GameObject bulletHitEffect;

    //[Header("-----Gun Stats-----")]
    //[SerializeField] List<gunStats> gunList = new List<gunStats>();
    //[SerializeField] float shootRate;
    //[SerializeField] int shootDist;
    //[SerializeField] int shootDamage;
    //[SerializeField] MeshFilter gunModel;
    //[SerializeField] MeshRenderer gunMaterial;
    //[SerializeField] GameObject bulletHitEffect;

    [Header("-----Zoom-----")]
    [SerializeField] int zoomMax;
    [SerializeField] int speedZoomIn;
    [SerializeField] int speedZoomOut;


    int jumpCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;
    int HPOriginal;
    int StaminaOrig;
    float playerSpeedOrig;

    //int selectedGun;
    int selectedSpell;

    public bool isSprinting;
    float zoomOrig;

    Vector3 pushBack;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOriginal = HP;
        StaminaOrig =  stamina;
        playerHpUiUpdate();
        respawnPlayer();
        zoomOrig = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        zoomCamera();
        //selectGun();
        selectSpell();

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
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);

        if (Input.GetButtonDown("Sprint") && stamina != 0)
        {
            StartCoroutine(Dash());
            gameManager.instance.playerStaminaBar.transform.parent.gameObject.SetActive(true);
        }

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

        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        //Debug.Log(move);//TrackPlayer Movement Speed
    }

    
    IEnumerator Dash()
    {
        if (playerSpeed != playerSpeedOrig * sprintMod)
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
            StartCoroutine(staminaDrain());

            yield return new WaitForSeconds(drainRate / 2);

            isSprinting = false;
            playerSpeed /= sprintMod;
            StartCoroutine(staminaRecharge());
        }
    }

    IEnumerator staminaRecharge()
    {
        //adds a point back to the Stamina Pool
        yield return new WaitForSeconds(drainRate);
        if (stamina < StaminaOrig)
        {
          stamina ++;
          playerSTMUiUpdate();
        }
        yield return new WaitForSeconds(drainRate /2 );
        gameManager.instance.playerStaminaBar.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator staminaDrain()
    {
        //Subtracts point from the stamina pool
        stamina--;
        playerSTMUiUpdate();
        yield return new WaitForSeconds(drainRate);
        
    }

    public void takeForce(Vector3 direction, int damage)
    {
        pushBack += direction;
        takeDamage(damage);
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
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, spellShootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(spellShootDamage);
            }

            Instantiate(bulletHitEffect, hit.point, bulletHitEffect.transform.rotation);
        }
        yield return new WaitForSeconds(spellShootRate);
        isShooting = false;
    }


    public void respawnPlayer()
    {
        pushBack = Vector3.zero;
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

    public void playerSTMUiUpdate()
    {
        gameManager.instance.playerStaminaBar.fillAmount = (float)stamina / (float)StaminaOrig;
    }

    //public void gunPickup(gunStats gunStat)
    //{
    //    gunList.Add(gunStat);
    //    
    //    shootDamage = gunStat.shootDamage;
    //    shootDist = gunStat.shootDist;
    //    shootRate = gunStat.shootRate;
    //
    //    gunModel.sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
    //    gunMaterial.sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    //
    //    selectedGun = gunList.Count - 1;
    //}

    public void spellPickup(powerUpStats powerUpStat)
    {
        spellList.Add(powerUpStat);

        spellShootDamage = powerUpStat.shootDamage;
        spellShootDist = powerUpStat.shootDist;
        spellShootRate = powerUpStat.shootRate;
        bulletHitEffect = powerUpStat.bulletHitEffect;

        selectedSpell = spellList.Count - 1;
    }

    //void selectGun()
    //{
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
    //    {
    //        selectedGun++;
    //        changeGun();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
    //    {
    //        selectedGun--;
    //        changeGun();
    //    }
    //}

    void selectSpell()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedSpell < spellList.Count - 1)
        {
            selectedSpell++;
            changeSpell();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedSpell > 0)
        {
            selectedSpell--;
            changeSpell();
        }
    }

    //void changeGun()
    //{
    //    shootDamage = gunList[selectedGun].shootDamage;
    //    shootDist = gunList[selectedGun].shootDist;
    //    shootRate = gunList[selectedGun].shootRate;
    //
    //    gunModel.sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
    //    gunMaterial.sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    //}

    void changeSpell()
    {
        spellShootDamage = spellList[selectedSpell].shootDamage;
        spellShootDist = spellList[selectedSpell].shootDist;
        spellShootRate = spellList[selectedSpell].shootRate;
        bulletHitEffect = spellList[selectedSpell].bulletHitEffect;

        selectedSpell = spellList.Count - 1;
    }
}
