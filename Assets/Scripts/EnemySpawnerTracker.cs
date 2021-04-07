using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerTracker : MonoBehaviour
{
    public Image OnScreenEnemySprite;
    public Image OffScreenEnemySprite;
    public Image OnScreenBaseSprite;
    public Image OffScreenBaseSprite;
    public List<GameObject> enemySpawners;
    public GameObject defencePoint;

    Image[] onScreenEnemySprites;
    Image[] offScreenEnemySprites;
    Image onScreenBaseSprite;
    Image offScreenBaseSprite;

    void Start()
    {
        onScreenEnemySprites = new Image[enemySpawners.Count];
        offScreenEnemySprites = new Image[enemySpawners.Count];
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            Image newOnScreenSprite = Instantiate(OnScreenEnemySprite, transform.position, transform.rotation);
            Image newOffScreenSprite = Instantiate(OffScreenEnemySprite, transform.position, transform.rotation);
            newOnScreenSprite.transform.SetParent(gameObject.transform);
            newOffScreenSprite.transform.SetParent(gameObject.transform);
            onScreenEnemySprites[i] = newOnScreenSprite;
            offScreenEnemySprites[i] = newOffScreenSprite;
        }
        onScreenBaseSprite = Instantiate(OnScreenBaseSprite, transform.position, transform.rotation);
        offScreenBaseSprite = Instantiate(OffScreenBaseSprite, transform.position, transform.rotation);
        onScreenBaseSprite.transform.SetParent(gameObject.transform);
        offScreenBaseSprite.transform.SetParent(gameObject.transform);
    }

    void LateUpdate()
    {
        PlaceIndicators();
    }

    void PlaceIndicators()
    {
        Vector3 screenpos;
        foreach (GameObject obj in enemySpawners)
        {
            screenpos = Camera.main.WorldToScreenPoint(obj.transform.position);
            int index = enemySpawners.IndexOf(obj);
            if (screenpos.z > 0 && screenpos.x < Screen.width && screenpos.x > 0 && screenpos.y < Screen.height && screenpos.y > 0)
            {
                onScreenEnemySprites[index].rectTransform.position = screenpos;
                onScreenEnemySprites[index].enabled = true;
                offScreenEnemySprites[index].enabled = false;
            }
            else
            {
                PlaceOffscreen(screenpos, offScreenEnemySprites[index]);
                onScreenEnemySprites[index].enabled = false;
                offScreenEnemySprites[index].enabled = true;
            }
        }
        screenpos = Camera.main.WorldToScreenPoint(defencePoint.transform.position);
        if (screenpos.z > 0 && screenpos.x < Screen.width && screenpos.x > 0 && screenpos.y < Screen.height && screenpos.y > 0)
        {
            onScreenBaseSprite.rectTransform.position = screenpos;
            onScreenBaseSprite.enabled = true;
            offScreenBaseSprite.enabled = false;
        }
        else
        {
            PlaceOffscreen(screenpos, offScreenBaseSprite);
            onScreenBaseSprite.enabled = false;
            offScreenBaseSprite.enabled = true;
        }
    }

    void PlaceOffscreen(Vector3 screenpos, Image sprite)
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
