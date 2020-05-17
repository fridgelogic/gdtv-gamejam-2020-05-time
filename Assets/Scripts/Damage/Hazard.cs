using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField]
        private float _damage = 1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(_damage);
            }
        }
    }
}
