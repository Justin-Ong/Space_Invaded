using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    public GameObject healthBarPrefab;
    public float currHealth;

    private Camera mainCamera;
    private GameObject healthBar;
    private HealthBarBehaviour myHealthBar;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = Instantiate(healthBarPrefab, gameObject.transform);
        myHealthBar = healthBar.GetComponent<HealthBarBehaviour>();
        currHealth = maxHealth;
        mainCamera = Camera.main;
        isDead = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myHealthBar.transform.position = gameObject.transform.position + Vector3.up;
    }

    public void TakeDamage(float value)
    {
        currHealth -= value;
        if (currHealth <= 0 && !isDead)
        {
            Die();
            isDead = true;
        }
    }

    private void Die()
    {
        Destroy(healthBar);
        EnemyBehaviour temp = gameObject.GetComponent<EnemyBehaviour>();
        if (temp)
        {
            temp.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
