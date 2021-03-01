using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float xySpeed;
    public float zSpeed;
    public GameObject turret;

    private int currTurret;

    void Start()
    {
        References.player = gameObject;
        currTurret = 0;
    }

    void Update()
    {
        Vector3 directionVector;

        if (Input.mouseScrollDelta.y < 0)
        {
            directionVector = new Vector3(0, zSpeed * Time.deltaTime, 0);
            transform.position += directionVector;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            directionVector = new Vector3(0, -zSpeed * Time.deltaTime, 0);
            transform.position += directionVector;
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            directionVector = new Vector3(Input.GetAxis("Horizontal") * xySpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * xySpeed * Time.deltaTime);
            transform.position += directionVector;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currTurret -= 1;
            if (currTurret < 0)
            {
                currTurret = References.numTurrets;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currTurret += 1;
            if (currTurret > References.numTurrets)
            {
                currTurret = 0;
            }
        }
        
        if (Input.GetMouseButtonDown(1)) // Check if RMB pressed once
        {
            GameObject newTurret = Instantiate(turret, transform.position, transform.rotation);
            newTurret.GetComponent<TurretBehaviour>().turretTypeIndex = currTurret;
            HealthSystem newTurretHealthSystem = newTurret.GetComponent<HealthSystem>();
            string turretType = References.turretTypes[currTurret];
            if (turretType == "basic")
            {
                newTurretHealthSystem.maxHealth = 10;
            }
            else if (turretType == "fast")
            {
                newTurretHealthSystem.maxHealth = 5;
            }
            else if (turretType == "slow")
            {
                newTurretHealthSystem.maxHealth = 20;
            }
        }
    }
}
