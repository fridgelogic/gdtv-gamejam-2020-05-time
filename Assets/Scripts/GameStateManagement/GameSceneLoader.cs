using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridgeLogic.GameStateManagement
{
    public class GameSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneReference mainMenu = null;

        [SerializeField]
        private SceneReference firstLevel = null;

        public void GoToMainMenu()
        {
            StartCoroutine(LoadLevel(mainMenu));
        }

        public void StartGame()
        {
            StartCoroutine(LoadLevel(firstLevel));
        }

        public void RestartLevel()
        {
            StartCoroutine(LoadLevel(firstLevel));
        }

        private IEnumerator LoadLevel(SceneReference scene, bool unloadActiveScene = true)
        {
            if (unloadActiveScene)
            {
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scene.ScenePath));
        }

        private IEnumerator LoadLeve(Scene scene, bool unloadActiveScene = true)
        {
            if (unloadActiveScene)
            {
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            yield return SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(scene);
        }

        private void Start()
        {
            StartCoroutine(LoadLevel(mainMenu, false));
        }
    }
}