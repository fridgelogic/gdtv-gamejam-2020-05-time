using UnityEngine;

namespace FridgeLogic.Audio
{
    public interface ISoundPlayer
    {
        bool IsPlaying { get; }

        void PlaySound(AudioClip audioClip);
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, ISoundPlayer
    {
        [SerializeField] private SoundPlayerProvider _soundPlayerProvider = null;

        private AudioSource _audioSource = null;
        private AudioSource AudioSource => _audioSource ?? (_audioSource = GetComponent<AudioSource>());

        public bool IsPlaying => AudioSource.isPlaying;
        public void PlaySound(AudioClip audioClip) => AudioSource.PlayOneShot(audioClip);

        // TODO: Move this to a separate scene, DDOL is essentially deprecated
        private void Awake()
        {
            if (_soundPlayerProvider.SoundPlayer != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                _soundPlayerProvider.SoundPlayer = this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}