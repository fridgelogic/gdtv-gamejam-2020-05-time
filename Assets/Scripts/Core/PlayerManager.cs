using System;
using Cinemachine;
using FridgeLogic.Control;
using FridgeLogic.Damage;
using UnityEngine;

namespace FridgeLogic.Core
{
    public class PlayerManager : EntitySpawner
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
        [SerializeField] private LevelManager _levelManager = null;
        [SerializeField] private PauseManager _pauseManager = null;

        private GameObject _player = null;
        private Health _playerHealth = null;
        private PlatformerInputAgent _playerControls = null;

        public void OnPlayerDied()
        {
            Unsubscribe();
            _levelManager.ReloadCurrentLevel(0.2f);
        }

        public void OnPlayerPause()
        {
            _pauseManager.TogglePause();
        }

        private void Start()
        {
            _player = Spawn();
            _virtualCamera.Follow = _player.transform;

            if (!_player.TryGetComponent<Health>(out _playerHealth))
            {
                throw new Exception($"{_player} player does not have a {typeof(Health).Name} component");
            }

            if (!_player.TryGetComponent<PlatformerInputAgent>(out _playerControls))
            {
                throw new Exception($"{_player} player does not have a {typeof(PlatformerInputAgent).Name} component");
            }

            Subscribe();
        }

        private void Subscribe()
        {
            if (_playerHealth)
            {
                _playerHealth.EntitiyDied += OnPlayerDied;
            }

            if (_playerControls)
            {
                _playerControls.PlayerToggledPause += OnPlayerPause;
            }
        }

        private void Unsubscribe()
        {
            if (_playerHealth)
            {
                _playerHealth.EntitiyDied -= OnPlayerDied;
            }

            if (_playerControls)
            {
                _playerControls.PlayerToggledPause -= OnPlayerPause;
            }
        }

        private void OnEnable() => Subscribe();
        private void OnDisable() => Unsubscribe();
    }
}