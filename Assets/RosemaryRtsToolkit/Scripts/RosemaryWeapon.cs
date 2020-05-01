using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryWeapon : MonoBehaviour
		{
			[SerializeField]
			private RosemaryUnit m_unit;
			public RosemaryUnit unit {
				get
				{
					if (!m_unit)
					{
						m_unit = GetComponentInParent<RosemaryUnit>();
						if (!m_unit)
						{
							Debug.LogWarning($"{nameof(unit)} is null", this);
						}
					}
					return m_unit;
				}
				set => m_unit = value;
			}

			[SerializeField]
			private float m_maxAttackAngle = 10.0f;
			public float maxAttackAngle {
				get => m_maxAttackAngle;
				set => m_maxAttackAngle = value;
			}

			[SerializeField]
			private float m_minAttackRange = 0.0f;
			public float minAttackRange {
				get => m_minAttackRange;
				set => m_minAttackRange = value;
			}

			[SerializeField]
			private float m_maxAttackRange = 10.0f;
			public float maxAttackRange {
				get => m_maxAttackRange;
				set => m_maxAttackRange = value;
			}

			[SerializeField]
			private UnityEvent m_onDischarge = new UnityEvent();
			public UnityEvent onDischarge => m_onDischarge;

			[SerializeField]
			private GameObject debugVisuals;

			private void Update()
			{
				debugVisuals.SetActive(false);
				if (unit)
				{
					var target = unit.enemyTarget;
					if (target)
					{
						var toTarget = target.transform.position - transform.position;
						if (Vector3.Angle(toTarget, transform.forward) <= maxAttackAngle)
						{
							onDischarge.Invoke();
							debugVisuals.SetActive(true);
						}
					}
				}
			}
		}
	}
}
