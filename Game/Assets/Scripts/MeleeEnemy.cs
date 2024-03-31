using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour
{
    private const int HEALTH = 2;
    private const int DAMAGE = 1;
    private const int Range = 30;
    private const float CoolDownTime = 1f;
    private const float RefreshTime = 2f;
    private int health = HEALTH;
    private bool _canFire = true;

    private void Awake()
    {
        GetComponent<AIDestinationSetter>().target = GameObject.Find("Hero").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canFire)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            if (touchedTage == "Hero")
            {
                int heroHealth = touchedObject.GetComponent<Hero>().GetHealth();
                if (heroHealth > DAMAGE)
                {
                    touchedObject.GetComponent<Hero>().SetHealth(heroHealth - DAMAGE);
                }
                else
                {/*
                    touchedObject.SetActive(false);*/
                    Debug.Log("fail");
                }
            }

            _canFire = false;
            StartCoroutine(CoolDownAttackForSeconds(CoolDownTime));
        }
    }

    private IEnumerator CoolDownAttackForSeconds(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        _canFire = true;
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
        
        float radius = this.GetComponentInChildren<CircleCollider2D>().radius;
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - radius - offset;
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
}
