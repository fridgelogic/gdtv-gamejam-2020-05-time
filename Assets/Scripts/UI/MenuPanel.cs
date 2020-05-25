using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class MenuPanel : MonoBehaviour
    
    {
        [SerializeField] private EventSystem _eventSystem = null;
        [SerializeField] private Button _selectedControl = null;

        private void Awake()
        {
            Debug.Assert(_eventSystem);
            Debug.Assert(_selectedControl);
        }

        private void OnEnable()
        {
            _eventSystem.SetSelectedGameObject(null);
            _eventSystem.SetSelectedGameObject(_selectedControl.gameObject);
        }
    }
}