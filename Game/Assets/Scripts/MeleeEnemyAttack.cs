using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeEnemyAttack : MonoBehaviour
{
    public GameObject MeleeEnemy;
    private int DAMAGE;

    private void Start()
    {
        DAMAGE = MeleeEnemy.GetComponent<MeleeEnemy>().getDAMAGE();
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            {
                touchedObject.GetComponent<Hero>().Fail();
            }
                
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Image>().enabled = false;
            StartCoroutine(MeleeEnemy.GetComponent<MeleeEnemy>().CoolDownAttackForSeconds(MeleeEnemy.GetComponent<MeleeEnemy>().getCoolDownTime()));
        }
    }
}
