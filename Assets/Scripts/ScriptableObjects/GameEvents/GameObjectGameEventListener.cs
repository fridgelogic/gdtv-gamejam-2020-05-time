using System;
using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject> { }

    public class GameObjectGameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameObjectGameEvent gameEvent = null;

        [SerializeField]
        private GameObjectUnityEvent response = null;

        public void OnEventRaised(GameObject context)
        {
            response?.Invoke(context);
        }

        private void Awake()
        {
            Debug.Assert(gameEvent != null);
        }

        private void OnEnable()
        {
            gameEvent.Register(this);
        }

        private void OnDisable()
        {
            gameEvent.Unregister(this);
        }
    }
}