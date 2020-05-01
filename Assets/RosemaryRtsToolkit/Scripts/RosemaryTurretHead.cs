using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryTurretHead : RosemaryTurretAngleComponent
		{
			[SerializeField]
			private RosemaryTurretBody m_body;
			public RosemaryTurretBody body {
				get
				{
					if (!m_body)
					{
						m_body = GetComponentInParent<RosemaryTurretBody>();
						if (!m_body)
						{
							Debug.LogWarning("body is null", this);
						}
					}
					return m_body;
				}
				set => m_body = value;
			}

			public float GetBodyAngleNorm()
			{
				var bodyAngle = body ? body.angle : 0.0f;
				var bodyAngleNorm = (bodyAngle + 90.0f) / 180.0f;
				return bodyAngleNorm;
			}

			private float EvaluateCurve(AnimationCurve curve, float bodyAngleNorm)
			{
				var headAngleNorm = Mathf.Clamp01(curve.Evaluate(bodyAngleNorm));
				var headAngle = headAngleNorm * 179.98f - 89.99f;
				return headAngle;
			}



			[Header("Constraints")]

			[Tooltip(@"
Time axis relates to `body.angle`.
Time ranges from 0 to 1,
where 0 means -180 degrees,
and 1 means +180 degrees.
Value axis relates to minimum value for `angle`.
Value ranges from 0 to 1,
where 0 means -90 degrees (looking up),
and 1 means +90 degrees (looking down).
")]
			[SerializeField]
			private AnimationCurve m_minHeadAngleWrtBodyAngle = MakeDefaultMinCurve();
			public AnimationCurve minAngleWrtBodyAngle {
				get => m_minHeadAngleWrtBodyAngle;
				set => m_minHeadAngleWrtBodyAngle = value != null ? value : MakeDefaultMinCurve();
			}
			private static AnimationCurve MakeDefaultMinCurve() => new AnimationCurve(
				new Keyframe(0, 0),
				new Keyframe(1, 0));

			public float EvaluateMinAngle(float bodyAngleNorm) =>
				EvaluateCurve(minAngleWrtBodyAngle, bodyAngleNorm);



			[Tooltip(@"
Time axis relates to `body.angle`.
Time ranges from 0 to 1,
where 0 means -180 degrees,
and 1 means +180 degrees.
Value axis relates to maximum value for `angle`.
Value ranges from 0 to 1,
where 0 means -90 degrees (looking up),
and 1 means +90 degrees (looking down).
")]
			[SerializeField]
			private AnimationCurve m_maxHeadAngleWrtBodyAngle = MakeDefaultMaxCurve();
			public AnimationCurve maxAngleWrtBodyAngle {
				get => m_maxHeadAngleWrtBodyAngle;
				set => m_maxHeadAngleWrtBodyAngle = value != null ? value : MakeDefaultMaxCurve();
			}
			private static AnimationCurve MakeDefaultMaxCurve() => new AnimationCurve(
				new Keyframe(0, 1),
				new Keyframe(1, 1));

			public float EvaluateMaxAngle(float bodyAngleNorm) =>
				EvaluateCurve(maxAngleWrtBodyAngle, bodyAngleNorm);



			[Header("Runtime")]

			[Tooltip(@"
Current angle in degrees.
Rotation around local X axis.
Ranges from -90 (looking down)
to +90 (looking up).
")]
			[SerializeField]
			private float m_angle = 0;
			public override float angle {
				get => m_angle;
				set
				{
					var bodyAngleNorm = GetBodyAngleNorm();
					m_angle = Mathf.Clamp(value,
						min: EvaluateMinAngle(bodyAngleNorm),
						max: EvaluateMaxAngle(bodyAngleNorm));

					transform.localEulerAngles = new Vector3(m_angle, 0, 0);
				}
			}



			private void Awake()
			{
				ApplyConstraints();
			}



			public override float CalcAngleFromWorldPoint(Vector3 worldPoint)
			{
				var directionFromBody = worldPoint - body.transform.position;
				var directionOnBodyPlane = Vector3.ProjectOnPlane(directionFromBody, body.transform.up);
				var desiredAngleOnBodyPlane = SignedAngle(directionOnBodyPlane,
					zeroAxis: body.transform.forward,
					positiveAxis: body.transform.right);

				var q = Quaternion.AngleAxis(-desiredAngleOnBodyPlane, body.transform.up);
				var worldDirection = worldPoint - transform.position;
				var directionOnHeadPlane = q * worldDirection;
				var desiredHeadAngle = SignedAngle(directionOnHeadPlane,
					zeroAxis: transform.parent.forward,
					positiveAxis: -transform.parent.up);
				return desiredHeadAngle;
			}
		}
	}
}
