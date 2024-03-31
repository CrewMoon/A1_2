using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


    public class RangedAttack : MonoBehaviour
    {
        private const int Damage = 2;
        private const float CooldownTime = 0.5f;
        private const int BulletSpeed = 20;
        

        public void Fire()
        {
            Rigidbody2D bulletRigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            bulletRigidbody2D.velocity = direction * BulletSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            switch (touchedTage)
            {
                case "Player":
                    return;
                case "WreckableObstacle":
                {
                    bool IsOver2Heart = false;
                    if (IsOver2Heart)
                    {
                        Debug.Log("heart minus 2");
                    }
                    else
                    {
                        Destroy(touchedObject);
                    }

                    break;
                }
                case "RangedEnemy":
                    Destroy(touchedObject);
                    break;
                case "MeleeEnemy":
                {
                    touchedObject.GetComponent<MeleeEnemy>().Kill();
                    StartCoroutine(RefreshMeleeEnemy(touchedObject,
                        touchedObject.GetComponent<MeleeEnemy>().GetRefreshTime()));
                    break;
                }
                case "RangedEnemyStation":
                {
                    touchedObject.GetComponent<RangedEnemyStation>().Kill();
                    StartCoroutine(RefreshRangedEnemyStation(touchedObject,
                        touchedObject.GetComponent<RangedEnemyStation>().GetRefreshTime()));
                    break;
                }
            }
            Destroy(this.gameObject);
        }

        IEnumerator RefreshRangedEnemyStation(GameObject rangedEnemyStation, float refrshTime)
        {
            yield return new WaitForSeconds(refrshTime);
            rangedEnemyStation.SetActive(true);
            
            GameObject Canvas = GameObject.Find("Canvas");
            GameObject Parent = GameObject.Find("RangedEnemyStations");
            rangedEnemyStation.transform.position = Canvas.GetComponent<SceneManager>().RefreshBoxPosition(this.gameObject, Parent);
        }
        
        IEnumerator RefreshMeleeEnemy(GameObject meleeEnemy, float refrshTime)
        {
            yield return new WaitForSeconds(refrshTime);
            meleeEnemy.SetActive(true);
            
            GameObject Canvas = GameObject.Find("Canvas");
            GameObject Parent = GameObject.Find("MeleeEnemies");
            meleeEnemy.transform.position = Canvas.GetComponent<SceneManager>().RefreshBoxPosition(this.gameObject, Parent);
        }

        public float GetCoolDownTime()
        {
            return CooldownTime;
        }
    }