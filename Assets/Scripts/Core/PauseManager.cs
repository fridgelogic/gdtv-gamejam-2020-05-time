using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.Core
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _gamePaused = null;
        [SerializeField] private UnityEvent _gameUnpaused = null;

        private bool _isPaused;
        private float _originalTimeScale;

        [ContextMenu("Toggle Pause")]
        public void TogglePause()
        {
            if (_isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        [ContextMenu("Pause Game")]
        public void PauseGame()
        {
            if (_isPaused) return;
            _isPaused = true;
            Time.timeScale = 0;
            _gamePaused?.Invoke();
        }

        [ContextMenu("Unpause Game")]
        public void UnpauseGame()
        {
            if (!_isPaused) return;
            _isPaused = false;
            Time.timeScale = _originalTimeScale;
            _gameUnpaused?.Invoke();
        }

        private void Start()
        {
            _originalTimeScale = Time.timeScale;
            #if UNITY_EDITOR
            if (FindObjectsOfType<PauseManager>().Length > 1)
            {
                Debug.LogWarning($"Running multiple instances of {typeof(PauseManager).Name} is not advised.");
            }
            #endif
        }

        private void OnDestroy()
        {
            UnpauseGame();
        }
    }
}