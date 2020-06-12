using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Despawns after a particle system stops playing.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="17/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	[RequireComponent(typeof(RosemarySpawn))]
	public class RosemaryCommandEffect : AEffectComponent
	{
		[SerializeField]
		private ParticleSystem m_particles;
		public ParticleSystem particles {
			get => m_particles;
			set => m_particles = value;
		}

		[SerializeField]
		private RosemaryAltParent m_altParent;
		public RosemaryAltParent altParent {
			get => m_altParent;
			set => m_altParent = value;
		}

		public override bool IsReadyToDespawn()
		{
			return !particles || !particles.isPlaying;
		}

		private void OnEnable()
		{
			if (altParent)
			{
				var spawn = GetComponent<RosemarySpawn>();
				var commandEffectSpawner = spawn.spawner.GetComponent<RosemaryCommandEffectSpawner>();
				altParent.target = commandEffectSpawner.target;
			}
		}
	}
}
