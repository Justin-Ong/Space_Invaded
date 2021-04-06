using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public float timeToLive;
    public string text;

    private TextMesh textComponent;
    private float timer;

    private void Start()
    {
        textComponent = gameObject.GetComponent<TextMesh>();
        textComponent.text = text;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
