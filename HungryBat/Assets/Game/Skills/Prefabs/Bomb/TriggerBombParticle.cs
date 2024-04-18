using UnityEngine;
using Controllers;

namespace Skills
{
    public class TriggerBombParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _onBombExplode;
        [SerializeField] private AudioClip _audioClip;

        public void TriggerParticle()
        {
            _onBombExplode.Play();
            GameSounds.Instance.OnValidPlay(_audioClip, enablePitchVariation: true);
        }
    }
}
