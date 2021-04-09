using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigEnemyBehaviour : EnemyBehaviour
{
    public override void Die()
    {
        ResourceSystem.money += 100;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
