using System;
using FridgeLogic.ScriptableObjects.Values;
using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    [Serializable]
    public class IntValueUnityEvent : UnityEvent<IntValue> { }

    public class IntValueGameEventListener : MonoBehaviour
    {
        [SerializeField]
        private IntValueGameEvent gameEvent = null;

        [SerializeField]
        private IntValueUnityEvent response = null;

        public void OnEventRaised(IntValue context)
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