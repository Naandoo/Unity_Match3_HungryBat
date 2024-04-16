using UnityEngine;

namespace Effects
{
    public class ClickButtonEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickButton;

        public void OnClickButtonEffect()
        {
            GameSounds.Instance.OnValidPlay(_clickButton, enablePitchVariation: false);
        }
    }
}
