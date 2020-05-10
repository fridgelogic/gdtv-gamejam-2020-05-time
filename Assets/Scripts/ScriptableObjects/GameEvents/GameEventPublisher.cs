using UnityEngine;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    public class GameEventPublisher : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent = null;

        public void RaiseEvent()
        {
            gameEvent.Raise();
        }

        private void Awake()
        {
            Debug.Assert(gameEvent);
        }
    }
}