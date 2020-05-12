using System;
using UnityEngine;

namespace FridgeLogic.Interactable
{
    public class Pickup : MonoBehaviour
    {
        public static event Action<GameObject> PickedUp;

        private void OnTriggerEnter2D(Collider2D other)
        {
            PickedUp?.Invoke(gameObject);
        }
    }
}