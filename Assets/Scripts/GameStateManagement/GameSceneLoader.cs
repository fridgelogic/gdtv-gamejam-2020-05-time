using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridgeLogic.GameStateManagement
{
    public class GameSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneReference scene = null;

        public void LoadScene(float timeToWait)
        {
            StartCoroutine(HandleSceneLoad(timeToWait));
        }

        private IEnumerator HandleSceneLoad(float timeToWait)
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

            if (!string.IsNullOrWhiteSpace(scene.ScenePath))
            {
                yield return SceneManager.LoadSceneAsync(scene);
            }
            else
            {
                // Reload the current scene
                yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}