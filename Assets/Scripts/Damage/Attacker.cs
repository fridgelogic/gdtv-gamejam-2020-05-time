using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;

        public void Attack(Health health)
        {
            health.TakeDamage(damage);
        }
    }
}