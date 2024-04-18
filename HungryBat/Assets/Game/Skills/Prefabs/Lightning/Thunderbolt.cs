using UnityEngine;
using Controllers;

namespace Skills
{
    public class Thunderbolt : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _thunderboltParticle;
        [SerializeField] private ParticleSystem _lightningStrikeParticle;
        [SerializeField] private AudioClip _thunderboltInvoked;
        [SerializeField] private AudioClip _thunderboltStrike;
        public void PlayThunderboltAnim()
        {
            _animator.Play("Thunderbolt");
            _thunderboltParticle.Play();
            GameSounds.Instance.OnValidPlay(_thunderboltInvoked, enablePitchVariation: false);
        }

        public void PlayLightningStrikeAnim()
        {
            _animator.Play("LightningStrike");
            _lightningStrikeParticle.Play();
            GameSounds.Instance.OnValidPlay(_thunderboltStrike, enablePitchVariation: false);
        }
    }
}
