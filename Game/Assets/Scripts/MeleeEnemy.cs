using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private const int InitHeart = 2;
    private const int Damage = 1;
    private const int Range = 30;
    private const float CoolDownTime = 1f;
    private const float RefreshTime = 2f;
    private bool _canFire = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canFire)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            if (touchedTage == "Player")
            {
                int heroHealth = touchedObject.GetComponent<Hero>().GetHealth();
                if (heroHealth > Damage)
                {
                    touchedObject.GetComponent<Hero>().SetHealth(heroHealth - Damage);
                }
                else
                {
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

    public void Kill()
    {
        this.gameObject.SetActive(false);
    }

    public float GetRefreshTime()
    {
        return RefreshTime;
    }
}
