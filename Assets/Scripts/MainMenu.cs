using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Level1Pressed()
    {
        StartCoroutine(LoadLevel1());
    }
    
    public void Level2Pressed()
    {
        StartCoroutine(LoadLevel2());
    }

    private IEnumerator LoadLevel1()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator LoadLevel2()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level2");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
