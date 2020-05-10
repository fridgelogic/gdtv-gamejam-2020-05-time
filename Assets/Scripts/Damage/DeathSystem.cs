using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Groups;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class DeathSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObjectGroup players = null;

        [SerializeField]
        private GameEvent allPlayersDead = null;

        public void EntityDied(GameObject entity)
        {
            Debug.Log($"Entity {entity.name} died");
            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (players.Group[i] == entity)
                {
                    players.Group[i].SetActive(false);
                }
            }

            Debug.Log($"{players.Count} players remaining");
            if (players.Count == 0)
            {
                allPlayersDead.Raise();
            }
        }
    }
}