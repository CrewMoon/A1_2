using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour
{
    private const int HEALTH = 2;
    private const int DAMAGE = 1;
    private const int Range = 30;
    private const float CoolDownTime = 1f;
    private const float RefreshTime = 2f;
    private int health = HEALTH;
    private GameObject hero;
    private int[] gridPos;

    private void Awake()
    {
        hero = GameObject.Find("Hero");
        GetComponent<AIDestinationSetter>().target = hero.transform;
    }

    private void Update()
    {
        if (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition,
                hero.GetComponent<RectTransform>().anchoredPosition) < 50)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public IEnumerator CoolDownAttackForSeconds(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void RefreshAfterFreshTime()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Invoke("Reset", RefreshTime);
        health = HEALTH;
    }

    private void Reset()
    {
        RefreshPosition();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void RefreshPosition()
    {
        if (this.gridPos == null)
        {
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
        else
        {
            GridMap.Instance.ReleaseGrid(gridPos[0], gridPos[1]);
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public int getDAMAGE()
    {
        return DAMAGE;
    }

    public float getCoolDownTime()
    {
        return CoolDownTime;
    }
}