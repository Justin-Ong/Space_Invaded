using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastEnemyBehaviour : EnemyBehaviour
{
    public override void Die()
    {
        ResourceSystem.money += 25;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
