using System;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Health : MonoBehaviour
    {
        public static event Action<GameObject> EntityDied;

        [SerializeField] private float maxHealth = 1f;

        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, maxHealth);
            if (CurrentHealth <= 0f)
            {
                IsDead = true;
                EntityDied?.Invoke(gameObject);
            }
        }

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }
    }
}