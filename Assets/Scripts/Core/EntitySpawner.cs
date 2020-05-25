using UnityEngine;

namespace FridgeLogic.Core
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab = null;
        [SerializeField] private GameObject _parent = null;

        [ContextMenu("Spawn Entity")]
        public GameObject Spawn()
        {
            var transform = this.transform;
            var entity = Instantiate(_prefab, transform.position, transform.rotation);
            if (_parent)
            {
                entity.transform.parent = _parent.transform;
            }

            return entity;
        }

        private void Awake()
        {
            Debug.Assert(_prefab);

            if (TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.enabled = false;
            }
        }
    }
}