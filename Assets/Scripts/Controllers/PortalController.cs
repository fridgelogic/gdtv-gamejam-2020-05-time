using FridgeLogic.Core;
using FridgeLogic.Pickups;
using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(Animator))]
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager = null;
        [SerializeField] private SceneReference _nextLevel = null;
        [SerializeField] private int _coinsRequiredtoOpen = 50;
        [SerializeField] private AudioClip _portalOpenSound = null;
        [SerializeField] private SoundPlayerProvider _soundPlayerProvider = null;

        private Animator _animator = null;
        private Animator Animator => _animator ?? (_animator = GetComponent<Animator>());

        private bool _isActivated;

        [ContextMenu("Activate Portal")]
        public void ActivatePortal()
        {
            if (_isActivated) return;

            _isActivated = true;
            _coinsRequiredtoOpen = 0;
            Animator.SetTrigger("Activate");
            if (_soundPlayerProvider && _portalOpenSound)
            {
                _soundPlayerProvider.SoundPlayer.PlaySound(_portalOpenSound);
            }
        }

        public void OnCoinPickedUp(int coinValue)
        {
            _coinsRequiredtoOpen -= coinValue;
            if (_coinsRequiredtoOpen <= 0)
            {
                Coin.CoinPickedUp -= OnCoinPickedUp;
                ActivatePortal();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActivated)
            {
                _levelManager.LoadLevel(_nextLevel);
            }
        }

        private void OnEnable()
        {
            Coin.CoinPickedUp += OnCoinPickedUp;
        }

        private void OnDisable()
        {
            Coin.CoinPickedUp -= OnCoinPickedUp;
        }
    }
}