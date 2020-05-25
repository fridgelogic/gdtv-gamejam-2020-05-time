using FridgeLogic.ScriptableObjects.Providers;
using UnityEngine;

namespace FridgeLogic.Audio
{
    public interface IMusicPlayer
    {
        bool IsPlaying { get; }
        AudioClip CurrentAudioClip { get; }

        void PauseMusic();
        void PlayMusic(AudioClip audioClip);
        void StopMusic();
        void UnpauseMusic();
    }

    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour, IMusicPlayer
    {
        [SerializeField] private MusicPlayerProvider _musicPlayerProvider = null;

        private AudioSource _audioSource = null;
        private AudioSource AudioSource => _audioSource ?? (_audioSource = GetComponent<AudioSource>());

        public bool IsPlaying => AudioSource.isPlaying;

        public AudioClip CurrentAudioClip => AudioSource.clip;

        public void PlayMusic(AudioClip audioClip)
        {
            StopMusic();
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }

        public void PauseMusic() => AudioSource.Pause();
        public void UnpauseMusic() => AudioSource.UnPause();
        public void StopMusic() => AudioSource.Stop();

        // TODO: Move this to a separate scene, DDOL is essentially deprecated
        private void Awake()
        {
            if (_musicPlayerProvider.MusicPlayer != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                _musicPlayerProvider.MusicPlayer = this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}