using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerTracker : MonoBehaviour
{
    public Image OnScreenSprite;
    public Image OffScreenSprite;
    public List<GameObject> objects;

    Image[] onScreenSprites;
    Image[] offScreenSprites;

    void Start()
    {
        onScreenSprites = new Image[objects.Count];
        offScreenSprites = new Image[objects.Count];
        for (int i = 0; i < objects.Count; i++)
        {
            Image newOnScreenSprite = Instantiate(OnScreenSprite, transform.position, transform.rotation);
            Image newOffScreenSprite = Instantiate(OffScreenSprite, transform.position, transform.rotation);
            newOnScreenSprite.transform.SetParent(gameObject.transform);
            newOffScreenSprite.transform.SetParent(gameObject.transform);
            onScreenSprites[i] = newOnScreenSprite;
            offScreenSprites[i] = newOffScreenSprite;
        }
    }

    void LateUpdate()
    {
        PlaceIndicators();
    }

    void PlaceIndicators()
    {
        foreach (GameObject obj in objects)
        {
            Vector3 screenpos = Camera.main.WorldToScreenPoint(obj.transform.position);
            int index = objects.IndexOf(obj);
            if (screenpos.z > 0 && screenpos.x < Screen.width && screenpos.x > 0 && screenpos.y < Screen.height && screenpos.y > 0)
            {
                onScreenSprites[index].rectTransform.position = screenpos;
                onScreenSprites[index].enabled = true;
                offScreenSprites[index].enabled = false;
            }
            else
            {
                PlaceOffscreen(screenpos, offScreenSprites[index], obj);
                onScreenSprites[index].enabled = false;
                offScreenSprites[index].enabled = true;
            }
        }

    }

    void PlaceOffscreen(Vector3 screenpos, Image sprite, GameObject obj)
    {
        float x = screenpos.x;
        float y = screenpos.y;
        float offset = 10;

        if (screenpos.z < 0)
        {
            screenpos.x = Screen.width - screenpos.x;
            screenpos.y = Screen.height - screenpos.y;
        }

        if (screenpos.x > Screen.width)
        {
            x = Screen.width - offset;
        }
        if (screenpos.x < 0)
        {
            x = offset;
        }

        if (screenpos.y > Screen.height)
        {
            y = Screen.height - offset;
        }
        if (screenpos.y < 0)
        {
            y = offset;
        }

        sprite.rectTransform.position = new Vector3(x, y, 0);
    }
}
