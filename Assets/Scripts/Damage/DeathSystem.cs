using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Groups;
using UnityEngine;

namespace FridgeLogic.Damage
{
    public class DeathSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObjectGroup _entities = null;

        [SerializeField]
        private GameEvent _allEntitiesDead = null;

        public void EntityDied(GameObject entity)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                if (_entities.Group[i] == entity)
                {
                    _entities.Group[i].SetActive(false);
                }
            }

            if (_entities.Count == 0)
            {
                _allEntitiesDead?.Raise();
            }
        }
    }
}