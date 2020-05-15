using System.Collections.Generic;
using UnityEngine;

namespace FridgeLogic.Tags
{
    [CreateAssetMenu(fileName = "TagCollection", menuName = "FridgeLogic/Collections/Tags", order = 53)]
    public class TagCollection : ScriptableObject
    {
        private List<ComponentTag> _tags = new List<ComponentTag>();

        public void Add(ComponentTag tag) =>  _tags.Add(tag);

        public void Remove(ComponentTag tag) => _tags.Remove(tag);

        public int Count => _tags.Count;

        public IEnumerable<ComponentTag> Tags
        {
            get
            {
                for (int i = _tags.Count - 1; i >= 0; i--)
                {
                    yield return _tags[i];
                }
            }
        }
    }
}