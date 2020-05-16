using FridgeLogic.Audio;
using UnityEngine;

namespace FridgeLogic.Interactable
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private AudioClip _pickupSound = null;
        [SerializeField] private SoundPlayerProvider _soundPlayer = null;

        public void OnPickUp(GameObject go)
        {
            Debug.Log($"{name} picked up by {go}");

            if (_pickupSound && _soundPlayer)
            {
                _soundPlayer.SoundPlayer.PlaySound(_pickupSound);
            }

            Destroy(gameObject, 0.5f);
            gameObject.SetActive(false);
        }
    }
}