using UnityEngine;

namespace FridgeLogic.Interactable
{
    public class Gatherer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Pickup>(out var pickup))
            {
                pickup.OnPickUp(gameObject);
            }
        }
    }
}