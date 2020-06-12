#if false
using Cresspresso.Rosemary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.ExampleProject
{
	public class ExampleDestinationLineRenderer : MonoBehaviour
	{
		private ExampleUnit m_unit;
		public ExampleUnit unit {
			get
			{
				if (m_unit == null)
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
			if (unit && (unit.destinationTarget?.position).HasValue)
			{
				var vec = Vector3.up * 0.2f;
				lineRenderer.SetPosition(0, unit.asTarget.position.Value + vec);
				lineRenderer.SetPosition(1, unit.destinationTarget.position.Value + vec);
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