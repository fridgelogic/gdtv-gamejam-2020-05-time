using FridgeLogic.ScriptableObjects.Providers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridgeLogic.Core
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private MusicPlayerProvider _musicPlayerProvider = null;
        [SerializeField] private AudioClip _backgroundMusic = null;
        [SerializeField] private SceneReference _nextLevel = null;

        public void LoadLevel(SceneReference level, float delayInSeconds = 0f)
        {
            StartCoroutine(LoadSceneCoroutine(delayInSeconds, level));
        }

        public void LoadNextLevel()
        {
            if (_nextLevel == null)
            {
                throw new Exception($"{nameof(_nextLevel)}");
            }
            StartCoroutine(LoadSceneCoroutine(0f, _nextLevel));
        }

        public void ReloadCurrentLevel(float delayInSeconds = 0f)
        {
            StartCoroutine(LoadSceneCoroutine(delayInSeconds));
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private IEnumerator LoadSceneCoroutine(int buildIndex)
        {
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            yield return LoadSceneCoroutine(0f, scene.name);
        }

        private IEnumerator LoadSceneCoroutine(float timeToWait = 0f, string sceneName = "")
        {
            if (timeToWait > 0)
            {
                Time.timeScale = 1f;
                yield return new WaitForSeconds(timeToWait);
            }
            else
            {
                yield return null;
            }

            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                yield return SceneManager.LoadSceneAsync(sceneName);
            }
            else
            {
                // Reload the current scene
                yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void Start()
        {
            if (_musicPlayerProvider.MusicPlayer.CurrentAudioClip != _backgroundMusic)
            {
                _musicPlayerProvider.MusicPlayer.PlayMusic(_backgroundMusic);
            }
        }
    }
}