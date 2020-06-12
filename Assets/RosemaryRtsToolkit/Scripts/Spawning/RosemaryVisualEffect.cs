using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
    public class RosemaryVisualEffect : AEffectComponent
    {
        public float lifetime = 5;
        public float remainingLifetime { get; private set; }

        public ParticleSystem[] pss = new ParticleSystem[0];

        public override bool IsReadyToDespawn()
        {
            return remainingLifetime <= 0;
        }

        protected virtual void OnEnable()
        {
            if (spawn.spawner.spawnMode == SpawnMode.Spawning)
            {
                remainingLifetime = lifetime;
                foreach (var ps in pss)
                {
                    ps.Play();
                }
            }
        }

        protected virtual void OnDisable()
        {
            remainingLifetime = 0;
            foreach (var ps in pss)
            {
                ps.Stop();
            }
        }

        protected override void Update()
        {
            base.Update();
            remainingLifetime -= Time.deltaTime;
        }
    }
}
