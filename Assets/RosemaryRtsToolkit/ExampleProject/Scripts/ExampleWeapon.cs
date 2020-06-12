#if false
using Cresspresso.Rosemary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.ExampleProject
{
	public class ExampleWeapon : MonoBehaviour, IWeapon
	{
#pragma warning disable CS0649
		[SerializeField]
		private ExampleProjectile m_projectilePrefab;
#pragma warning restore CS0649
		public RosemarySpawner projectileSpawner { get; set; }

		[SerializeField]
		private float m_maxRange = 10;
		public float maxRange {
			get => m_maxRange;
			set => m_maxRange = value;
		}

		[SerializeField]
		private float m_damage = 1;
		public float damage {
			get => m_damage;
			set => m_damage = value;
		}

		public class ProjectileSpawnEventArgs : IProjectileSpawnEventArgs
		{
			public Vector3 position { get; set; }
			public Quaternion rotation { get; set; }
			public IWeapon weapon { get; set; }
			public ITarget target { get; set; }
		}

		private void Awake()
		{
			projectileSpawner =
				(from spawner in FindObjectsOfType<RosemarySpawner>()
				where spawner.prefab as ExampleProjectile == m_projectilePrefab
				select spawner
				).FirstOrDefault();
			if (!projectileSpawner)
			{
				Debug.LogError("Spawner not found for projectile prefab", this);
			}
		}

		public void Fire(ITarget target)
		{
			if (!projectileSpawner)
			{
				Debug.LogError("Spawner not found for projectile prefab", this);
				return;
			}
			else
			{
				projectileSpawner.Spawn(new ProjectileSpawnEventArgs
				{
					position = transform.position,
					rotation = transform.rotation,
					weapon = this,
					target = target,
				});
			}
		}

		public bool IsReadyToFire(ITarget target)
		{
			var position = target?.position;
			return position.HasValue && Vector3.Distance(position.Value, transform.position) <= maxRange;
		}
	}
}
#endif