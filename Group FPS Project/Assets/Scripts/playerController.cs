using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IPhysics, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPos;

    [Header("-----Player Stats-----")]
    [Range(1, 100)] [SerializeField] public int HP;
    [Range(1, 15)] [SerializeField] public float playerSpeed;
    [Range(2,5)] [SerializeField] float sprintMod;
    [Range(1, 10)] [SerializeField] int drainRate;
    [Range(1, 4)] [SerializeField] int jumpTimes;
    [Range(1, 15)] [SerializeField] public int jumpSpeed;
    [Range(1, 70)] [SerializeField] int gravity;
    [SerializeField] int pushBackResolve;
    [Range(1, 100)] [SerializeField] int stamina;
    [SerializeField] float uiBarSpeed;
    [SerializeField] GameObject playerClone;
    [SerializeField] int projectileSpeed;
    [SerializeField] float bulletSpeedY;



    [Header("-----Power-Up Stats-----")]
    [SerializeField] List<powerUpStats> spellList = new List<powerUpStats>();
    [SerializeField] float spellShootRate;
    [SerializeField] int spellShootDist;
    [SerializeField] public int spellShootDamage;
    [SerializeField] GameObject bulletHitEffect;
    [SerializeField] MeshFilter spellModel;
    [SerializeField] MeshRenderer spellMaterial;

    [Header("-----Zoom-----")]
    [SerializeField] int zoomMax;
    [SerializeField] int speedZoomIn;
    [SerializeField] int speedZoomOut;

    [Header("-----Audio-----")]
    [SerializeField] AudioClip[] audJump;
    [Range (0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)] [SerializeField] float audWalkVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)] [SerializeField] float audDamageVol;

    int jumpCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;
    public int HPOriginal;
    int StaminaOrig;
    public float playerSpeedOrig;
    Camera cam;
    //int selectedGun;
    int selectedSpell;
    Vector3 destination;
    public bool isSprinting;
    float zoomOrig;
    bool isPlayingFootsteps;
   
   


    Vector3 pushBack;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeedOrig = playerSpeed;
        HPOriginal = HP;
        StaminaOrig =  stamina;
        StartCoroutine(playerHpUiUpdate());
        respawnPlayer();
        zoomOrig = Camera.main.fieldOfView;

       

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        zoomCamera();
        selectSpell();

        if (!gameManager.instance.isPaused)//Fix the bug when player can shoot or jump once after pausing
        { 
            if (!isShooting && Input.GetButton("Shoot"))
            {
                StartCoroutine(shoot());
            }
        }
        //Checks if a clone already exist before making one
        if (!GameObject.FindWithTag("PClone") && Input.GetButton("Ab"))
        {
            CreateClone();
        }
    }
    //Movement settings
    void movement()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);

        if (!isPlayingFootsteps && move.normalized.magnitude > 0.5f && controller.isGrounded)
            StartCoroutine(playFootSteps());

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

        move = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        move = move.normalized;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpCurrent < jumpTimes)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
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
          StartCoroutine(playerSTMUiUpdate());
        }
        yield return new WaitForSeconds(drainRate /2 );
        gameManager.instance.playerStaminaBar.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator staminaDrain()
    {
        //Subtracts point from the stamina pool
        stamina--;
        StartCoroutine(playerSTMUiUpdate());
        yield return new WaitForSeconds(drainRate);
        
    }

    IEnumerator playFootSteps()
    {
        isPlayingFootsteps = true;

        aud.PlayOneShot(audWalk[Random.Range(0, audWalk.Length)], audWalkVol);
        
        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.3f);

        isPlayingFootsteps = false;
    }

    public void takeForce(Vector3 direction)
    {
        pushBack += direction;
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
            GameObject projectileClone = Instantiate(projectile, shootPos.position, Quaternion.identity);
            destination = hit.point;
            projectileClone.GetComponent<Rigidbody>().velocity = (destination - shootPos.position).normalized * projectileSpeed;
        }
        yield return new WaitForSeconds(spellShootRate);
        isShooting = false;
    }

   



    public void respawnPlayer()
    {
        pushBack = Vector3.zero;
        HP = HPOriginal;
        StartCoroutine(playerHpUiUpdate());
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
    public void recoverHealth(float amount)
    {
        StartCoroutine(playerHpUiUpdate());
        if (HP < HPOriginal)
        {
            HP += (int)(HPOriginal * amount);
            if (HP > HPOriginal)
                HP = HPOriginal;
        }        
        
    }
    public void recoverStamina(float amount)
    {
        StartCoroutine(playerSTMUiUpdate());
        if (stamina < StaminaOrig)
        {
            stamina += (int)(StaminaOrig * amount);
            if (stamina > StaminaOrig)
                stamina = StaminaOrig;
        }

    }
    public void takeDamage(int dmg)
    {
        StartCoroutine(playerHpUiUpdate());
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        HP -= dmg;
        StartCoroutine(gameManager.instance.playerHit());

        if (HP <= 0)
        {
            gameManager.instance.playerDead();
        }
    }
    IEnumerator playerHpUiUpdate()
    {
        float timePassed = 0;
        while (timePassed < uiBarSpeed)
        {
            gameManager.instance.playerHPBar.fillAmount = Mathf.Lerp(gameManager.instance.playerHPBar.fillAmount,((float)HP / (float)HPOriginal), timePassed / uiBarSpeed);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator playerSTMUiUpdate()
    {
        float timePassed = 0;
        while (timePassed < uiBarSpeed)
        {
            gameManager.instance.playerStaminaBar.fillAmount = Mathf.Lerp(gameManager.instance.playerStaminaBar.fillAmount, ((float)stamina/ (float)StaminaOrig), timePassed / uiBarSpeed);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    public void spellPickup(powerUpStats powerUpStat)
    {
        spellList.Add(powerUpStat);

        spellShootDamage = powerUpStat.shootDamage;
        spellShootDist = powerUpStat.shootDist;
        spellShootRate = powerUpStat.shootRate;
        bulletHitEffect = powerUpStat.bulletHitEffect;

        spellModel.sharedMesh = powerUpStat.spellModel.GetComponent<MeshFilter>().sharedMesh;
        spellMaterial.sharedMaterial = powerUpStat.spellModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedSpell = spellList.Count - 1;
    }

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

    void changeSpell()
    {
        spellShootDamage = spellList[selectedSpell].shootDamage;
        spellShootDist = spellList[selectedSpell].shootDist;
        spellShootRate = spellList[selectedSpell].shootRate;
        bulletHitEffect = spellList[selectedSpell].bulletHitEffect;

        spellModel.sharedMesh = spellList[selectedSpell].spellModel.GetComponent<MeshFilter>().sharedMesh;
        spellMaterial.sharedMaterial = spellList[selectedSpell].spellModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    //Creates clone from clones orginal spawn location
    void CreateClone()
    {
        if (Input.GetButton("Ab") && playerClone != null)
        {
            Vector3 spawnPos = gameManager.instance.player.transform.position + gameManager.instance.player.transform.forward * 2;
            GameObject playerCloneVis = Instantiate(playerClone, spawnPos, playerClone.transform.rotation);
            playerCloneVis = GameObject.FindWithTag("PClone");
        }
    }
}
