using FridgeLogic.ScriptableObjects.GameEvents;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float maxHealth = 1f;

        [SerializeField]
        private GameObjectGameEvent entityDied = null;

        private float currentHealth;

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
            if (currentHealth <= 0f)
            {
                entityDied.Raise(this.gameObject);
                Destroy(gameObject, 0.5f);
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            currentHealth = maxHealth;
            Debug.Assert(entityDied);
        }
    }

    public interface IHealth
    {
        void TakeDamage(float damage);
    }
}