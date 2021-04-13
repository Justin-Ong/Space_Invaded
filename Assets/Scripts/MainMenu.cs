using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    public GameObject loadingScreen;

    private Slider loadingBar;

    private void Start()
    {
        credits.SetActive(false);
        loadingScreen.SetActive(false);
        loadingBar = loadingScreen.transform.Find("LoadingBar").GetComponent<Slider>();
    }

    private void Update()
    {
        if (credits.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            credits.SetActive(false);
        }
    }

    public void TutorialPressed()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadTutorial());
    }
    
    public void Level1Pressed()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevel1());
    }

    public void CreditsPressed()
    {
        credits.SetActive(true);
    }

    public void ExitPressed()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator LoadTutorial()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Tutorial");

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }
    }

    private IEnumerator LoadLevel1()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }
    }
}
