using System.Collections.Generic;
using UnityEngine;

public class ParticleCollector : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    private List<ParticleSystem.Particle> particles = new();

    private void OnParticleTrigger()
    {
        int triggeredParticles = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle newParticle = particles[i];
            newParticle.remainingLifetime = 0;
            particles[i] = newParticle;
        }

        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
