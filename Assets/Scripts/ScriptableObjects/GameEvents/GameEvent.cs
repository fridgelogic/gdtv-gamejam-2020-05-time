using System.Collections.Generic;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "FridgeLogic/Events/GameEvent", order = 51)]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> listeners = new List<GameEventListener>();

        public void Register(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unregister(GameEventListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }
    }
}