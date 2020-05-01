using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryUnit : MonoBehaviour
		{
			public Vector3 destination;
			public float speed = 10;
			public float angularSpeed = 90;

			[SerializeField]
			private UnityEvent m_onSelected = new UnityEvent();
			public UnityEvent onSelected => m_onSelected;

			[SerializeField]
			private UnityEvent m_onDeselected = new UnityEvent();
			public UnityEvent onDeselected => m_onDeselected;

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

				transform.rotation = Quaternion.RotateTowards(transform.rotation, orientation, angularSpeed * Time.fixedDeltaTime);
			}

			public void OnSelected()
			{
				m_onSelected.Invoke();
			}

			public void OnDeselected()
			{
				m_onDeselected.Invoke();
			}
		}
	}
}
