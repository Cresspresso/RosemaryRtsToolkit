using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryUnit : MonoBehaviour
		{
			[SerializeField]
			private float m_speed = 5.0f;
			public float speed {
				get => m_speed;
				set => m_speed = value;
			}

			[SerializeField]
			private float m_maxSeekRange = 5.0f;
			public float maxSeekRange {
				get => m_maxSeekRange;
				set => m_maxSeekRange = value;
			}

			[SerializeField]
			private float m_minSeekRange = 2.0f;
			public float minSeekRange {
				get => m_minSeekRange;
				set => m_minSeekRange = value;
			}

			[SerializeField]
			private UnityEvent m_onSelected = new UnityEvent();
			public UnityEvent onSelected => m_onSelected;

			[SerializeField]
			private UnityEvent m_onDeselected = new UnityEvent();
			public UnityEvent onDeselected => m_onDeselected;



			[Header("Runtime")]

			[SerializeField]
			private Vector3 m_destination;
			public Vector3 destination {
				get => m_destination;
				set => m_destination = value;
			}
			
			[SerializeField]
			private RosemaryUnit m_enemyTarget = null;
			public RosemaryUnit enemyTarget {
				get => m_enemyTarget;
				set
				{
					m_enemyTarget = value;

					if (m_enemyTarget)
					{
						foreach (var motor in GetComponentsInChildren<RosemaryTurretMotor>())
						{
							motor.SetTarget(m_enemyTarget.transform);
						}
					}
				}
			}



			private void Awake()
			{
				OnDeselected();
				destination = transform.position;
			}

			private void Move()
			{
				transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
			}

			private void FixedUpdate()
			{
				var rootMotor = GetComponent<RosemaryTurretMotor>();
				rootMotor.SetTarget(Vector3.Distance(transform.position, destination) < 0.001f
					? 1000 * rootMotor.transform.forward
					: destination);

				if (enemyTarget)
				{
					var enemyPosition = enemyTarget.transform.position;
					var fromEnemyToThis = transform.position - enemyPosition;
					var s = Mathf.Clamp(fromEnemyToThis.magnitude, minSeekRange, maxSeekRange);
					destination = enemyPosition + fromEnemyToThis.normalized * s;
					Move();
				}
				else
				{
					Move();
				}
			}

			public void OnSelected()
			{
				m_onSelected.Invoke();
			}

			public void OnDeselected()
			{
				m_onDeselected.Invoke();
			}

			public void CommandMove(Vector3 destination)
			{
				this.destination = destination;
				this.enemyTarget = null;
			}

			public void CommandAttack(RosemaryUnit otherUnit)
			{
				this.enemyTarget = otherUnit;
			}
		}
	}
}
