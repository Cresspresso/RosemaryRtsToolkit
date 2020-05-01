using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso
{
	namespace Rosemary
	{
		public interface IRosemarySelectorInputProvider
		{
			bool DidButtonGoDown();
			bool DidButtonGoUp();
			Vector2 GetPointerPositionInScreenSpace();
		}

		public class RosemarySelector : MonoBehaviour
		{
			public IRosemarySelectorInputProvider inputProvider;

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
			public LayerMask unitLayerMask = ~0;
			public LayerMask terrainLayerMask = ~0;
			public Vector2 boxSelectMinSizeInViewport = new Vector2(0.01f, 0.01f);
			public RectTransform boxSelectRectTransform;

			[SerializeField]
			private List<RosemaryUnit> m_selectedUnits = new List<RosemaryUnit>();
			public IReadOnlyList<RosemaryUnit> selectedUnits => m_selectedUnits;
			private bool isPointerDown = false;
			private bool isBoxSelecting = false;
			private Vector2 initialPointerPosition;
			private Vector2 currentPointerPosition;
			private Rect boxSelectRect;

			private void Awake()
			{
				isPointerDown = false;
				isBoxSelecting = false;
				OnBoxSelectRectChanged();
			}

			private void ClearSelectedUnits()
			{
				foreach (var unit in selectedUnits)
				{
					unit.OnDeselected();
				}
				m_selectedUnits.Clear();
			}

			private void AddUnitToSelection(RosemaryUnit unit)
			{
				m_selectedUnits.Add(unit);
				unit.OnSelected();
			}

			private void OnPointerUp()
			{
				if (!isPointerDown) { return; }

				try
				{
					var camera = this.cam;
					if (isBoxSelecting)
					{
						ClearSelectedUnits();
						// TODO optimise
						foreach (var unit in
							from unit in FindObjectsOfType<RosemaryUnit>()
							let screenPosition = camera.WorldToScreenPoint(unit.transform.position)
							where screenPosition.z >= 0 && boxSelectRect.Contains(screenPosition)
							select unit)
						{
							AddUnitToSelection(unit);
						}
					}
					else
					{
						// Just a pointer tap therefore Single Select.
						bool found = false;
						if (Physics.Raycast(
							camera.ScreenPointToRay(inputProvider.GetPointerPositionInScreenSpace()),
							out RaycastHit hit,
							Mathf.Infinity,
							unitLayerMask,
							QueryTriggerInteraction.Ignore))
						{
							var unit = hit.collider.GetComponentInParent<RosemaryUnit>();
							if (unit)
							{
								found = true;
								ClearSelectedUnits();
								AddUnitToSelection(unit);
								unit.OnSelected();
							}
						}
						if (!found)
						{
							ClearSelectedUnits();
						}
					}
				}
				finally
				{
					isPointerDown = false;
					isBoxSelecting = false;
					OnBoxSelectRectChanged();
				}
			}

			private void OnBoxSelectRectChanged()
			{
				if (boxSelectRectTransform)
				{
					if (isBoxSelecting)
					{
						boxSelectRectTransform.gameObject.SetActive(true);
						boxSelectRectTransform.anchoredPosition = boxSelectRect.min;
						boxSelectRectTransform.sizeDelta = boxSelectRect.size;
					}
					else
					{
						boxSelectRectTransform.gameObject.SetActive(false);
					}
				}
			}

			private void Update()
			{
				if (isPointerDown)
				{
					if (inputProvider != null)
					{
						currentPointerPosition = inputProvider.GetPointerPositionInScreenSpace();
					}
					Vector2 displacement = currentPointerPosition - initialPointerPosition;
					boxSelectRect = new Rect(
						x: Mathf.Min(initialPointerPosition.x, currentPointerPosition.x),
						y: Mathf.Min(initialPointerPosition.y, currentPointerPosition.y),
						width: Mathf.Abs(displacement.x),
						height: Mathf.Abs(displacement.y));

					if (!isBoxSelecting)
					{
						var camRect = cam.rect;
						if (Mathf.Abs(boxSelectRect.width / (camRect.width * Screen.width)) > boxSelectMinSizeInViewport.x
							&& Mathf.Abs(boxSelectRect.height / (camRect.height * Screen.height)) > boxSelectMinSizeInViewport.y)
						{
							isBoxSelecting = true;
						}
					}

					OnBoxSelectRectChanged();
				}

				if (inputProvider != null)
				{
					if (inputProvider.DidButtonGoDown())
					{
						isPointerDown = true;
						isBoxSelecting = false;
						initialPointerPosition = inputProvider.GetPointerPositionInScreenSpace();
						currentPointerPosition = initialPointerPosition;
						boxSelectRect = new Rect(initialPointerPosition, Vector2.zero);
						OnBoxSelectRectChanged();
					}

					if (inputProvider.DidButtonGoUp())
					{
						OnPointerUp();
					}
				}
			}
		}
	}
}
