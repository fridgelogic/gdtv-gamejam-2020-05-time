using FridgeLogic.Audio;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Providers
{
    [CreateAssetMenu(fileName = "SoundPlayerProvider", menuName = "FridgeLogic/Providers/SoundPlayerProvider", order = 51)]
    public class SoundPlayerProvider : ScriptableObject
    {
        public ISoundPlayer SoundPlayer { get; set; }
    }
}