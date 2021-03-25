using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehaviour : MonoBehaviour
{
    private Text baseHealthDisplay;
    private Text gameOverDisplay;
    private Text buildModeDisplay;
    private GameObject pauseDisplay;

    void Awake()
    {
        References.canvas = gameObject;
        baseHealthDisplay = gameObject.transform.Find("BaseHealthDisplay").GetComponent<Text>();
        gameOverDisplay = gameObject.transform.Find("GameOverDisplay").GetComponent<Text>();
        gameOverDisplay.enabled = false;
        buildModeDisplay = gameObject.transform.Find("BuildModeDisplay").GetComponent<Text>();
        pauseDisplay = gameObject.transform.Find("PauseDisplay").gameObject;
    }

    void Update()
    {
        baseHealthDisplay.text = References.defencePointObject.currBaseHealth + "/" + References.defencePointObject.maxBaseHealth;
        if (References.defencePointObject.currBaseHealth <= 0)
        {
            gameOverDisplay.enabled = true;
        }
        if (BuildManager.buildModeFlag) { buildModeDisplay.enabled = true; }
        if (!BuildManager.buildModeFlag) { buildModeDisplay.enabled = false;}
        if (BuildManager.pauseFlag) { pauseDisplay.SetActive(true); }
        if (!BuildManager.pauseFlag) { pauseDisplay.SetActive(false);}
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
