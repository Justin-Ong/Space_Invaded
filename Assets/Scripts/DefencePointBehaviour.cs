using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencePointBehaviour : MonoBehaviour
{
    public float maxBaseHealth = 100;
    public float currBaseHealth;

    void Awake()
    {
        References.defencePointObject = this;
        currBaseHealth = maxBaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (currBaseHealth <= 0) {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
