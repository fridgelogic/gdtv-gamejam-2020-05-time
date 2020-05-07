using UnityEngine;

namespace FridgeLogic.Audio
{
    public class AudioClipLoader : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource = null;

        [SerializeField]
        private AudioClip audioClip = null;

        private void Start()
        {
            Debug.Assert(audioSource);
            Debug.Assert(audioClip);
        }

        public void PlayClip()
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void PlayOneShot()
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}