using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private const int HEALTH = 1;
    private const int DAMAGE = 1;
    private const float CoolDownTime = 1f;
    private const int BulletSpeed = 40;
    

    public void Fire()
    {
        GetComponent<AIDestinationSetter>().target = GameObject.Find("Hero").transform;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject touchedObject = collision.gameObject;
        string touchedTage = touchedObject.tag;

        switch (touchedTage)
        {
            case "Hero":
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
                break;
            }
        }
        Destroy(this.gameObject);
    }
}
