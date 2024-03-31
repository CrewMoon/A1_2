using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyStation : MonoBehaviour
{
    private const int Health = 2;
    private const float CoolDownTime = 1;
    private const float RefreshTime = 4;
    private bool _canFire = true;
    private GameObject Hero;
    
    public GameObject RangedEnemyPrefab;

    private void Awake()
    {
        Hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (_canFire)
        {
            GameObject rangedEnemy = Instantiate(RangedEnemyPrefab, this.transform.position, this.transform.rotation,
                this.transform);
            rangedEnemy.GetComponent<RangedEnemy>().Fire(Hero);
            
            _canFire = false;
            StartCoroutine(CoolDown(CoolDownTime));
        }
        
    }

    private IEnumerator CoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        _canFire = true;
    }

    public void Kill()
    {
        this.gameObject.SetActive(false);
    }

    public float GetRefreshTime()
    {
        return RefreshTime;
    }
}
