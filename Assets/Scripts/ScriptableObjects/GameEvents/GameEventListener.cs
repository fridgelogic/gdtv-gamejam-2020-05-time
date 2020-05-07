using UnityEngine;
using UnityEngine.Events;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent = null;

        [SerializeField]
        private UnityEvent response = null;

        public void OnEventRaised()
        {
            response?.Invoke();
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