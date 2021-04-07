using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    public static int money;
    public int startmoney = 1000;

    void Start()
    {
        money = startmoney;
    }
}
