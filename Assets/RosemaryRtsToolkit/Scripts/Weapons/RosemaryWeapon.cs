using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Fires projectiles at the target enemy.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="04/06/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryWeapon : MonoBehaviour
	{
		[SerializeField]
		private RosemaryAimer m_aimer;
		public RosemaryAimer aimer {
			get
			{
				FindAimer();
				return m_aimer;
			}
		}
		private void FindAimer()
		{
			if (!m_aimer)
			{
				m_aimer = GetComponentInParent<RosemaryAimer>();
				if (!m_aimer)
				{
					Debug.LogError($"{nameof(RosemaryAimer)} component not found in parent.", this);
				}
			}
		}
		public RosemaryUnit unit => aimer.unit;

		[SerializeField]
		private Transform m_gunPivot;
		public Transform gunPivot {
			get
			{
				if (!m_gunPivot)
				{
					m_gunPivot = transform;
				}
				return m_gunPivot;
			}
			set => m_gunPivot = value;
		}

		[SerializeField]
		private Transform m_muzzle;
		public Transform muzzle {
			get
			{
				if (!m_muzzle)
				{
					m_muzzle = transform;
				}
				return m_muzzle;
			}
			set => m_muzzle = value;
		}



#pragma warning disable CS0649
		[SerializeField]
		private string m_projectileSpawnerName = "@???";
#pragma warning restore CS0649
		public string projectileSpawnerName => m_projectileSpawnerName;

		private RosemaryProjectileSpawner m_projectileSpawner;
		public RosemaryProjectileSpawner projectileSpawner {
			get
			{
				FindProjectileSpawner();
				return m_projectileSpawner;
			}
		}
		private void FindProjectileSpawner()
		{
			if (!m_projectileSpawner)
			{
				m_projectileSpawner = FindObjectsOfType<RosemaryProjectileSpawner>()
					.FirstOrDefault(s => s.name == projectileSpawnerName);
				if (!m_projectileSpawner)
				{
					Debug.LogError($"{nameof(RosemaryProjectileSpawner)} with name \"{projectileSpawnerName}\" not found.", this);
				}
			}
		}



#pragma warning disable CS0649
		[SerializeField]
		private float m_reloadDuration = 2;
#pragma warning restore CS0649
		public float reloadDuration => m_reloadDuration;

		public float reloadTimeRemaining { get; private set; } = 0.0f;



		public bool useMaxAttackAngle = true;
		[Range(0.0f, 180.0f)]
		public float maxAttackAngle = 3.0f;

		private bool IsInAttackAngle(RosemaryUnit other)
			=> !useMaxAttackAngle
			|| (Vector3.Angle(gunPivot.forward, other.centre - gunPivot.position)
			< maxAttackAngle);



		private void Awake()
		{
			FindAimer();
			FindProjectileSpawner();
		}

		private void Update()
		{
			reloadTimeRemaining = Mathf.Max(0, reloadTimeRemaining - Time.deltaTime);

			var target = aimer.target;
			if (target
				&& reloadTimeRemaining <= 0.0f
				&& aimer.IsInAttackRange(target)
				&& IsInAttackAngle(target))
			{
				// Discharge weapon.
				// Launch/fire projectile.
				reloadTimeRemaining = reloadDuration;
				projectileSpawner.Spawn(
					muzzle.position,
					muzzle.rotation,
					this,
					unit.faction,
					target,
					TargetPoint.Transform(target.centreTransform)
					);
			}
		}
	}
}
