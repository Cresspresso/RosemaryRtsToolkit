using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public partial class RosemaryUnit
	{
		[Header("Health")]

		[SerializeField]
		private float m_initialHealth = 100;
		public float initialHealth => m_initialHealth;

		public string deathEffectSpawnerName = "@???";

		public bool useResourceReward = false;
		public ResourceAmount resourceReward = new ResourceAmount();

		public float health { get; private set; }


		private RosemarySpawner m_deathEffectSpawner;
		public RosemarySpawner deathEffectSpawner {
			get
			{
				FindDeathEffectSpawner();
				return m_deathEffectSpawner;
			}
		}
		private void FindDeathEffectSpawner()
		{
			if (!m_deathEffectSpawner)
			{
				m_deathEffectSpawner = FindObjectsOfType<RosemarySpawner>()
					.FirstOrDefault(s => s.name == deathEffectSpawnerName);
				if (!m_deathEffectSpawner)
				{
					Debug.LogError($"Death Effect Spawner with name \"{deathEffectSpawnerName}\" not found.", this);
				}
			}
		}



		private void OnEnableHealthPartial()
		{
			FindDeathEffectSpawner();

			if (m_initialHealth <= 0)
			{
				Debug.LogError("Initial Health <= 0", this);
			}
			health = m_initialHealth;
		}

		public void InflictDamage(RosemaryProjectile projectile)
		{
			health -= projectile.damage;
			if (health <= 0)
			{
				if (useResourceReward)
				{
					projectile.faction.IncreaseResourceBalance(resourceReward.resourceName, resourceReward.amount);
				}

				SelfDestruct();
			}
			
			//if (isIdle)
			//{
			//	CommandAttack(projectile.weapon.unit);
			//}
		}

		public void SelfDestruct()
		{
			health = 0;
			try
			{
				deathEffectSpawner.Spawn(transform.position, transform.rotation);
			}
			finally
			{
				spawn.Despawn();
			}
		}
	}
}
