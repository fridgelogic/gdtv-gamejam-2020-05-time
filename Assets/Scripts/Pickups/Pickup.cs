using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.Pickups
{
    [RequireComponent(typeof(Collider2D))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private UnityEvent _pickedUp = null;
        [SerializeField] private SoundPlayerProvider _soundPlayerProvider = null;
        [SerializeField] private AudioClip _playOnPickup = null;

        [ContextMenu("Pick Up")]
        public virtual void PickUp()
        {
            if (_soundPlayerProvider && _playOnPickup)
            {
                _soundPlayerProvider.SoundPlayer.PlaySound(_playOnPickup);
            }

            _pickedUp?.Invoke();
            Destroy(this.gameObject, 1f);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PickUp();
        }
    }
}