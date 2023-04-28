using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public GameObject pauseFSelectedButton;
    public GameObject WinFSelectedButton;
    public GameObject LoseFSelectedButton;
    public GameObject MainFSelectedButton;

    [Header("-----Player Stuff-----")]
    public GameObject player;
    public GameObject playerClone;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("-----UI-----")]
    public AudioSource aud;
    public AudioClip audClip;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject optionMenu;
    public GameObject AudioMenu;
    public GameObject statsMenu;
    public GameObject checkPointMenu;
    public GameObject PlayerhitFlash;
    public GameObject PlayerlowHealth;
    public Image playerHPBar;
    public Image playerStaminaBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI totalExpText;

    [Header("-----Game Goals-----")]
    List <GameObject> enemyList = new List<GameObject>();
    public int enemiesRemaining;
    public bool finished;
    [SerializeField] int totalExperience;   
    public bool isPaused;
    public bool loadNextlevel;
    VolumeControl volume;
    Spawner spawner;
    // Start is called before the first frame update
    void Awake()
    {        
        activeMenu = AudioMenu;        
        activeMenu = null;
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawnPos");
    }
    
    // Update is called once per frame
    void Update()
    {

        //Sets clones created from player as part of game manager
        playerClone = GameObject.FindGameObjectWithTag("PClone");
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                pauseState();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseFSelectedButton);
            }
            else
                unpauseState();
        }
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpauseState()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    //Tracks the number of enemies spawned in
    public void updateGameGoal(int amount, GameObject enemy)
    {
        Scene scene = SceneManager.GetActiveScene();
        if(amount == 1) {
            enemyList.Add(enemy);
        }
        else
        {
            enemyList.Remove(enemy);
            updateEnemyTotal(amount);
        }

        //The Goal(Subject to change)

        if (enemiesRemaining <= 0 && scene.name == "Level 3" && finished == true)
            {
                StartCoroutine(youWin());
            }
        
    }

    public void updateEnemyTotal (int total)
    {
        enemiesRemaining += total;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
    }
    public void updatePlayerExperience(int total)
    {
        totalExperience += total;
        totalExpText.text = totalExperience.ToString("F0");
    }
    public void playerDead()
    {
        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LoseFSelectedButton);
    }

    public void levelFinish()
    {
        pauseState();
        activeMenu = statsMenu;
        activeMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFSelectedButton);
    }

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(3);
        pauseState();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(WinFSelectedButton);

    }

    public IEnumerator playerHit()
    {
        if (instance.playerScript.HP <= (instance.playerScript.HPOriginal * .25))
        {
            StartCoroutine(lowHealth());
        }

       PlayerhitFlash.SetActive(true);
       StopCoroutine(UIFade(PlayerhitFlash.GetComponent<Image>(), 1f, .1f));
       StartCoroutine(UIFade(PlayerhitFlash.GetComponent<Image>(), 1f, .1f));
       yield return new WaitForSeconds(.5f);
       StartCoroutine(UIFade(PlayerhitFlash.GetComponent<Image>(), 0f, .5f));
       PlayerhitFlash.SetActive(false);
    
    }

    public IEnumerator lowHealth()
    {
        while (instance.playerScript.HP <= instance.playerScript.HPOriginal * .25)
        {
            PlayerlowHealth.SetActive(true);
            StartCoroutine(UIFade(PlayerlowHealth.GetComponent<Image>(), .3f, .3f));
            yield return new WaitForSeconds(2);
            StartCoroutine(UIFade(PlayerlowHealth.GetComponent<Image>(), 1f, .3f));
            yield return new WaitForSeconds(.5f);
        }
        PlayerlowHealth.SetActive(false);   
    }

    public IEnumerator UIFade(Image I,float e_Value ,float dur)
    {
        float e_time = 0;
        float s_Value = I.color.a;
        while (e_time < dur)
        {
            e_time += Time.deltaTime;
            float N_alpha = Mathf.Lerp(s_Value, e_Value, e_time/dur);
            I.color = new Color(I.color.r, I.color.b, I.color.g, N_alpha);
            yield return null;
        }
    }
}
