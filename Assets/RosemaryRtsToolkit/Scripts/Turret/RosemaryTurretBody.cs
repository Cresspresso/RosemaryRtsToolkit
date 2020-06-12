using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Rotates yaw (left/right).
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryTurretBody : ATurretComponent
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
				m_minAngle = Mathf.Clamp(m_minAngle, -179.99f, 179.99f);
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
				m_maxAngle = Mathf.Clamp(m_maxAngle, -179.99f, 179.99f);
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
		private float m_angle;
		protected override float protectedAngle {
			get => m_angle;
			set => m_angle = value;
		}

		public override void Apply()
		{
			transform.localEulerAngles = new Vector3(0, angle, 0);
		}

		public override float Constrain(float angle)
		{
			angle = Mathf.Repeat(angle, 360.0f);
			if (angle > 180.0f)
			{
				angle -= 360.0f;
			}
			if (useConstraints)
			{
				angle = Mathf.Clamp(angle, minAngle, maxAngle);
			}
			return angle;
		}

		protected override void Awake()
		{
			minAngle = m_minAngle;
			maxAngle = m_maxAngle;
			base.Awake();
		}

		public override float CalcAngleToAimAt(Vector3 worldPoint)
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
