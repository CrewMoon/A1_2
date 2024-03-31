using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemy : MonoBehaviour
{
    private const int HEALTH = 1;
    private const float CoolDownTime = 1;
    private const float RefreshTime = 4;
    private bool _canFire = true;
    private int health = HEALTH;
    private GameObject Hero;
    private Vector2 direction;
    
    public GameObject BulletEnemyPrefab;

    private void Awake()
    {
        Hero = GameObject.Find("Hero");
    }

    private void Update()
    {
        ShootingAlignment();
        Attack();
    }

    private void Attack()
    {
        if (_canFire)
        {
            GameObject bulletEnemy = Instantiate(BulletEnemyPrefab, this.transform.position, this.transform.GetChild(0).rotation,
                this.transform);
            bulletEnemy.GetComponent<BulletEnemy>().Fire();
            
            _canFire = false;
            StartCoroutine(CoolDown(CoolDownTime));
        }
        
    }

    private IEnumerator CoolDown(float coolDownTime)
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

    private void ShootingAlignment()
    {
        Vector2 heroPosition = Hero.GetComponent<RectTransform>().anchoredPosition;
        direction = (heroPosition - this.GetComponent<RectTransform>().anchoredPosition).normalized;
        this.transform.GetChild(0).rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
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
