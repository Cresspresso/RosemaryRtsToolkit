using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Base class for <see cref="RosemaryTurretBody"/> and <see cref="RosemaryTurretHead"/>.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public abstract class ATurretComponent : MonoBehaviour
	{
		protected abstract float protectedAngle { get; set; }

		/// <summary>
		/// Call this after changing any properties that constrain <see cref="angle"/>.
		/// </summary>
		public abstract void Apply();

		/// <summary>
		/// Constrains the angle value for assignment to the <see cref="angle"/> property.
		/// </summary>
		public abstract float Constrain(float angle);

		/// <summary>
		/// Calculates the desired angle that would align the turret component's
		/// forward direction with the desired direction,
		/// after projecting it onto the plane of rotation.
		/// </summary>
		/// <param name="worldDirection">Position in world space to look at.</param>
		/// <seealso cref="angle"/>
		public abstract float CalcAngleToAimAt(Vector3 worldPoint);

		/// <summary>
		/// Current angle in degrees, constrained to a certain range (for example, -180 to +180).
		/// </summary>
		public float angle {
			get => protectedAngle;
			set
			{
				protectedAngle = Constrain(value);
				Apply();
			}
		}

		protected virtual void Awake()
		{
			angle = protectedAngle;
		}



		/// <summary>
		/// Returns the angle in degrees between <paramref name="vector"/> and <paramref name="zeroAxis"/>,
		/// in the range -180 to +180.
		/// </summary>
		/// <param name="vector">The vector to check the angle against the <paramref name="zeroAxis"/></param>
		/// <param name="zeroAxis">If <paramref name="vector"/> has the same direction as this axis vector, angle is zero.</param>
		/// <param name="positiveAxis">
		/// If <paramref name="vector"/> has a similar direction to this axis vector, angle is positive.
		/// If <paramref name="vector"/> has a direction opposite to this axis vector, angle is negative.
		/// </param>
		/// <returns>Angle in degrees, in the range -180 to +180.</returns>
		public static float SignedAngle(Vector3 vector, Vector3 zeroAxis, Vector3 positiveAxis)
		{
			var angle = Vector3.Angle(vector, zeroAxis);
			if (Vector3.Dot(vector, positiveAxis) < 0.0f)
			{
				angle = -angle;
			}
			return angle;
		}

		public static float MirrorAngle(float angle)
		{
			if (angle > 90.0f) { return 180.0f - angle; }
			else if (angle < -90.0f) { return -180.0f - angle; }
			else { return angle; }
		}
	}
}
