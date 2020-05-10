using System.Collections.Generic;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Groups
{
    [CreateAssetMenu(fileName = "GameObjectGroup", menuName = "FridgeLogic/Groups/GameObjectGroup", order = 52)]
    public class GameObjectGroup : ScriptableObject
    {
        private List<GameObject> gameObjects = new List<GameObject>();

        public IReadOnlyList<GameObject> Group => gameObjects;
        public int Count => gameObjects.Count;

        public void Add(GameObject gameObject)
        {
            if(!gameObjects.Contains(gameObject))
            {
                gameObjects.Add(gameObject);
            }
        }

        public void Remove(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
    }
}