using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Groups
{
    public class GameObjectGroupMember : MonoBehaviour
    {
        [SerializeField] private GameObjectGroup _gameObjectGroup = null;

        private void Awake()
        {
            Debug.Assert(_gameObjectGroup);
        }

        private void OnEnable()
        {
            _gameObjectGroup.Add(gameObject);
        }

        private void OnDisable()
        {
            _gameObjectGroup.Remove(gameObject);
        }
    }
}