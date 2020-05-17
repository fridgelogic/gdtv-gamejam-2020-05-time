using System.Collections.Generic;
using FridgeLogic.ScriptableObjects.Values;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.GameEvents
{
    [CreateAssetMenu(fileName = "IntValueGameEvent", menuName = "FridgeLogic/Events/IntValueGameEvent", order = 51)]
    public class IntValueGameEvent : ScriptableObject
    {
        private readonly List<IntValueGameEventListener> listeners = new List<IntValueGameEventListener>();

        public void Register(IntValueGameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unregister(IntValueGameEventListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise(IntValue value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }
    }
}