using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Microsoft.Unity.VisualStudio.Editor;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Hero : MonoBehaviour
{
    private SceneManager sceneManager;
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

    public GameObject StopPanel;
    public GameObject MeleeAttackPrefab;
    public GameObject RangedAttackPrefab;
    public GameObject RangedSkill;
    public GameObject MeleeSkill;
    public GameObject TimeIndicator;
    public GameObject ScoreIndicator;
    public GameObject LifeIndicator;



    private void Awake()
    {
        Refresh();
        sceneManager = GameObject.Find("Canvas").GetComponent<SceneManager>();
    }

    private void Update()
    {
        FollowingMouse();
        KeyCodeResponse();
        SurviveTimeUpdate();
        RangedAttackCoolDown();
        MeleeAttackCoolDown();
        TimeIndicator.GetComponent<TMP_Text>().text = "Time: " + Math.Round(RequiredSurviveTime - AlreadySurviveTime, 1);
        ScoreIndicator.GetComponent<TMP_Text>().text = "Score: " + Score;
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
        Vector2 position = new Vector2();
        bool walkable = false;
    
        
        while (!walkable)
        {
            position.x = Random.Range(-rangeRadius, rangeRadius);
            position.y = Random.Range(-rangeRadius, rangeRadius);
            this.GetComponent<RectTransform>().anchoredPosition = position;
            position = this.transform.position;

            Vector2 leftUp = new Vector2(position.x - width / 2, position.y + width / 2);
            Vector2 rightUp = new Vector2(position.x + width / 2, position.y + width / 2);
            Vector2 leftDown = new Vector2(position.x - width / 2, position.y - width / 2);
            Vector2 rightDown = new Vector2(position.x + width / 2, position.y - width / 2);
            if (AstarPath.active.GetNearest(leftUp).node.Walkable &&
                AstarPath.active.GetNearest(rightUp).node.Walkable &&
                AstarPath.active.GetNearest(leftDown).node.Walkable &&
                AstarPath.active.GetNearest(rightDown).node.Walkable)
            {
                walkable = true;
            }
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
                StopPanel.SetActive(true);
                Time.timeScale = 1f;
                Debug.Log("Start");
            }
            else
            {
                StopPanel.SetActive(true);
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
                Success();
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
            GameObject rangedAttack = Instantiate(RangedAttackPrefab, this.transform.position, this.transform.GetChild(2).rotation, this.transform);
            rangedAttack.GetComponent<BulletAttack>().Fire();
            canFireRangedAttack = false;
            Invoke("ResetRangedAttack", rangedAttack.GetComponent<BulletAttack>().GetCoolDownTime());
        }
    }

    private void MeleeAttack()
    {
        if (canFireMeleeAttack)
        {
            GameObject meleeAttack = Instantiate(MeleeAttackPrefab, this.transform.position, this.transform.rotation, this.transform.GetChild(0));
            Destroy(meleeAttack, MeleeAttackDestroryTime);
            canFireMeleeAttack = false;
            Invoke("ResetMeleeAttack", meleeAttack.GetComponent<MeleeAttack>().GetCoolDownTime());
        }
    }

    private void RangedAttackCoolDown()
    {
        GameObject TimeText = RangedSkill.transform.GetChild(0).GameObject();
        GameObject CoolDownMask = RangedSkill.transform.GetChild(1).GetChild(0).GameObject();
        if (!canFireRangedAttack)
        {
            if (RangedAttackCoolDownTime > 0)
            {
                TimeText.SetActive(true);
                CoolDownMask.SetActive(true);
                RangedAttackCoolDownTime -= Time.deltaTime;
                double d = Math.Round(RangedAttackCoolDownTime, 1);
                TimeText.GetComponent<TMP_Text>().text = d.ToString();
                CoolDownMask.GetComponent<Image>().fillAmount = RangedAttackCoolDownTime /
                                                                RangedAttackPrefab.GetComponent<BulletAttack>()
                                                                    .GetCoolDownTime();
                return;
            }
        }
        TimeText.SetActive(false);
        CoolDownMask.SetActive(false);
        RangedAttackCoolDownTime = RangedAttackPrefab.GetComponent<BulletAttack>().GetCoolDownTime();
        canFireRangedAttack = true;
    }
    
    private void MeleeAttackCoolDown()
    {
        GameObject TimeText = MeleeSkill.transform.GetChild(0).GameObject();
        GameObject CoolDownMask = MeleeSkill.transform.GetChild(1).GetChild(0).GameObject();
        if (!canFireMeleeAttack)
        {
            if (MeleeAttackCoolDownTime > 0)
            {
                TimeText.SetActive(true);
                CoolDownMask.SetActive(true);
                MeleeAttackCoolDownTime -= Time.deltaTime;
                double d = Math.Round(MeleeAttackCoolDownTime, 1);
                TimeText.GetComponent<TMP_Text>().text = d.ToString();
                CoolDownMask.GetComponent<Image>().fillAmount = MeleeAttackCoolDownTime /
                                                                MeleeAttackPrefab.GetComponent<MeleeAttack>()
                                                                    .GetCoolDownTime();
                return;
            }
        }
        TimeText.SetActive(false);
        CoolDownMask.SetActive(false);
        MeleeAttackCoolDownTime = MeleeAttackPrefab.GetComponent<MeleeAttack>().GetCoolDownTime();
        canFireMeleeAttack = true;
    }

    private void FollowingMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 Direction = (mousePos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.GetChild(2).rotation = Quaternion.Slerp(transform.GetChild(2).rotation, targetRotation, Time.deltaTime * 8);
    }

    public void Fail()
    {
        sceneManager.Fail(Score);
        Refresh();
    }

    private void Success()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level1")
        {
            SceneLoader.Instance.LoadScene("Level2");
        }else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level2")
        {
            sceneManager.Success(Score);
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
        for (int i = health - 1; i < this.health; i++)
        {
            LifeIndicator.transform.GetChild(1).GetChild(i).GameObject().SetActive(false);
        }
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
