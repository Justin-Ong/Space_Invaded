using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public int turretTypeIndex;
    public float attackTimer;
    public GameObject bulletPrefab;

    private float currAttackTimer;

    // Start is called before the first frame update
    void Start()
    {
        string turretType = References.turretTypes[turretTypeIndex];
        if (turretType == "basic")
        {
            // Pass
        }
        else if (turretType == "fast")
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (turretType == "slow")
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
