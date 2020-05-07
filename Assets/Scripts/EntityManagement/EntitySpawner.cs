using FridgeLogic.ScriptableObjects.GameEvents;
using UnityEngine;

namespace FridgeLogic.EntityManagement
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab = null;

        [SerializeField]
        private GameObject parent = null;

        [SerializeField]
        private GameObjectGameEvent entitySpawned = null;

        public void Spawn()
        {
            var entity = Instantiate(prefab, transform.position, transform.rotation);
            if (parent)
            {
                entity.transform.parent = parent.transform;
            }
            
            entitySpawned?.Raise(entity);
        }

        private void Awake()
        {
            Debug.Assert(prefab);

            if (TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.enabled = false;
            }
        }
    }
}