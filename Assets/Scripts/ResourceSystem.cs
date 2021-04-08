using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSystem : MonoBehaviour
{
    public static int money;
    public int startmoney = 1000;

    void Start()
    {
        money = startmoney;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
    }
}
