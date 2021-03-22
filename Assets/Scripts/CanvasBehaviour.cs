using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehaviour : MonoBehaviour
{
    private Text baseHealthDisplay;
    private Text gameOverDisplay;

    void Awake()
    {
        References.canvas = gameObject;
        baseHealthDisplay = gameObject.transform.Find("BaseHealthDisplay").GetComponent<Text>();
        gameOverDisplay = gameObject.transform.Find("GameOverDisplay").GetComponent<Text>();
        gameOverDisplay.enabled = false;
    }

    void Update()
    {
        baseHealthDisplay.text = References.defencePointObject.currBaseHealth + "/" + References.defencePointObject.maxBaseHealth;
        if (References.defencePointObject.currBaseHealth <= 0)
        {
            gameOverDisplay.enabled = true;
        }
    }
}
