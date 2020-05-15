using UnityEngine;

namespace FridgeLogic.Tags
{
    public class ComponentTag : MonoBehaviour
    {
        [SerializeField] private TagCollection _tagCollection = null;

        private void OnEnable()
        {
            _tagCollection?.Add(this);
        }

        private void OnDisable()
        {
            _tagCollection?.Remove(this);
        }
    }
}