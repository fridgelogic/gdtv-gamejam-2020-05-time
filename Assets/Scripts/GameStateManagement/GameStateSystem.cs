using FridgeLogic.ScriptableObjects.GameEvents;
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
        private GameEvent gamePaused = null;

        [SerializeField]
        private GameEvent gameUnpaused = null;

        private bool isGamePaused;

        public void StartGame()
        {
            gameStarted.Raise();
        }

        public void EndGame()
        {
            gameOver.Raise();
        }

        public void TogglePause()
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                gamePaused.Raise();
            }
            else
            {
                gameUnpaused.Raise();
            }
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