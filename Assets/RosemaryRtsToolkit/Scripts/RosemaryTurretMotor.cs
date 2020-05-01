using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Schema;
using UnityEngine;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryTurretMotor : MonoBehaviour
		{
			[SerializeField]
			private RosemaryTurretAngleComponent m_turret;
			public RosemaryTurretAngleComponent turret {
				get
				{
					if (!m_turret)
					{
						m_turret = GetComponent<RosemaryTurretAngleComponent>();
					}
					return m_turret;
				}
				set => m_turret = value;
			}

			[SerializeField]
			private float m_speed = 180.0f;
			public float speed {
				get => m_speed;
				set => m_speed = value;
			}

			[SerializeField]
			private Transform m_target;
			public Transform target {
				get => m_target;
				set => m_target = value;
			}

			[SerializeField]
			private Vector3 m_fallbackTargetPoint;
			public Vector3 fallbackTargetPoint {
				get => m_fallbackTargetPoint;
				set => m_fallbackTargetPoint = value;
			}

			public Vector3 GetTargetPoint()
				=> target
				? target.position
				: fallbackTargetPoint;

			public void SetTarget(Vector3 point)
			{
				fallbackTargetPoint = point;
				target = null;
			}

			public void SetTarget(Transform target)
			{
				this.target = target;
			}

			public float CalcDesiredAngle()
				=> turret ? turret.CalcAngleFromWorldPoint(GetTargetPoint())
				: 0.0f;

			private void FixedUpdate()
			{
				if (turret)
				{
					turret.angle = Mathf.MoveTowardsAngle(
						current: turret.angle,
						target: CalcDesiredAngle(),
						maxDelta: speed * Time.fixedDeltaTime);
				}
			}
		}
	}
}
