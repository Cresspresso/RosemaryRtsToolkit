using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Moves an <see cref="ATurretComponent"/> to aim at a <see cref="TargetPoint"/>.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryTurretMotor : MonoBehaviour
	{
		[SerializeField]
		private ATurretComponent m_turret;
		public ATurretComponent turret {
			get
			{
				GatherTurret();
				return m_turret;
			}
			set => m_turret = value;
		}
		private void GatherTurret()
		{
			if (!m_turret)
			{
				m_turret = GetComponent<ATurretComponent>();
				if (!m_turret)
				{
					Debug.LogError("Turret is null", this);
				}
			}
		}

		[SerializeField]
		private float m_speed = 180.0f;
		public float speed {
			get => m_speed;
			set => m_speed = value;
		}

		public TargetPoint targetToAimAt;

		public float? CalcAngleToAimAt()
		{
			if (!turret) { return null; }
			Vector3? point = targetToAimAt.position;
			return point.HasValue
				? (float?)turret.CalcAngleToAimAt(point.Value)
				: null;
		}

		private void OnEnable()
		{
			InstantAim();
		}

		public void InstantAim()
		{
			float? angle = CalcAngleToAimAt();
			if (angle.HasValue)
			{
				turret.angle = angle.Value;
			}
		}

		private void LateUpdate()
		{
			float? angle = CalcAngleToAimAt();
			if (angle.HasValue)
			{
				turret.angle = Mathf.MoveTowardsAngle(
					current: turret.angle,
					target: angle.Value,
					maxDelta: speed * Time.deltaTime);
			}
		}
	}
}
