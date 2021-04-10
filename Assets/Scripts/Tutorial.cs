using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private string[] textArray;
    private int index = 0;
    private int num = 14;

    private void Start()
    {
        textArray = new string[num];
        textArray[0] = "Welcome to the tutorial level! This level is used for helping you to warm up.";
        textArray[1] = "Please use WASD to move in horizontal plane and use Q to move down and E to move up.";
        textArray[2] = "The health bar of the base is shown on the top left of the screen and the money to buy turrets is at the top right corner.";
        textArray[3] = "The Stargate which is highlighted by the red box and at the bottom left corner is where enemies spawn.";
        textArray[4] = "Enemies spawn by waves and the game is divided two phases: Build phase and Defence phase.";
        textArray[5] = "In the build phase you can see a red line tracer which shows the path of the enemies.  It's highly recommended to build turrets alongside the tracer.";
        textArray[6] = "The information of turret is shown in the bottom right corner. Different turrets have different abilities and costs.";
        textArray[7] = "Use Left Click to build a turret and Right Click to select turrets. Remember to use the recalculation button to check the path because the enemies are effected by your turrets.";
        textArray[8] = "You can use LeftAlt + Mouse Left button to rotate your viewport. Mouse wheel is used to zoom.";
        textArray[9] = "The build phase lasts for 60 seconds between waves. If you have confidence, you can press the Spacebar to skip this phase.";
        textArray[10] = "You can gain money by killing enemies and sometimes there's bonus ship heading around. Take the chance to kill it.";
        textArray[11] = "The goal is to defend your base for a certain amount of waves. You can press Esc to pause the game or check the settings.";
        textArray[12] = "This is the end of the tutorial. Good luck and enjoy the game!";
    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (index > num)
            {
                GameObject.Find("Continue").SetActive(false);
                return;
            }

            index += 1;
            GameObject.Find("Tutorial").GetComponent<Text>().text = textArray[index];
        }

        if(index == 3)
        {
            PlayerTracker.instance.cameraTransform.position = new Vector3(1, 1, 31);
        }
    }
}
