using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Groups;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class DeathSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObjectGroup entities = null;

        [SerializeField]
        private GameEvent allEntitiesDead = null;

        public void EntityDied(GameObject entity)
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (entities.Group[i] == entity)
                {
                    entities.Group[i].SetActive(false);
                }
            }

            if (entities.Count == 0)
            {
                allEntitiesDead?.Raise();
            }
        }
    }
}