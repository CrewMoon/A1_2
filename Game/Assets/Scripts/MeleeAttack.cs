using UnityEngine;

namespace DefaultNamespace
{
    public class MeleeAttack : MonoBehaviour
    {
        private const int DAMAGE = 2;
        private const float CooldownTime = 2;
        private const int Range = 60;
        private GameObject hero;
        private int[] gridPos;
        private void Awake()
        {
            hero = GameObject.Find("Hero");
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
                    Destroy(this.gameObject, 0.2f);
                    break;
                }
                case "MeleeEnemy":
                {
                    GameObject meleeEnemy = touchedObject;
                    int MeleeEnemyHealth = meleeEnemy.GetComponent<MeleeEnemy>().getHealth();
                    if (MeleeEnemyHealth > DAMAGE)
                    {
                        meleeEnemy.GetComponent<MeleeEnemy>().setHealth(MeleeEnemyHealth - DAMAGE);
                    }
                    else
                    {
                        meleeEnemy.GetComponent<MeleeEnemy>().RefreshAfterFreshTime();
                        hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 10);
                    }
                    Destroy(this.gameObject, 0.2f);
                    break;
                }
                case "BulletEnemy":
                {
                    Destroy(touchedObject);
                    Destroy(this.gameObject, 0.2f);
                    break;
                }
                case "RangedEnemy":
                {
                    touchedObject.GetComponent<RangedEnemy>().RefreshAfterFreshTime();
                    hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 20);
                    Destroy(this.gameObject, 0.2f);
                    break;
                }
            }
        }
        
        public float GetCoolDownTime()
        {
            return CooldownTime;
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

        public int getDamage()
        {
            return DAMAGE;
    }
    }
}