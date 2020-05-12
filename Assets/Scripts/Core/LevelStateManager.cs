using System.Collections;
using FridgeLogic.Audio;
using FridgeLogic.Damage;
using FridgeLogic.EntityManagement;
using FridgeLogic.Tags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridgeLogic.GameStateManagement
{
    public class LevelStateManager : MonoBehaviour
    {
        [SerializeField] private AudioClip backgroundMusic = null;
        [SerializeField] private PrefabSpawner playerSpawner = null;
        [SerializeField] private MusicPlayerProvider musicPlayerProvider = null;

        private bool isGamePaused;
        private float originalTimeScale;

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void GameOver()
        {
            
        }

        public void TogglePause()
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = originalTimeScale;
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator HandleSceneLoad(float timeToWait)
        {
            yield return null;
            // if (timeToWait > 0)
            // {
            //     Time.timeScale = 1f;
            //     yield return new WaitForSeconds(timeToWait);
            // }
            // else
            // {
            //     yield return null;
            // }

            // if (!string.IsNullOrWhiteSpace(scene.ScenePath))
            // {
            //     yield return SceneManager.LoadSceneAsync(scene);
            // }
            // else
            // {
            //     // Reload the current scene
            //     yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            // }
        }

        private void CheckForPlayerDeath(GameObject gameObject)
        {
            if (gameObject.GetComponent<PlayerTag>())
            {
                RestartLevel();
            }
        }

        private void Awake()
        {
            Debug.Assert(backgroundMusic);
            Debug.Assert(playerSpawner);
        }

        private void Start()
        {
            if (!musicPlayerProvider.MusicPlayer.IsPlaying)
            {
                musicPlayerProvider.MusicPlayer.PlayMusic(backgroundMusic);
            }

            playerSpawner.Spawn();
        }

        private void OnEnable()
        {
            Health.EntityDied += CheckForPlayerDeath;
        }

        private void OnDisable()
        {
            Health.EntityDied -= CheckForPlayerDeath;
        }
    }
}

// using System.Collections;
// using UnityEngine;

// namespace FridgeLogic.TimeLimit
// {
//     public class TimeLimitSystem : MonoBehaviour
//     {
//         [SerializeField]
//         private float timeFactor = 1f;

//         private Coroutine runTimer;
//         private bool firedTimeLimitLow;
//         private float nextTimerTick;
//         private float timerProgress;

//         public void StartTimer()
//         {
//             nextTimerTick = Time.time + timeFactor - timerProgress;
//             runTimer = StartCoroutine(RunTimer());
//         }

//         public void PauseTimer()
//         {
//             if (runTimer != null)
//             {
//                 StopCoroutine(runTimer);
//                 runTimer = null;
//             }
//             timerProgress = timeFactor - (nextTimerTick - Time.time);
//         }

//         public void ResetTimer()
//         {
//             PauseTimer();
//             timerProgress = 0f;
//             timeLimit.Value = timeLimit.OriginalValue;
//             StartTimer();
//         }

//         private IEnumerator RunTimer()
//         {
//             while (timeLimit.Value > 0)
//             {
//                 yield return new WaitForSeconds(nextTimerTick - Time.time);
//                 timeLimit.Value--;
//                 timeLimitUpdated.Raise();

//                 if (!firedTimeLimitLow && timeLimit.Value <= lowTimeLimit.Value)
//                 {
//                     firedTimeLimitLow = true;
//                     lowTimeRemaining.Raise();
//                 }
//                 nextTimerTick = Time.time + timeFactor;
//             }

//             yield return new WaitForSeconds(1);
//             timeLimitExpired.Raise();
//         }

//         private void Start()
//         {
//             timerProgress = 0f;
//             timeLimit.Value = timeLimit.OriginalValue;
//         }
//     }
// }