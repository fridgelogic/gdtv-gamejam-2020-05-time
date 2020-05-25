using System;
using UnityEngine;

namespace FridgeLogic.Pickups
{
    public class Coin : Pickup
    {
        public static event Action<int> CoinPickedUp;

        [SerializeField] private int _coinValue = 1;
        
        public override void PickUp()
        {
            CoinPickedUp?.Invoke(_coinValue);
            base.PickUp();
        }
    }
}