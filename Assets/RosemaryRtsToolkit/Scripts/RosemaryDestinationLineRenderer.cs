using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryDestinationLineRenderer : MonoBehaviour
		{
			[SerializeField]
			private RosemaryUnit m_unit;
			public RosemaryUnit unit {
				get
				{
					if (!m_unit)
					{
						m_unit = GetComponentInParent<RosemaryUnit>();
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
				var vec = Vector3.up * 0.2f;
				lineRenderer.SetPosition(0, unit.transform.position + vec);
				lineRenderer.SetPosition(1, unit.destination + vec);
			}
		}
	}
}
