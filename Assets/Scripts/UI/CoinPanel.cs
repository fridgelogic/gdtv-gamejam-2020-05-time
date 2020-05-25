using FridgeLogic.Pickups;
using UnityEngine;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField] private Text _coinCount = null;

        private int _coins;

        public void OnCoinPickup(int coinValue)
        {
            _coins += coinValue;
            UpdateCoinCount();
        }

        private void UpdateCoinCount()
        {
            _coinCount.text = _coins.ToString("D2");
        }

        private void Start()
        {
            UpdateCoinCount();
        }

        private void OnEnable()
        {
            Coin.CoinPickedUp += OnCoinPickup;
        }

        private void OnDisable()
        {
            Coin.CoinPickedUp -= OnCoinPickup;
        }
    }
}