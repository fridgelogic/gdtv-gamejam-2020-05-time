using FridgeLogic.Audio;
using UnityEngine;

namespace FridgeLogic.Audio
{
    [CreateAssetMenu(fileName = "SoundPlayerProvider", menuName = "FridgeLogic/Providers/SoundPlayerProvider", order = 51)]
    public class SoundPlayerProvider : ScriptableObject
    {
        public ISoundPlayer SoundPlayer { get; set; }
    }
}