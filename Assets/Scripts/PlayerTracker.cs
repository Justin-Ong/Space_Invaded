using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public GameObject ourCamera;
    public GameObject target;
    public static PlayerTracker instance;
    public float rotateSpeed = 8;
    public Vector3 maxZoom = new Vector3(25, 25, 25);
    public Vector3 minZoom = new Vector3(5, 5, 5);

    public Transform cameraTransform;
    private Transform targetTransform;
    private Quaternion originalCameraPos;

    void Start()
    {
        cameraTransform = ourCamera.transform;
        targetTransform = target.transform;
        cameraTransform.position = targetTransform.position;
        originalCameraPos = cameraTransform.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            cameraTransform.rotation = originalCameraPos;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");

            if (cameraTransform.eulerAngles.z + v <= 0.1f || cameraTransform.eulerAngles.z + v >= 179.9f)
                v = 0;

            cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y + h, cameraTransform.eulerAngles.z + v);
        }

        float scrollFactor = Input.GetAxis("Mouse ScrollWheel");

        if (scrollFactor != 0)
        {
            cameraTransform.localScale = cameraTransform.localScale * (1f - scrollFactor);
            if (cameraTransform.localScale.x > maxZoom.x)
            {
                cameraTransform.localScale = maxZoom;
            }
            else if (cameraTransform.localScale.x < minZoom.x)
            {
                cameraTransform.localScale = minZoom;
            }
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

        transform.LookAt(targetTransform.position);
    }
}
