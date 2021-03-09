using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 10f;
    private Vector3 endpointPosition;

    void Start()
    {
        endpointPosition = GameObject.FindGameObjectWithTag("Endpoint").transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = endpointPosition - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endpointPosition) < 0.4f) {
            Destroy(gameObject);
        }
    }


}
