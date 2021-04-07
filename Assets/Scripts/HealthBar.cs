using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image filledPart;
    public Image background;

    public void ShowHealthFraction(float fraction)
    {
        if (fraction < 0)
        {
            fraction = 0;
        }
        filledPart.rectTransform.localScale = new Vector3(fraction, 1, 1);
    }
}
