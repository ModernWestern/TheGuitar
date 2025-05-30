using System.Linq;
using UnityEngine;

namespace Sago
{
    using Guitar;
    
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip[] audioClips;

        public void PlayGuitarString(GuitarStringNote note)
        {
            var clip = audioClips.FirstOrDefault(c => c.name == note.ToString());

            audioSource.PlayOneShot(clip);
        }
    }
}
