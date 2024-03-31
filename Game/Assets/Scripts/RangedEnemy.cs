using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private const int InitHeart = 1;
    private const int Damage = 1;
    private const float CoolDownTime = 1f;
    private const int BulletSpeed = 40;

    public void Fire(GameObject hero)
    {
        Rigidbody2D bulletRigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        Vector3 direction = (hero.transform.position - transform.position).normalized;
        bulletRigidbody2D.velocity = direction * BulletSpeed;
        Debug.Log("Fire: " + bulletRigidbody2D.velocity);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject touchedObject = collision.gameObject;
        string touchedTage = touchedObject.tag;

        switch (touchedTage)
        {
            case "Player":
            {
                int heroHealth = touchedObject.GetComponent<Hero>().GetHealth();
                if (heroHealth > Damage)
                {
                    touchedObject.GetComponent<Hero>().SetHealth(heroHealth - Damage);
                }
                else
                {
                    Debug.Log("hero fail");
                }

                break;
            }
            case "WreckableObstacle":
            {
                bool IsOver1Heart = false;
                if (IsOver1Heart)
                {
                    Debug.Log("heart minus 1");
                }
                else
                {
                    Destroy(touchedObject);
                }

                break;
            }
        }
        Destroy(this.gameObject);
    }
}
