using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Simulates parenting this transform to another transform by setting its position in LateUpdate.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="17/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public sealed class RosemaryAltParent : MonoBehaviour
	{
		public TargetPoint target = new TargetPoint(TargetPoint.Mode.Transform, null, default);
		public bool useRotation = false;

		private void OnEnable()
		{
			LateUpdate();
		}

		private void LateUpdate()
		{
			switch (target.mode)
			{
				default:
				case TargetPoint.Mode.None:
					break;
				case TargetPoint.Mode.Point:
					transform.position = target.point;
					transform.rotation = Quaternion.identity;
					break;
				case TargetPoint.Mode.Transform:
					if (target.transform)
					{
						transform.position = target.transform.position;
						transform.rotation = useRotation ? target.transform.rotation : Quaternion.identity;
					}
					break;
			}
		}
	}
}
