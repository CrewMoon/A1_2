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
                    Destroy(this.gameObject, 0.2f);
                    break;
                }
                case "MeleeEnemy":
                {
                    GameObject meleeEnemy = touchedObject.transform.parent.gameObject;
                    int MeleeEnemyHealth = meleeEnemy.GetComponent<MeleeEnemy>().getHealth();
                    if (MeleeEnemyHealth > DAMAGE)
                    {
                        meleeEnemy.GetComponent<MeleeEnemy>().setHealth(MeleeEnemyHealth - DAMAGE);
                    }
                    else
                    {
                        meleeEnemy.SetActive(false);
                        meleeEnemy.GetComponent<MeleeEnemy>().RefreshAfterFreshTime();
                        meleeEnemy.SetActive(true);
                    }
                    hero.GetComponent<Hero>().SetScore(hero.GetComponent<Hero>().GetScore() + 10);
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
                    touchedObject.SetActive(false);
                    touchedObject.GetComponent<RangedEnemy>().RefreshAfterFreshTime();
                    touchedObject.SetActive(true);
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
            GameObject parent = GameObject.Find("Map");
            float offset = 20f;
            float radius = this.GetComponent<CircleCollider2D>().radius;
            float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - radius - offset;
            Vector2 position = new Vector2();
            bool walkable = false;
            
            while (!walkable)
            {
                position.x = Random.Range(-rangeRadius, rangeRadius);
                position.y = Random.Range(-rangeRadius, rangeRadius);
        
                if (AstarPath.active.GetNearest(position).node.Walkable)
                {
                    walkable = true;
                    this.GetComponent<RectTransform>().anchoredPosition = position;
                }
            }
        }

    }
}