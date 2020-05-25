using FridgeLogic.Pickups;
using UnityEngine;

namespace FridgeLogic.Core
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager = null;
        [SerializeField] private float _timeLimit = 30f;

        public float RemainingTime { get; private set; }

        public void OnCoinPickedUp(int coinValue)
        {
            RemainingTime += coinValue;
        }

        private void Update()
        {
            RemainingTime -= Time.deltaTime;
            if (RemainingTime <= 0f)
            {
                _levelManager.ReloadCurrentLevel(0.5f);
            }
        }

        private void Start()
        {
            RemainingTime = _timeLimit;
            #if UNITY_EDITOR
            if (FindObjectsOfType<TimeManager>().Length > 1)
            {
                Debug.LogWarning($"Running multiple instances of {typeof(TimeManager).Name} is not advised.");
            }
            #endif
        }
        
        private void OnEnable() => Coin.CoinPickedUp += OnCoinPickedUp;
        private void OnDisable() => Coin.CoinPickedUp -= OnCoinPickedUp;
    }
}