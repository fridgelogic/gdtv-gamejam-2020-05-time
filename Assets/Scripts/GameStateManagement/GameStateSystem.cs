using System.Linq;
using System.Collections;
using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Groups;
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
        private GameEvent restartGame = null;

        [SerializeField]
        private GameObjectGroup players = null;

        private bool isGamePaused;
        private float originalTimeScale;

        public void StartGame()
        {
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
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0;
                gamePaused.Raise();
            }
            else
            {
                Time.timeScale = originalTimeScale;
                gameUnpaused.Raise();
            }
        }

        public void RestartLevel()
        {
            StartCoroutine(ProcessLevelRestart());
        }

        private IEnumerator ProcessLevelRestart()
        {
            for (int i = players.Count - 1; i >= 0; i--)
            {
                players.Group[i].SetActive(false);
            }

            yield return new WaitForEndOfFrame();
            restartGame.Raise();
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