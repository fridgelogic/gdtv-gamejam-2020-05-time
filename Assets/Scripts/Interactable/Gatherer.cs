using System;
using UnityEngine;

namespace FridgeLogic.Interactable
{
    public class Gatherer : MonoBehaviour
    {
        public event Action<Pickup> Gathered;

        public void Gather(Pickup pickup)
        {
            Gathered?.Invoke(pickup);
            pickup.gameObject.SetActive(false);
            Destroy(pickup.gameObject, 0.5f);
        }
    }
}