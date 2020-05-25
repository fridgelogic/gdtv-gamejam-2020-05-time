using FridgeLogic.ScriptableObjects.Providers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.Damage
{
    public class Health : MonoBehaviour
    {
        public event Action EntitiyDied;
        public event Action EntityHit;

        [SerializeField] private UnityEvent _entityDied = null;
        [SerializeField] private UnityEvent _entityHit = null;
        [SerializeField] private SoundPlayerProvider _soundPlayerProvider = null;
        [SerializeField] private AudioClip _playOnDeath = null;
        [SerializeField] private float _maxHealth = 1f;

        private float _currentHealth;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _entityHit?.Invoke();
            EntityHit?.Invoke();
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        [ContextMenu("Die")]
        public void Die()
        {
            _currentHealth = 0;
            if (_soundPlayerProvider && _playOnDeath)
            {
                _soundPlayerProvider.SoundPlayer.PlaySound(_playOnDeath);
            }

            _entityDied?.Invoke();
            EntitiyDied?.Invoke();
            Destroy(gameObject, 0.5f);
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
    }
}