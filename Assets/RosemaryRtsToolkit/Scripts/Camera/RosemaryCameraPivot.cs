using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cresspresso.Rosemary
{
	/// <summary>
	/// An object which is polled to provide input for <see cref="RosemaryCameraPivot"/>.
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///		version="1.0"
	///		author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public interface IRosemaryCameraPivotInputProvider
	{
		float GetSidewaysInput();
		float GetForwardsInput();
		float GetBoomInput();
	}

	/// <summary>
	///		<para>
	///			Top-down RTS camera.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///		version="1.0"
	///		author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	/// <todo>Comments in more detail</todo>
	public class RosemaryCameraPivot : MonoBehaviour
	{
		public IRosemaryCameraPivotInputProvider inputProvider;
		private float sidewaysInput;
		private float forwardsInput;

		public enum VerticalAxis { Z, Y }
		public VerticalAxis verticalAxis = VerticalAxis.Z;

		public AnimationCurve speedRelativeToBoom = new AnimationCurve(
			new Keyframe(0, 1),
			new Keyframe(1, 5));

		public Bounds bounds = new Bounds(
			center: new Vector3(500, 0, 500),
			size: new Vector3(1000, 0, 1000));

		public Transform boomTransform;

		[SerializeField]
		private float m_boomNorm = 0.5f;
		public float boomNorm {
			get => m_boomNorm;
			set
			{
				m_boomNorm = Mathf.Clamp01(value);
				boomDistance = EvaluateBoom();
			}
		}
		public float boomDistance { get; private set; }

		public AnimationCurve boomCurve = new AnimationCurve(
			new Keyframe(0, 10),
			new Keyframe(1, 100));
		private float EvaluateBoom() => boomCurve.Evaluate(boomNorm);



		private void Start()
		{
			boomNorm = m_boomNorm;

			if (!boomTransform)
			{
				Debug.LogWarning("Boom Transform is null", this);
			}
		}

		private void Update()
		{
			if (inputProvider == null)
			{
				sidewaysInput = 0;
				forwardsInput = 0;
			}
			else
			{
				var boomInput = inputProvider.GetBoomInput();
				if (Mathf.Abs(boomInput) > 0.001f)
				{
					boomNorm += boomInput;
				}

				sidewaysInput = inputProvider.GetSidewaysInput();
				forwardsInput = inputProvider.GetForwardsInput();
			}
		}

		private void FixedUpdate()
		{
			// Move this transform according to input and this transform's orientation.
			var up = transform.parent ? transform.parent.up : Vector3.up;
			var rightpl = Vector3.ProjectOnPlane(transform.right, up);
			var forwpl = Vector3.ProjectOnPlane(verticalAxis == VerticalAxis.Z ? transform.forward : transform.up, up);
			if (forwpl.sqrMagnitude < 0.001f) { forwpl = Vector3.ProjectOnPlane(transform.up, up); }
			forwpl.Normalize();
			rightpl.Normalize();
			var pos = transform.localPosition;
			var s = speedRelativeToBoom.Evaluate(boomNorm) * Time.fixedDeltaTime;
			pos += rightpl * (sidewaysInput * s);
			pos += forwpl * (forwardsInput * s);
			transform.localPosition = bounds.ClosestPoint(pos);

			if (boomTransform)
			{
				var b = boomTransform.localPosition;
				b.z = -boomDistance;
				boomTransform.localPosition = b;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			var p = transform.TransformPoint(new Vector3(0, 0, -boomCurve.keys.Min(k => k.value)));
			Gizmos.DrawLine(transform.position, p);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(p, transform.TransformPoint(new Vector3(0, 0, -boomCurve.keys.Max(k => k.value))));

			if (transform.parent)
			{
				Gizmos.matrix = transform.parent.localToWorldMatrix;
			}
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
	}
}
