using System.Collections.Generic;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    [CreateAssetMenu(fileName = "GameObjectGameEvent", menuName = "FridgeLogic/Events/GameObjectGameEvent", order = 54)]
    public class GameObjectGameEvent : ScriptableObject
    {
        private readonly List<GameObjectGameEventListener> listeners = new List<GameObjectGameEventListener>();

        public void Register(GameObjectGameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unregister(GameObjectGameEventListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise(GameObject context)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(context);
            }
        }
    }
}