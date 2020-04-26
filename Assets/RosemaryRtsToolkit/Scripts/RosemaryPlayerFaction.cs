using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryPlayerFaction : MonoBehaviour
		{
			public bool useMainCamera = true;
			public Camera m_camera;
			public Camera cam => useMainCamera ? Camera.main : m_camera;
			public LayerMask unitLayerMask = ~0;
			public LayerMask terrainLayerMask = ~0;
			public List<RosemaryUnit> selectedUnits = new List<RosemaryUnit>();
			private bool isPointerDown = false;
			public bool isBoxSelecting = false;
			private Vector2 initialPointerPosition;
			private Vector2 currentPointerPosition;
			private Rect boxSelectRect;
			public Vector2 boxSelectMinSizeInViewport = new Vector2(0.01f, 0.01f);
			public RectTransform boxSelectRectTransform;

			private void Awake()
			{
				isPointerDown = false;
				isBoxSelecting = false;
				OnBoxSelectRectChanged();
			}

			private void Clear()
			{
				foreach (var unit in selectedUnits)
				{
					unit.OnDeselected();
				}
				selectedUnits.Clear();
			}

			private void Add(RosemaryUnit unit)
			{
				selectedUnits.Add(unit);
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
						Clear();
						// TODO optimise
						foreach (var unit in
							from unit in FindObjectsOfType<RosemaryUnit>()
							let screenPosition = camera.WorldToScreenPoint(unit.transform.position)
							where screenPosition.z >= 0 && boxSelectRect.Contains(screenPosition)
							select unit)
						{
							Add(unit);
						}
					}
					else
					{
						// Just a pointer tap therefore Single Select.
						bool found = false;
						if (Physics.Raycast(
							camera.ScreenPointToRay(Input.mousePosition),
							out RaycastHit hit,
							Mathf.Infinity,
							unitLayerMask,
							QueryTriggerInteraction.Ignore))
						{
							var unit = hit.collider.GetComponentInParent<RosemaryUnit>();
							if (unit)
							{
								found = true;
								Clear();
								Add(unit);
								unit.OnSelected();
							}
						}
						if (!found)
						{
							Clear();
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
					currentPointerPosition = Input.mousePosition;
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

				if (Input.GetButtonDown("Fire1"))
				{
					isPointerDown = true;
					isBoxSelecting = false;
					initialPointerPosition = Input.mousePosition;
					currentPointerPosition = initialPointerPosition;
					boxSelectRect = new Rect(initialPointerPosition, Vector2.zero);
					OnBoxSelectRectChanged();
				}

				if (Input.GetButtonUp("Fire1"))
				{
					OnPointerUp();
				}

				if (Input.GetButtonDown("Fire2"))
				{
					if (Physics.Raycast(
						cam.ScreenPointToRay(Input.mousePosition),
						out RaycastHit hit,
						Mathf.Infinity,
						terrainLayerMask,
						QueryTriggerInteraction.Ignore))
					{
						var destination = hit.point;
						foreach (var unit in selectedUnits)
						{
							unit.destination = destination;
						}
					}
				}
			}
		}
	}
}
