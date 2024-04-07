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
            /*Vector2 worldMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mousePosition =
                transform.InverseTransformPoint(Camera.main.ScreenToViewportPoint(worldMousePosition));
            Vector2 originalPosition = this.GetComponent<RectTransform>().anchoredPosition;
            this.transform.position = mousePosition;
            Vector2 newPosition = this.GetComponent<RectTransform>().anchoredPosition;
            
            direction = (this.GetComponent<RectTransform>().anchoredPosition - originalPosition).normalized;
            this.GetComponent<RectTransform>().anchoredPosition = originalPosition;
            float angleParent = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angleParent);
            float angleChild = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
            this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleChild);*/
            isFired = true;
            Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            switch (touchedTage)
            {
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
                        touchedObject.SetActive(false);
                        touchedObject.GetComponent<MeleeEnemy>().RefreshAfterFreshTime();
                        touchedObject.SetActive(true);
                    }
                    hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 10);
                    break;
                }
                case "BulletEnemy":
                {
                    Destroy(touchedObject);
                    break;
                }
                case "RangedEnemy":
                {
                    touchedObject.SetActive(false);
                    touchedObject.GetComponent<RangedEnemy>().RefreshAfterFreshTime();
                    touchedObject.SetActive(true);
                    hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 20);
                    break;
                }
            }
            Destroy(this.gameObject);
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
        
    }