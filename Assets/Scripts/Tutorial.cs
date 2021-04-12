using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private string[] textArray;
    private int index = 0;
    private int num = 16;

    private void Start()
    {
        textArray = new string[num];
        textArray[0] = "Welcome to the tutorial!";
        textArray[1] = "This stargate is where enemies spawn from. There can be more than one per level, but in the tutorial there's only one.";
        textArray[2] = "This is your space station. Its health bar is shown at the top left of the screen. Your money is displayed at the top right of the screen.";
        textArray[3] = "The game is divided into two phases, a building phase and a defending phase.";
        textArray[4] = "The building phase lasts for 60 seconds between waves. But you have more time to read the tips in this tutorial level. If you have confidence, you can press the Spacebar to skip this phase.";
        textArray[5] = "During the building phase, you will be able to see red tracers that show an estimate of the enemies' path. It's highly recommended to build turrets alongside the tracer.";
        textArray[6] = "Please use WASD to move in horizontal plane. Use Q to move down and E to move up.";
        textArray[7] = "Use Left Click to build a turret and Right Click to change the selected turret. The currently selected turret is shown in the bottom right corner. Different turrets have different abilities and costs.";
        textArray[8] = "The enemies can change their path based on where you place your turrets, make sure to check using the tracers.";
        textArray[9] = "You can use LeftAlt + Left Click to rotate your viewport. The mouse wheel is used to zoom in and out.";
        textArray[10] = "Press R to reset your camera view.";
        textArray[11] = "You can gain money by killing enemies. Sometimes, a bonus ship might appear that grants extra money when destroyed. Take the chance to kill it.";
        textArray[12] = "The goal is to defend your base for a certain number of waves. You can press Esc to pause the game or check the controls.";
        textArray[13] = "After beating all the enemy waves, you may optionally choose to continue fighting an infinite stream of enemies.";
        textArray[14] = "This is the end of the tutorial. Good luck and enjoy the game!";
        textArray[15] = "";
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return))
        {
            index += 1;
            GameObject.Find("Tutorial").GetComponent<Text>().text = textArray[index];
        }
        if (index == 1)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(1, 1, 31);
        }
        if(index == 2)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(31, 31, 1);
        }
        if (index == 15)
        {
            gameObject.SetActive(false);
        }
    }
}
