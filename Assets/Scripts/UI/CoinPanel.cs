using FridgeLogic.ScriptableObjects.Groups;
using UnityEngine;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField]
        private Text coinCount = null;

        [SerializeField]
        private GameObjectGroup coinGroup = null;

        public void CollectCoin()
        {
            UpdateCoinCount();
        }

        private void UpdateCoinCount()
        {
            coinCount.text = coinGroup.Count.ToString();
        }

        // Start is called before the first frame update
        private void Start()
        {
            UpdateCoinCount();
        }
    }
}