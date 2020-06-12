using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Aims at any enemy that ventures into view range.
	///		</para>
	///		<para>
	///			This component's transform is used as the centre of a sphere for detecting enemies.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="04/06/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryAimer : MonoBehaviour
	{
		[SerializeField]
		private bool m_autoFindTurretMotors = false;
		public bool autoFindTurretMotors {
			get => m_autoFindTurretMotors;
			set => m_autoFindTurretMotors = value;
		}
		[SerializeField]
		private RosemaryTurretMotor[] m_initialTurretMotors = new RosemaryTurretMotor[0];
		public HashSet<RosemaryTurretMotor> turretMotors { get; private set; } = new HashSet<RosemaryTurretMotor>();

		[SerializeField]
		private TargetPoint m_aimIdle = new TargetPoint(TargetPoint.Mode.Transform, null, default);
		public TargetPoint aimIdle {
			get => m_aimIdle;
			set
			{
				m_aimIdle = value;
				InvokeTargetChanged();
			}
		}

		[SerializeField]
		private float m_maxAttackRange = 8.0f;
		public float maxAttackRange {
			get => m_maxAttackRange;
			set => m_maxAttackRange = value;
		}

		[SerializeField]
		private float m_maxViewRange = 12.0f;
		public float maxViewRange {
			get => m_maxViewRange;
			set => m_maxViewRange = value;
		}



		private RosemaryUnit m_unit;
		public RosemaryUnit unit {
			get
			{
				FindUnit();
				return m_unit;
			}
		}
		private void FindUnit()
		{
			if (!m_unit)
			{
				m_unit = GetComponentInParent<RosemaryUnit>();
				if (!m_unit)
				{
					Debug.LogError($"{nameof(RosemaryUnit)} component not found in parent.", this);
				}
			}
		}

		public bool IsEnemyAndInViewRange(RosemaryUnit other)
			=> IsInViewRange(other) && other.faction.IsEnemy(this.unit.faction);

		public virtual bool CanAttackUnit(RosemaryUnit other)
			=> IsEnemyAndInViewRange(other) && !other.useResourceReward;

		public virtual IEnumerable<RosemaryUnit> GetTargetUnitsInViewRange()
			=> RosemaryUnit.instances.Where(CanAttackUnit);

		private RosemaryUnit m_target;
		public RosemaryUnit target {
			get
			{
				if (m_target && !m_target.isActiveAndEnabled)
				{
					m_target = unit.targetUnit;
					InvokeTargetChanged();
				}
				return m_target;
			}
			set
			{
				m_target = !value || !value.isActiveAndEnabled ? null : value;
				InvokeTargetChanged();
			}
		}

		private float SqrRange(RosemaryUnit other)
		{
			return (other.centre - transform.position).sqrMagnitude;
		}

		public bool IsInAttackRange(RosemaryUnit other)
		{
			var r = other.centreRadius + maxAttackRange + unit.centreRadius;
			return SqrRange(other) < r * r;
		}

		public bool IsInViewRange(RosemaryUnit other)
		{
			var r = other.centreRadius + maxViewRange + unit.centreRadius; 
			return SqrRange(other) < r * r;
		}

		private void TryChooseDifferentTarget()
		{
			if (unit.targetUnit)
			{
				target = unit.targetUnit;
			}

			var targets = GetTargetUnitsInViewRange();
			if (targets.Any())
			{
				target = targets.OrderBy(SqrRange).First();
			}
		}



		private void Awake()
		{
			FindUnit();

			if (m_initialTurretMotors != null)
			{
				turretMotors.UnionWith(m_initialTurretMotors);
			}

			if (autoFindTurretMotors)
			{
				turretMotors.UnionWith(GetComponentsInChildren<RosemaryTurretMotor>());
			}
		}

		private void OnEnable()
		{
			InvokeTargetChanged(instantAim: true);
		}

		private void Update()
		{
			if (!target || !IsInAttackRange(target))
			{
				if (target && !IsInViewRange(target))
				{
					target = unit.targetUnit;
				}
				TryChooseDifferentTarget();
			}
		}

		private void InvokeTargetChanged(bool instantAim = false)
		{
			TargetPoint targetToAimAt = target
				? TargetPoint.Transform(target.centreTransform)
				: aimIdle;

			foreach (var motor in turretMotors)
			{
				motor.targetToAimAt = targetToAimAt;
				if (instantAim) motor.InstantAim();
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, maxAttackRange);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, maxViewRange);
		}
	}
}
