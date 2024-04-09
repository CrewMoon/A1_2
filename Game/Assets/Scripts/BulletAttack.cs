using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class BulletAttack : MonoBehaviour
    {
        private const int DAMAGE = 1;
        private const float CooldownTime = 0.5f;
        private const int BulletSpeed = 40;
        private Vector2 direction;
        private bool isFired = false;
        private GameObject hero;

        private void Awake()
        {
            hero = GameObject.Find("Hero");
        }

        private void Update()
        {
            Move();
        }

        public void Fire()
        {
            isFired = true;
            Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            switch (touchedTage)
            {
                case "NondestructibleObstacle":
                    Destroy(this.gameObject);
                    break;
                case "WreckableObstacle":
                {
                    int ObstacleHealth = touchedObject.GetComponent<WreckableObstacle>().getHealth();
                    if (ObstacleHealth > DAMAGE)
                    {
                        touchedObject.GetComponent<WreckableObstacle>().setHealth(ObstacleHealth - DAMAGE);
                    }
                    else
                    {
                        touchedObject.SetActive(false);
                    }
                    Destroy(this.gameObject);
                    break;
                }
                case "MeleeEnemy":
                {
                    int MeleeEnemyHealth = touchedObject.GetComponent<MeleeEnemy>().getHealth();
                    if (MeleeEnemyHealth > DAMAGE)
                    {
                        touchedObject.GetComponent<MeleeEnemy>().setHealth(MeleeEnemyHealth - DAMAGE);
                    }
                    else
                    {
                        touchedObject.GetComponent<MeleeEnemy>().RefreshAfterFreshTime();
                        hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 10);
                    }
                    Destroy(this.gameObject);
                    break;
                }
                case "BulletEnemy":
                {
                    Destroy(touchedObject);
                    Destroy(this.gameObject);
                    break;
                }
                case "RangedEnemy":
                {
                    touchedObject.GetComponent<RangedEnemy>().RefreshAfterFreshTime();
                    hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 20);
                    Destroy(this.gameObject);
                    break;
                }
            }
        }

        private void Move()
        {
            if (isFired)
            {
                direction = new Vector2(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
                Vector2 bulletPosition = this.GetComponent<RectTransform>().anchoredPosition;
                bulletPosition += direction * BulletSpeed * Time.deltaTime;
                this.GetComponent<RectTransform>().anchoredPosition = bulletPosition;
            }
        }
        
        public float GetCoolDownTime()
        {
            return CooldownTime;
        }

        public int GetDamage()
        {
            return DAMAGE;
        }
    }