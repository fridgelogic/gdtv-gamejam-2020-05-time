using Cinemachine;
using UnityEngine;

namespace FridgeLogic.EntityManagement
{
    public class SetCameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

        public void SetFollowTarget(GameObject gameObject)
        {
            virtualCamera.Follow = gameObject.transform;
        }

        private void Awake()
        {
            Debug.Assert(virtualCamera);
        }
    }
}