using UnityEngine;

namespace DefaultNamespace
{
    public class MeleeAttack : MonoBehaviour
    {
        private const int Damage = 2;
        private const float CooldownTime = 2;
        private const int Range = 3;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject touchedObject = collision.gameObject;
            string touchedTage = touchedObject.tag;

            switch (touchedTage)
            {
                case "Player":
                    return;
                case "WreckableObstacle":
                {
                    bool IsOver2Heart = false;
                    if (IsOver2Heart)
                    {
                        Debug.Log("heart minus 2");
                    }
                    else
                    {
                        Destroy(touchedObject);
                    }

                    break;
                }
                case "MeleeEnemy":
                    Destroy(touchedObject);
                    break;
                case "RangedEnemy":
                    Destroy(touchedObject);
                    break;
            }
        }
        
        public float GetCoolDownTime()
        {
            return CooldownTime;
        }
    }
}