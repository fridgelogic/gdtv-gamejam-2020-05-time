using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridgeLogic.GameStateManagement
{
    public class GameSceneLoader : MonoBehaviour
    {
        public void RestartScene()
        {
            // StartCoroutine(ReloadCurrentScene());
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(scene.name);
        }

        private IEnumerator ReloadCurrentScene()
        {
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
            var scene = SceneManager.GetActiveScene();
            yield return SceneManager.UnloadSceneAsync(scene);
            yield return SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Single);
            Destroy(gameObject);
        }
    }
}