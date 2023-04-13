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
    public GameObject checkPointMenu;
    public GameObject PlayerhitFlash;
    public Image playerHPBar;
    public Image playerStaminaBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI totalExpText;

    [Header("-----Game Goals-----")]
    List <GameObject> enemyList = new List<GameObject>();
    public int enemiesRemaining;
    [SerializeField] int totalExperience;

    public bool isPaused;
    public bool loadNextlevel;

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

            if (enemiesRemaining <= 0 && scene.name == "Level 2")
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
        PlayerhitFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        PlayerhitFlash.SetActive(false);
    }

}
