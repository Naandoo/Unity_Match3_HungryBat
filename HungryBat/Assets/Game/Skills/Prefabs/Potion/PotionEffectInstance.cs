using UnityEngine;

namespace Skills
{
    public class PotionEffectInstance : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Renderer _fruitFountainRenderer;
        [SerializeField] private AudioClip _appearPortalAudio, _throwItemsAudio, _appearPotionAudio, _popAudio;

        public void PlayEffect(Texture2D selectedFruitTexture)
        {
            _animator.Play("PotionEffect", 0, 0);
            SetFountainFruitTexture(selectedFruitTexture);
        }

        public Animator GetAnimator() => _animator;

        private void SetFountainFruitTexture(Texture texture) => _fruitFountainRenderer.material.mainTexture = texture;

        public float GetEffectDuration() => _animator.GetCurrentAnimatorStateInfo(0).length;
        public void PlayAppearPortal() => GameSounds.Instance.OnValidPlay(_appearPortalAudio, enablePitchVariation: false);
        public void PlayThrowItems() => GameSounds.Instance.OnValidPlay(_throwItemsAudio, enablePitchVariation: false);
        public void PlayAppearPotion() => GameSounds.Instance.OnValidPlay(_appearPotionAudio, enablePitchVariation: false);
        public void PlayPop() => GameSounds.Instance.OnValidPlay(_popAudio, enablePitchVariation: false);

    }
}