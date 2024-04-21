using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Effects
{
    public class ParticleCollector : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSystem _ringParticle;

        private List<ParticleSystem.Particle> _particles = new();

        private void Start()
        {
            GameEvents.Instance.OnInitiateLevel.AddListener(() =>
            {
                _particle.gameObject.SetActive(false);
            });
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnInitiateLevel.RemoveListener(() =>
            {
                _particle.gameObject.SetActive(false);
            });
        }

        private void OnParticleTrigger()
        {
            int triggeredParticles = _particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

            for (int i = 0; i < triggeredParticles; i++)
            {
                ParticleSystem.Particle newParticle = _particles[i];
                ParticleSystem.ExternalForcesModule externalForces = _particle.externalForces;
                externalForces.multiplier = 0;
                newParticle.remainingLifetime = 0;
                _particles[i] = newParticle;

                _ringParticle.transform.position = newParticle.position;
                _ringParticle.Play();
            }

            _particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

            GameEvents.Instance.onFruitReachedBat.Invoke();
        }
    }
}
