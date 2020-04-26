using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using System.Linq;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryCameraPivot : MonoBehaviour
		{
			public float speedSideways = 10.0f;
			public float speedForwards = 10.0f;
			public enum VerticalAxis { Z, Y }
			public VerticalAxis verticalAxis = VerticalAxis.Z;
			public AnimationCurve speedRelativeToBoom = new AnimationCurve(
				new Keyframe(0, 1),
				new Keyframe(1, 5));
			public string horizontal = "Horizontal";
			public string vertical = "Vertical";
			public string scroll = "Mouse ScrollWheel";
			public Bounds bounds = new Bounds(
				center: new Vector3(500, 0, 500),
				size: new Vector3(1000, 0, 1000));

			public float boomNorm = 0.5f;
			public float boomSensitivity = 0.5f;
			public AnimationCurve boomCurve = new AnimationCurve(
				new Keyframe(0, 10),
				new Keyframe(1, 100));
			private float CalcBoom() => boomCurve.Evaluate(boomNorm);
			public float boomDistance { get; private set; }
			public Transform boomTransform;

			private void Update()
			{
				boomNorm += boomSensitivity * -Input.GetAxis(scroll);
				boomNorm = Mathf.Clamp01(boomNorm);
				boomDistance = CalcBoom();
			}

			private void FixedUpdate()
			{
				// Move this transform with player input according to this transform's orientation.
				var up = transform.parent ? transform.parent.up : Vector3.up;
				var rightpl = Vector3.ProjectOnPlane(transform.right, up);
				var forwpl = Vector3.ProjectOnPlane(verticalAxis == VerticalAxis.Z ? transform.forward : transform.up, up);
				if (forwpl.sqrMagnitude < 0.001f) { forwpl = Vector3.ProjectOnPlane(transform.up, up); }
				forwpl.Normalize();
				rightpl.Normalize();
				var pos = transform.localPosition;
				var sb = speedRelativeToBoom.Evaluate(boomNorm);
				pos += rightpl * speedSideways * sb * Input.GetAxis(horizontal) * Time.fixedDeltaTime;
				pos += forwpl * speedForwards * sb * Input.GetAxis(vertical) * Time.fixedDeltaTime;
				transform.localPosition = bounds.ClosestPoint(pos);

				var b = boomTransform.localPosition;
				b.z = -boomDistance;
				boomTransform.localPosition = b;
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
}
