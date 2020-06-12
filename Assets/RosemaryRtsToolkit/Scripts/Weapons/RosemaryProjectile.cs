using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	[RequireComponent(typeof(RosemarySpawn))]
	public class RosemaryProjectile : MonoBehaviour
	{
		public float autoDestructDelay = 10;
		public float speed = 10;
		public float radius = 0.3f;
		public float damage = 40;
		public LayerMask hitMask;
		public ParticleSystem ps;

		public RosemaryFaction faction { get; private set; }
		public RosemaryWeapon weapon { get; private set; }
		public RosemaryUnit target { get; private set; }
		public TargetPoint targetPoint { get; private set; }



		private RosemarySpawn m_spawn;
		public RosemarySpawn spawn {
			get
			{
				PrepareSpawn();
				return m_spawn;
			}
		}
		private void PrepareSpawn()
		{
			if (!m_spawn)
			{
				m_spawn = GetComponent<RosemarySpawn>();
				if (!m_spawn)
				{
					Debug.LogError($"{nameof(RosemarySpawn)} component not found on GameObject.", this);
				}
				else
				{
					spawner = spawn.spawner as RosemaryProjectileSpawner;
					if (!spawner)
					{
						Debug.LogError($"Spawner is not a {nameof(RosemaryProjectileSpawner)}", spawn.spawner);
					}
				}
			}
		}

		public RosemaryProjectileSpawner spawner { get; private set; }



		private void Awake()
		{
			PrepareSpawn();
		}

		private void OnEnable()
		{
			bool spawning = spawn.spawner.spawnMode != SpawnMode.PreExisting;
			if (spawning || !faction || !faction.isActiveAndEnabled)
			{
				faction = spawner.faction;
			}
			if (spawning || !target || !target.isActiveAndEnabled)
			{
				target = spawner.target;
			}
			if (spawning || !weapon || !weapon.isActiveAndEnabled)
			{
				weapon = spawner.weapon;
			}
			if (spawning || !targetPoint.hasPosition)
			{
				targetPoint = spawner.targetPoint;
			}



			co_autoDestruct = StartCoroutine(Co_AutoDestruct());

			if (target)
			{
				transform.rotation = Quaternion.LookRotation(target.centre);
			}

			if (ps)
			{
				ps.Play();
			}
		}

		private void OnDisable()
		{
			if (ps)
			{
				ps.Stop();
			}

			if (co_autoDestruct != null)
			{
				StopCoroutine(co_autoDestruct);
				co_autoDestruct = null;
			}
		}

		private void FixedUpdate()
		{
			var delta = speed * Time.fixedDeltaTime;

			if (target)
			{
				var targetPosition = target.centre;
				var vec = targetPosition - transform.position;
				if (vec.sqrMagnitude > 0.01f)
				{
					transform.rotation = Quaternion.LookRotation(vec);
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, delta);
				}

				vec = targetPosition - transform.position;
				if (vec.sqrMagnitude < 0.1f)
				{
					try
					{
						var colliders = Physics.OverlapSphere(targetPosition, radius, hitMask, QueryTriggerInteraction.Ignore);
						var units = new HashSet<RosemaryUnit>();
						foreach (var col in colliders)
						{
							var unit = col.GetComponentInParent<RosemaryUnit>();
							if (unit)
							{
								units.Add(unit);
							}
						}
						foreach (var unit in units)
						{
							InflictDamage(unit);
						}
					}
					finally
					{
						spawn.Despawn();
					}
				}
			}
			else
			{
				transform.position += transform.forward * speed;
			}
		}

		private void InflictDamage(RosemaryUnit unit)
		{
			unit.InflictDamage(this);
		}



		Coroutine co_autoDestruct;
		IEnumerator Co_AutoDestruct()
		{
			yield return new WaitForSeconds(autoDestructDelay);
			spawn.Despawn();
		}
	}
}
