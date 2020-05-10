using FridgeLogic.ScriptableObjects.GameEvents;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float maxHealth = 1f;

        [SerializeField]
        private GameObjectGameEvent entityHit = null;

        [SerializeField]
        private GameObjectGameEvent entityDied = null;

        private float currentHealth;

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
            if (currentHealth > 0f)
            {
                entityHit?.Raise(gameObject);
            }
            else
            {
                entityDied?.Raise(gameObject);
                Destroy(gameObject, 0.5f);
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            currentHealth = maxHealth;
        }
    }

    public interface IHealth
    {
        void TakeDamage(float damage);
    }
}