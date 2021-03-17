using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public Transform trackedObject;
    public float maxDistance = 50;
    public float moveSpeed = 5;
    public float updateSpeed = 1;
    [Range(0, 10)]
    public float currentDistance = 3;
    public float hideDistance = 1.5f;

    private string moveAxis = "Mouse ScrollWheel";

    void Start()
    {
        
    }

    void LateUpdate()
    {
        currentDistance += Input.GetAxisRaw(moveAxis) * moveSpeed * Time.deltaTime * 100;
        currentDistance = Mathf.Clamp(currentDistance, 0, maxDistance);
        transform.position = Vector3.MoveTowards(transform.position, trackedObject.position + Vector3.up * currentDistance - trackedObject.forward * (currentDistance + maxDistance * 0.5f), updateSpeed * Time.deltaTime);
        transform.LookAt(trackedObject);
    }
}
