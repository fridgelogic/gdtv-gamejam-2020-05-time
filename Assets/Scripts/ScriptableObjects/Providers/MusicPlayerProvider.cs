using FridgeLogic.Audio;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Providers
{

    [CreateAssetMenu(fileName = "MusicPlayerProvider", menuName = "FridgeLogic/Providers/MusicPlayerProvider", order = 51)]
    public class MusicPlayerProvider : ScriptableObject
    {
        public IMusicPlayer MusicPlayer { get; set; }
    }
}