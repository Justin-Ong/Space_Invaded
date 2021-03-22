using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    public GameObject healthBarPrefab;

    private float currHealth;
    private GameObject healthBar;
    private HealthBarBehaviour myHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = Instantiate(healthBarPrefab, References.canvas.transform);
        myHealthBar = healthBar.GetComponent<HealthBarBehaviour>();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myHealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
    }

    public void TakeDamage(float value)
    {
        currHealth -= value;
        myHealthBar.UpdateHealthFraction(currHealth / maxHealth);
        if (currHealth <= 0)
        {
            Die();
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
