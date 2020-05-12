using Cinemachine;
using UnityEngine;

namespace FridgeLogic.EntityManagement
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private GameObject parent = null;
        [SerializeField] private CinemachineVirtualCamera followCamera = null;

        public GameObject Spawn()
        {
            var entity = Instantiate(prefab, transform.position, transform.rotation);
            if (parent)
            {
                entity.transform.parent = parent.transform;
            }
            if (followCamera)
            {
                followCamera.Follow = entity.transform;
            }
            return entity;
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