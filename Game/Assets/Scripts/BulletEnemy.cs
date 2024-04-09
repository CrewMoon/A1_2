using System;
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
    private Vector2 MoveDirection;
    private GameObject target;


    private void Update()
    {
        StartCoroutine(GetMoveDirection());
    }
    
    IEnumerator GetMoveDirection()
    {
        yield return null;
        if (target.activeSelf)
        {
            MoveDirection = (target.transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(Vector3.up, MoveDirection, Vector3.forward);
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }

    }

    public void Fire()
    {
        target = GameObject.Find("Hero");
        GetComponent<AIDestinationSetter>().target = target.transform;
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

    public Vector2 getMoveDirection()
    {
        return MoveDirection;
    }
}
