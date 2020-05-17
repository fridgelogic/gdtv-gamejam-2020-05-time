using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;

namespace FridgeLogic.GameStateManagement
{
    public class GameStateSystem : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameStarted = null;

        [SerializeField]
        private GameEvent gameOver = null;

        [SerializeField]
        private GameEvent quitGame = null;

        [SerializeField]
        private GameEvent gamePaused = null;

        [SerializeField]
        private GameEvent gameUnpaused = null;

        [SerializeField]
        private GameEvent restartLevel = null;

        [SerializeField]
        private GameEvent _loadNextLevel = null;

        [SerializeField]
        private MusicPlayerProvider _musicPlayerProvider = null;

        [SerializeField]
        private AudioClip _backgroundMusic = null;

        private bool _isGamePaused;
        private float _originalTimeScale;

        public void StartGame()
        {
            if (_musicPlayerProvider && _backgroundMusic)
            {
                if (!_musicPlayerProvider.MusicPlayer.IsPlaying)
                {
                    _musicPlayerProvider.MusicPlayer.PlayMusic(_backgroundMusic);
                }
            }

            Time.timeScale = 1f;
            gameStarted.Raise();
        }

        public void QuitGame()
        {
            quitGame.Raise();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void GameOver()
        {
            gameOver.Raise();
        }

        public void TogglePause()
        {
            _isGamePaused = !_isGamePaused;

            if (_isGamePaused)
            {
                _originalTimeScale = Time.timeScale;
                Time.timeScale = 0;
                gamePaused.Raise();
            }
            else
            {
                Time.timeScale = _originalTimeScale;
                gameUnpaused.Raise();
            }
        }

        public void RestartLevel()
        {
            restartLevel.Raise();
        }

        public void PlayerEnteredPortal()
        {
            _musicPlayerProvider.MusicPlayer.StopMusic();
            _loadNextLevel.Raise();
        }

        private void Awake()
        {
            Debug.Assert(gameStarted);
            Debug.Assert(gameOver);
            Debug.Assert(gamePaused);
            Debug.Assert(gameUnpaused);
        }

        private void Start()
        {
            StartGame();
        }
    }
}