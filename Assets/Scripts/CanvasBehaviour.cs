using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasBehaviour : MonoBehaviour
{
    private Text baseHealthDisplay;
    private HealthBar baseHealthBar;
    private GameObject gameOverDisplay;
    private Text buildModeDisplay;
    private GameObject pauseDisplay;
    private GameObject victoryDisplay;

    void Awake()
    {
        References.canvas = gameObject;
        baseHealthDisplay = gameObject.transform.Find("BaseHealthDisplay").GetComponent<Text>();
        baseHealthBar = gameObject.transform.Find("BaseHealthBar").GetComponent<HealthBar>();
        gameOverDisplay = gameObject.transform.Find("GameOverDisplay").gameObject;
        gameOverDisplay.SetActive(false);
        buildModeDisplay = gameObject.transform.Find("BuildModeDisplay").GetComponent<Text>();
        pauseDisplay = gameObject.transform.Find("PauseDisplay").gameObject;
        victoryDisplay = gameObject.transform.Find("VictoryDisplay").gameObject;
    }

    void Update()
    {
        baseHealthDisplay.text = References.defencePointObject.currBaseHealth + "/" + References.defencePointObject.maxBaseHealth;
        baseHealthBar.ShowHealthFraction(References.defencePointObject.currBaseHealth / References.defencePointObject.maxBaseHealth);
        if (References.defencePointObject.currBaseHealth <= 0)
        {
            gameOverDisplay.SetActive(true);
        }
        if (BuildManager.victoryFlag) { victoryDisplay.SetActive(true); }
        if (!BuildManager.victoryFlag) { victoryDisplay.SetActive(false); }
        if (BuildManager.buildModeFlag) { buildModeDisplay.enabled = true; }
        if (!BuildManager.buildModeFlag) { buildModeDisplay.enabled = false; }
        if (BuildManager.pauseFlag) { pauseDisplay.SetActive(true); }
        if (!BuildManager.pauseFlag) { pauseDisplay.SetActive(false); }
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Continue()
    {
        BuildManager.victoryFlag = false;
        Time.timeScale = 1;
    }
}
