using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private int[] gridPos;
    
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
            Quaternion rotation = Quaternion.AngleAxis(90, Vector3.forward);
            GameObject bulletEnemy = Instantiate(BulletEnemyPrefab, this.transform.position, this.transform.GetChild(0).rotation * rotation,
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
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Invoke("Reset", RefreshTime);
        health = HEALTH;
    }
    
    private void Reset()
    {
        GetComponent<Image>().enabled = true;
        RefreshPosition();
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void RefreshPosition()
    {
        if (this.gridPos == null)
        {
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
        else
        {
            GridMap.Instance.ReleaseGrid(gridPos[0], gridPos[1]);
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
    }

    private void ShootingAlignment()
    {
        Vector2 heroPosition = Hero.GetComponent<RectTransform>().anchoredPosition;
        direction = (heroPosition - this.GetComponent<RectTransform>().anchoredPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        this.transform.GetChild(0).rotation = Quaternion.Slerp(this.transform.GetChild(0).rotation, targetRotation, Time.deltaTime * 8);
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
