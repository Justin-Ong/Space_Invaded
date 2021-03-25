using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float damage;
    public float speed;
    public float timeToLive;
    public LayerMask turretLayer;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody ourBody = GetComponent<Rigidbody>();
        ourBody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCollidedWith = collision.gameObject;
        if (1 << objectCollidedWith.layer == turretLayer)
        {
            objectCollidedWith.GetComponent<HealthSystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
