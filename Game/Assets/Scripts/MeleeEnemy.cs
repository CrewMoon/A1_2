using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour
{
    private const int HEALTH = 2;
    private const int DAMAGE = 1;
    private const int Range = 30;
    private const float CoolDownTime = 1f;
    private const float RefreshTime = 2f;
    private int health = HEALTH;
    private GameObject hero;

    private void Awake()
    {
        hero = GameObject.Find("Hero");
        GetComponent<AIDestinationSetter>().target = hero.transform;
    }

    private void Update()
    {
        if (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition,
                hero.GetComponent<RectTransform>().anchoredPosition) < 50)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public IEnumerator CoolDownAttackForSeconds(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void RefreshAfterFreshTime()
    {
        Invoke("RefreshPosition", RefreshTime);
        health = HEALTH;
    }
    
    public void RefreshPosition()
    {
        GameObject parent = GameObject.Find("Map");
        float offset = 20f;
            
        float width = this.GetComponent<RectTransform>().rect.width;
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - width / 2 - offset;
        Vector2 position = new Vector2();
        bool walkable = false;
    
        
        while (!walkable)
        {
            position.x = Random.Range(-rangeRadius, rangeRadius);
            position.y = Random.Range(-rangeRadius, rangeRadius);
        
            if (AstarPath.active.GetNearest(position).node.Walkable)
            {
                walkable = true;
                this.GetComponent<RectTransform>().anchoredPosition = position;
            }
        }
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public int getDAMAGE()
    {
        return DAMAGE;
    }

    public float getCoolDownTime()
    {
        return CoolDownTime;
    }
}
