using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public float timeToLive;
    public string text;

    private TextMesh textComponent;
    private float timer;
    private Camera mainCam;

    private void Start()
    {
        textComponent = gameObject.GetComponent<TextMesh>();
        textComponent.text = text;
        mainCam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(2 * transform.position - mainCam.transform.position);
    }
}
