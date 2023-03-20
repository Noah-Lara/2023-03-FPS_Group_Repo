using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("-----Player Stuff-----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("-----UI-----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject checkPointMenu;
    public GameObject PlayerhitFlash;
    public Image playerHPBar;
    public Image playerStaminaBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI bossRemainingText;

    [Header("-----Game Goals-----")]
    public int enemiesRemaining;
    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawnPos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
                pauseState();
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
    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");

        //The Goal(Subject to change)
        if (enemiesRemaining <= 0)
        {
            StartCoroutine(youWin());
        }
    }
    public void playerDead()
    {
        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(3);
        pauseState();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }

    public IEnumerator playerHit()
    {
        PlayerhitFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        PlayerhitFlash.SetActive(false);
    }
}
