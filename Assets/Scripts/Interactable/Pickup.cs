using DG.Tweening;
using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;

namespace FridgeLogic.Interactable
{
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onPickup = null;

        [SerializeField]
        private SoundPlayerProvider _soundPlayerProvider = null;

        [SerializeField]
        private AudioClip _playOnPickup = null;

        [SerializeField]
        private Transform _moveToOnPickup = null;

        private Collider2D _collider = null;
        private Collider2D Collider => _collider ?? (_collider = GetComponent<Collider2D>());

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_soundPlayerProvider && _playOnPickup)
            {
                _soundPlayerProvider.SoundPlayer.PlaySound(_playOnPickup);
            }

            Collider.enabled = false;
            onPickup.Raise();

            if (_moveToOnPickup)
            {
                transform.DOMove(_moveToOnPickup.position, 0.5f, false)
                    .OnComplete(Die);
            }
            else
            {
                Die();
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject, 1f);
        }
    }
}