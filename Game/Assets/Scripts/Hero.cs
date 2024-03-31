using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Microsoft.Unity.VisualStudio.Editor;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Hero : MonoBehaviour
{
    private const int HEALTH = 30;
    private const int Velocity = 50;
    private const float MeleeAttackDestroryTime = 0.5f;
    private const float RequiredSurviveTime = 30f;
    private int health = HEALTH;
    private bool canFireRangedAttack;
    private bool canFireMeleeAttack;
    private bool isPositionFreshed = false;
    private float AlreadySurviveTime;
    private int Score;
    private float RangedAttackCoolDownTime;
    private float MeleeAttackCoolDownTime;
    
    public GameObject MeleeAttackPrefab;
    public GameObject RangedAttackPrefab;
    public GameObject RangedSkill;
    public GameObject MeleeSkill;
    public GameObject TimeIndicator;
    public GameObject ScoreIndicator;
    public GameObject LifeIndicator;
    public Image whycan;



    private void Awake()
    {
        Refresh();
    }

    private void Update()
    {
        KeyCodeResponse();
        SurviveTimeUpdate();
        RangedAttackCoolDown();
        MeleeAttackCoolDown();
    }

    private void Refresh()
    {
        health = 30;
        canFireRangedAttack = true;
        canFireMeleeAttack = true;
        AlreadySurviveTime = 0;
        Score = 0;
        RefreshPosition();
        isPositionFreshed = true;
        RangedAttackCoolDownTime = RangedAttackPrefab.GetComponent<BulletAttack>().GetCoolDownTime();
        MeleeAttackCoolDownTime = MeleeAttackPrefab.GetComponent<MeleeAttack>().GetCoolDownTime();
    }

    public void RefreshPosition()
    {
        GameObject parent = GameObject.Find("Map");
        float offset = 20f;
        
        float width = this.GetComponent<RectTransform>().rect.width;
        
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - width / 2 - offset;
        
        Vector2 position = new Vector2()
        {
            x = Random.Range(-rangeRadius, rangeRadius),
            y = Random.Range(-rangeRadius, rangeRadius)
        };
        
        if (AstarPath.active.GetNearest(position).node.Walkable)
        {
            this.GetComponent<RectTransform>().anchoredPosition  = position;
        }
        else
        {
            RefreshPosition();
        }
    }

    private void KeyCodeResponse()
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
        }else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) ||
                  Input.GetKeyUp(KeyCode.S))
        {
            heroRigidbody2D.velocity = Vector2.zero;
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

    private void SurviveTimeUpdate()
    {
        if (isPositionFreshed)
        {
            if (AlreadySurviveTime >= RequiredSurviveTime)
            {
                AlreadySurviveTime = 0;
                isPositionFreshed = false;
                Debug.Log("do refresh action");
            }
            else
            {
                AlreadySurviveTime += Time.deltaTime;
            }
        }
    }

    private void RangedAttack()
    {
        if (canFireRangedAttack)
        {
            GameObject rangedAttack = Instantiate(RangedAttackPrefab, this.transform.position, this.transform.rotation, this.transform.GetChild(0));
            rangedAttack.GetComponent<BulletAttack>().Fire();
            canFireRangedAttack = false;
            Invoke("ResetRangedAttack", rangedAttack.GetComponent<BulletAttack>().GetCoolDownTime());
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

    private void RangedAttackCoolDown()
    {
        if (!canFireRangedAttack)
        {
            GameObject TimeText = RangedSkill.transform.GetChild(0).GameObject();
            GameObject CoolDownMask = RangedSkill.transform.GetChild(2).GameObject();
            if (RangedAttackCoolDownTime > 0)
            {
                TimeText.SetActive(true);
                CoolDownMask.SetActive(true);
                RangedAttackCoolDownTime -= Time.deltaTime;
                TimeText.GetComponent<TMP_Text>().text = RangedAttackCoolDownTime.ToString();
                CoolDownMask.GetComponent<Image>().fillAmount = RangedAttackCoolDownTime /
                                                                RangedAttackPrefab.GetComponent<BulletAttack>()
                                                                    .GetCoolDownTime();
            }
            else
            {
                TimeText.SetActive(false);
                CoolDownMask.SetActive(false);
                RangedAttackCoolDownTime = RangedAttackPrefab.GetComponent<BulletAttack>().GetCoolDownTime();
                canFireRangedAttack = true;
            }
        }
    }
    
    private void MeleeAttackCoolDown()
    {
        if (!canFireMeleeAttack)
        {
            GameObject TimeText = MeleeSkill.transform.GetChild(0).GameObject();
            GameObject CoolDownMask = MeleeSkill.transform.GetChild(2).GameObject();
            if (MeleeAttackCoolDownTime > 0)
            {
                TimeText.SetActive(true);
                CoolDownMask.SetActive(true);
                MeleeAttackCoolDownTime -= Time.deltaTime;
                TimeText.GetComponent<TMP_Text>().text = MeleeAttackCoolDownTime.ToString();
                CoolDownMask.GetComponent<Image>().fillAmount = MeleeAttackCoolDownTime /
                                                                MeleeAttackPrefab.GetComponent<MeleeAttack>()
                                                                    .GetCoolDownTime();
            }
            else
            {
                TimeText.SetActive(false);
                CoolDownMask.SetActive(false);
                MeleeAttackCoolDownTime = MeleeAttackPrefab.GetComponent<MeleeAttack>().GetCoolDownTime();
                canFireMeleeAttack = true;
            }
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
        this.health = health;
    }

    public int GetHealth()
    {
        return this.health;
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
