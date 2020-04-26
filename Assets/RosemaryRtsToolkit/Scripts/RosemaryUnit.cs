using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryUnit : MonoBehaviour
		{
			public GameObject selectedVisuals;
			public Vector3 destination;
			public float speed = 10;
			public float rotateSpeed = 90;
			private Quaternion orientation;

			private void Awake()
			{
				OnDeselected();
				destination = transform.position;
			}

			private void FixedUpdate()
			{
				transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);

				var displacement = destination - transform.position;
				if (displacement.sqrMagnitude > 0.001f)
				{
					var up = transform.parent ? transform.parent.up : Vector3.up;
					orientation = Quaternion.LookRotation(displacement, up);
				}

				transform.rotation = Quaternion.RotateTowards(transform.rotation, orientation, rotateSpeed * Time.fixedDeltaTime);
			}

			public void OnSelected()
			{
				if (selectedVisuals)
				{
					selectedVisuals.SetActive(true);
				}
			}

			public void OnDeselected()
			{
				if (selectedVisuals)
				{
					selectedVisuals.SetActive(false);
				}
			}
		}
	}
}
