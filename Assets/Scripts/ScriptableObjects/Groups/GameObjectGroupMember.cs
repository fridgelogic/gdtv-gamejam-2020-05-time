using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Groups
{
    public class GameObjectGroupMember : MonoBehaviour
    {
        [SerializeField]
        private GameObjectGroup gameObjectGroup = null;

        private void Awake()
        {
            Debug.Assert(gameObjectGroup);
        }

        private void OnEnable()
        {
            gameObjectGroup.Add(gameObject);
        }

        private void OnDisable()
        {
            gameObjectGroup.Remove(gameObject);
        }
    }
}