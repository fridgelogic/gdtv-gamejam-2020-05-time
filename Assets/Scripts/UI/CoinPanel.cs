using FridgeLogic.ScriptableObjects.Groups;
using UnityEngine;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField]
        private Text coinCount = null;

        private int _coins = -1;

        public void CollectCoin()
        {
            UpdateCoinCount();
        }

        private void UpdateCoinCount()
        {
            coinCount.text = (++_coins).ToString();
        }

        // Start is called before the first frame update
        private void Start()
        {
            //_coins = coinGroup.Count;
            UpdateCoinCount();
        }
    }
}