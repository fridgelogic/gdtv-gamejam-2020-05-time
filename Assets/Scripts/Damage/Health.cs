using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float _maxHealth = 1f;

        [SerializeField]
        private GameObjectGameEvent _entityHit = null;

        [SerializeField]
        private GameObjectGameEvent _entityDied = null;

        [SerializeField]
        private SoundPlayerProvider _soundPlayerProvider = null;

        [SerializeField]
        private AudioClip _playOnDeath = null;

        private float _currentHealth;

        public void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0f, _maxHealth);
            if (_currentHealth > 0f)
            {
                _entityHit?.Raise(gameObject);
            }
            else
            {
                if (_soundPlayerProvider && _playOnDeath)
                {
                    _soundPlayerProvider.SoundPlayer.PlaySound(_playOnDeath);
                }

                _entityDied?.Raise(gameObject);
                Destroy(gameObject, 0.5f);
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
    }

    public interface IHealth
    {
        void TakeDamage(float damage);
    }
}