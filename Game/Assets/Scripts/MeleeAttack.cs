using UnityEngine;

namespace DefaultNamespace
{
    public class MeleeAttack : MonoBehaviour
    {
        private const int DAMAGE = 2;
        private const float CooldownTime = 2;
        private const int Range = 60;
        private GameObject hero;
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
        
        public float GetCoolDownTime()
        {
            return CooldownTime;
        }
        
        public void RefreshPosition()
        {
            GameObject parent = GameObject.Find("Map");
            
            float radius = this.GetComponent<CircleCollider2D>().radius;
            float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - radius;
        
            Vector3 position = new Vector3()
            {
                x = Random.Range(-rangeRadius, rangeRadius),
                y = Random.Range(-rangeRadius, rangeRadius),
                z = 0
            };
        
            if (AstarPath.active.GetNearest(position).node.Walkable)
            {
                this.transform.position = position;
            }
            else
            {
                RefreshPosition();
            }
        }
    }
}