using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class MenuPanel : MonoBehaviour
    
    {
        [SerializeField]
        private EventSystem eventSystem = null;

        [SerializeField]
        private Button selectedControl = null;

        private void Awake()
        {
            Debug.Assert(eventSystem);
            Debug.Assert(selectedControl);
        }

        private void OnEnable()
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(selectedControl.gameObject);
        }
    }
}