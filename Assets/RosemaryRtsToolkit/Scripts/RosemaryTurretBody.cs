using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryTurretBody : RosemaryTurretAngleComponent
		{
			[Header("Constraints")]

			[Tooltip(@"
If true, angle will be constrained to the range from minAngle to maxAngle.
")]
			[SerializeField]
			private bool m_useConstraints = false;
			public bool useConstraints {
				get => m_useConstraints;
				set => m_useConstraints = value;
			}

			[Tooltip(@"
Minimum value for angle in degrees.
This limit is only applied if useBodyAngleLimits is set to true.
Rotation around local Y axis.
Ranges from -180 (anticlockwise from top-down view)
to +180 (clockwise).
")]
			[SerializeField]
			private float m_minAngle = -45.0f;
			public float minAngle {
				get => m_minAngle;
				set
				{
					m_angle = Mathf.Clamp(m_minAngle, -179.99f, 179.99f);
				}
			}

			[Tooltip(@"
Maximum value for angle in degrees.
This limit is only applied if useBodyAngleLimits is set to true.
Rotation around local Y axis.
Ranges from -180 (anticlockwise from top-down view)
to +180 (clockwise).
")]
			[SerializeField]
			private float m_maxAngle = 45.0f;
			public float maxAngle {
				get => m_maxAngle;
				set
				{
					m_angle = Mathf.Clamp(m_maxAngle, -179.99f, 179.99f);
				}
			}



			[Header("Runtime")]

			[Tooltip(@"
Current angle in degrees.
Rotation around local Y axis.
Ranges from -180 (anticlockwise from top-down view)
to +180 (clockwise).
")]
			[SerializeField]
			private float m_angle = 0;
			public override float angle {
				get => m_angle;
				set
				{
					m_angle = Mathf.Repeat(value, 360.0f);
					if (m_angle > 180.0f)
					{
						m_angle -= 360.0f;
					}
					if (useConstraints)
					{
						m_angle = Mathf.Clamp(m_angle, minAngle, maxAngle);
					}

					transform.localEulerAngles = new Vector3(0, m_angle, 0);
				}
			}



			private void Awake()
			{
				minAngle = m_minAngle;
				maxAngle = m_maxAngle;
				ApplyConstraints();
			}



			public override float CalcAngleFromWorldPoint(Vector3 worldPoint)
			{
				var parent = transform.parent;
				var up = parent ? parent.up : Vector3.up;
				var right = parent ? parent.right : Vector3.right;
				var forward = parent ? parent.forward : Vector3.forward;

				var worldDirection = worldPoint - transform.position;
				var directionOnPlane = Vector3.ProjectOnPlane(worldDirection, up);
				var desiredAngle = SignedAngle(directionOnPlane,
					zeroAxis: forward,
					positiveAxis: right);
				return desiredAngle;
			}
		}
	}
}
