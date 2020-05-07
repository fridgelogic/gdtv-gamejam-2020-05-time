using FridgeLogic.ScriptableObjects.GameEvents;
using UnityEngine;

namespace FridgeLogic.Interactable
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onPickup = null;

        private void OnTriggerEnter2D(Collider2D other)
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject, 1f);
            onPickup.Raise();
        }
    }
}