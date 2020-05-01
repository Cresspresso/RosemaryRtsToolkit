using Cresspresso.Rosemary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso
{
	namespace Rosemary
	{
		public interface IRosemaryCommanderInputProvider
		{
			bool DidButtonGoDown();
			bool DidButtonGoUp();
			Vector2 GetPointerPositionInScreenSpace();
		}

		public class RosemaryCommander : MonoBehaviour
		{
			public IRosemaryCommanderInputProvider inputProvider;

			public bool useMainCamera = true;
			[SerializeField]
			private Camera m_camera;
			public Camera cam {
				get => useMainCamera ? Camera.main : m_camera;
				set
				{
					m_camera = value;
					useMainCamera = !(bool)m_camera;
				}
			}

			public LayerMask terrainLayerMask = ~0;

			public RosemarySelector selector;

			private void Awake()
			{
				if (!selector)
				{
					selector = GetComponent<RosemarySelector>();
					if (!selector)
					{
						selector = FindObjectOfType<RosemarySelector>();
					}
				}
			}

			private void Update()
			{
				if (inputProvider != null)
				{
					if (inputProvider.DidButtonGoDown())
					{
						if (Physics.Raycast(
							cam.ScreenPointToRay(inputProvider.GetPointerPositionInScreenSpace()),
							out RaycastHit hit,
							Mathf.Infinity,
							terrainLayerMask,
							QueryTriggerInteraction.Ignore))
						{
							var destination = hit.point;
							foreach (var unit in selector.selectedUnits)
							{
								unit.destination = destination;
							}
						}
					}
				}
			}
		}
	}
}
