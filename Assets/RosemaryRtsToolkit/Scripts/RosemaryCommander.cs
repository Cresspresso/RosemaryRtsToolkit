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

			private void RaycastCommand()
			{
				var pointerPosition = inputProvider.GetPointerPositionInScreenSpace();
				var ray = selector.cam.ScreenPointToRay(pointerPosition);
				RaycastHit hit;
				if (Physics.Raycast(
					ray,
					out hit,
					Mathf.Infinity,
					selector.unitLayerMask,
					QueryTriggerInteraction.Ignore))
				{
					var otherUnit = hit.collider.GetComponentInParent<RosemaryUnit>();
					if (otherUnit)
					{
						foreach (var unit in selector.selectedUnits)
						{
							unit.CommandAttack(otherUnit);
						}
						return;
					}
				}

				if (Physics.Raycast(
					selector.cam.ScreenPointToRay(inputProvider.GetPointerPositionInScreenSpace()),
					out hit,
					Mathf.Infinity,
					selector.terrainLayerMask,
					QueryTriggerInteraction.Ignore))
				{
					var destination = hit.point;
					foreach (var unit in selector.selectedUnits)
					{
						unit.CommandMove(destination);
					}
					return;
				}
			}

			private void Update()
			{
				if (inputProvider != null)
				{
					if (inputProvider.DidButtonGoDown())
					{
						RaycastCommand();
					}
				}
			}
		}
	}
}
