#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.ExampleProject
{
	public class ExampleEnemyTargetLineRenderer : MonoBehaviour
	{
		[SerializeField]
		private ExampleUnit m_unit;
		public ExampleUnit unit {
			get
			{
				if (!m_unit)
				{
					m_unit = GetComponentInParent<ExampleUnit>();
				}
				return m_unit;
			}
		}

		[SerializeField]
		private LineRenderer m_lineRenderer;
		public LineRenderer lineRenderer {
			get
			{
				if (!m_lineRenderer)
				{
					m_lineRenderer = GetComponent<LineRenderer>();
				}
				return m_lineRenderer;
			}
		}

		public void LateUpdate()
		{
			if (unit && (unit.enemyUnit?.asTarget.position).HasValue)
			{
				var vec = Vector3.up * 0.2f;
				lineRenderer.SetPosition(0, unit.asTarget.position.Value + vec);
				lineRenderer.SetPosition(1, unit.enemyUnit.asTarget.position.Value + vec);
				lineRenderer.enabled = true;
			}
			else
			{
				lineRenderer.enabled = false;
			}
		}
	}
}
#endif