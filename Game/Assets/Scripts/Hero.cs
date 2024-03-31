using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private int Health;
    private const int Velocity = 50;
    private bool canFireRangedAttack;
    private bool canFireMeleeAttack;
    private float MeleeAttackDestroryTime = 0.5f;
    private int Score;

    
    public GameObject MeleeAttackPrefab;
    public GameObject RangedAttackPrefab;



    private void Awake()
    {
        Refresh();
    }

    private void Update()
    {
        Rigidbody2D heroRigidbody2D = this.GetComponent<Rigidbody2D>();
        if (Input.GetMouseButtonDown(0))
        {
            RangedAttack();
        }else if (Input.GetMouseButtonDown(1))
        {
            MeleeAttack();
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            heroRigidbody2D.velocity = Vector2.right * Velocity;
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            heroRigidbody2D.velocity = Vector2.left * Velocity;
            // transform.Translate(Vector3.left * Velocity * Time.deltaTime);
        }else if (Input.GetKeyDown(KeyCode.W))
        {
            heroRigidbody2D.velocity = Vector2.up * Velocity;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            heroRigidbody2D.velocity = Vector2.down * Velocity;
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                Debug.Log("Start");
            }
            else
            {
                Time.timeScale = 0f;
                Debug.Log("Pause");
            }
        }
    }

    private void Refresh()
    {
        Health = 30;
        canFireRangedAttack = true;
        canFireMeleeAttack = true;
        Score = 0;
    }

    private void RangedAttack()
    {
        if (canFireRangedAttack)
        {
            GameObject rangedAttack = Instantiate(RangedAttackPrefab, this.transform.position, this.transform.rotation, this.transform.GetChild(0));
            rangedAttack.GetComponent<RangedAttack>().Fire();
            canFireRangedAttack = false;
            Invoke("ResetRangedAttack", rangedAttack.GetComponent<RangedAttack>().GetCoolDownTime());
        }
    }

    private void MeleeAttack()
    {
        if (canFireMeleeAttack)
        {
            GameObject meleeAttack = Instantiate(MeleeAttackPrefab, this.transform.position, this.transform.rotation, this.transform.GetChild(1));
            Destroy(meleeAttack, MeleeAttackDestroryTime);
            canFireMeleeAttack = false;
            Invoke("ResetMeleeAttack", meleeAttack.GetComponent<MeleeAttack>().GetCoolDownTime());
        }
    }
    private void ResetRangedAttack()
    {
        canFireRangedAttack = true;
    }

    private void ResetMeleeAttack()
    {
        canFireMeleeAttack = true;
    }

    public void SetHealth(int health)
    {
        Health = health;
    }

    public int GetHealth()
    {
        return Health;
    }

    public void SetScore(int score)
    {
        Score = score;
    }

    public int GetScore()
    {
        return Score;
    }
    
}
