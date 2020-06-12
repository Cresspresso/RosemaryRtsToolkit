using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Adds and removes units from the <see cref="RosemarySelection"/> set of selected units,
	///			by responding to player pointer input.
	///		</para>
	/// </summary>
	/// <remarks>
	///		<para>
	///			This component should be attached to a <see cref="RectTransform"/>
	///			with an <see cref="Image"/> component with a transparent color.
	///		</para>
	/// </remarks>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryPlayerSelection : MonoBehaviour,
		IPointerDownHandler,
		IPointerUpHandler,
		IBeginDragHandler,
		IEndDragHandler,
		IDragHandler
	{
		public RectTransform boxSelectRectTransform;
		public LayerMask unitLayerMask = ~0;
		public LayerMask terrainLayerMask = ~0;
		public bool isMultiSelecting = false;
		public bool useLeftShiftForMultiSelecting = true;

		private bool isPointerDown;
		private bool isBoxSelecting;
		private Rect boxSelectRect;

		public RosemarySelection selection => RosemarySelection.instance;
		public Camera cam => Camera.main;

		public static RosemaryPlayerSelection instance => Singleton<RosemaryPlayerSelection>.instance;
		public static RosemaryPlayerSelection instanceOrNull => Singleton<RosemaryPlayerSelection>.instanceOrNull;

		protected virtual void Awake()
		{
			Singleton<RosemaryPlayerSelection>.OnAwake(this);
		}

		private void OnEnable()
		{
			isPointerDown = false;
			isBoxSelecting = false;
			boxSelectRectTransform.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			isPointerDown = false;
			isBoxSelecting = false;
			boxSelectRectTransform.gameObject.SetActive(false);
		}

		protected virtual void Update()
		{
			if (useLeftShiftForMultiSelecting)
			{
				isMultiSelecting = Input.GetKey(KeyCode.LeftShift);
			}

			if (Input.GetKeyDown(KeyCode.Delete))
			{
				selection.CommandDestroy();
			}
		}

		protected virtual IEnumerable<RosemaryUnit> GetUnitsInScreenRect(Camera camera, Rect rect)
		{
			return
				from unit in FindObjectsOfType<RosemaryUnit>()
				let point = Camera.main.WorldToScreenPoint(unit.centre)
				where rect.Contains(point)
				select unit;
		}

		protected virtual bool IsUnitMultiSelectable(RosemaryUnit unit)
		{
			return unit.faction == selection.faction;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left) { return; }

			isPointerDown = true;
			isBoxSelecting = false;
			boxSelectRect = new Rect(eventData.position, Vector2.zero);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			var camera = this.cam;
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					{
						try
						{
							if (isBoxSelecting)
							{
								// Complete Box Selection.
								var units = GetUnitsInScreenRect(camera, boxSelectRect)
									.Where(IsUnitMultiSelectable);
								if (isMultiSelecting)
								{
									if (units.All(unit => selection.Contains(unit)))
									{
										foreach (var unit in units)
										{
											selection.Remove(unit);
										}
									}
									else
									{
										foreach (var unit in units)
										{
											selection.Add(unit);
										}
									}
								}
								else
								{
									selection.Clear();
									foreach (var unit in units)
									{
										selection.Add(unit);
									}
								}
							}
							else
							{
								// Just a click.
								// Complete Single Selection.
								RosemaryUnit unit = null;
								if (Physics.Raycast(
									camera.ScreenPointToRay(eventData.position),
									out RaycastHit hit,
									Mathf.Infinity,
									unitLayerMask,
									QueryTriggerInteraction.Collide))
								{
									unit = hit.collider.GetComponentInParent<RosemaryUnit>();
								}
								if (unit)
								{
									if (isMultiSelecting)
									{
										selection.RemoveAll(u => !IsUnitMultiSelectable(u));
										if (IsUnitMultiSelectable(unit))
										{
											// if contains then remove else add.
											if (!selection.Remove(unit))
											{
												selection.Add(unit);
											}
										}
										// else do not modify selection.
									}
									else
									{
										selection.Clear();
										selection.Add(unit);
									}
								}
								else
								{
									if (!isMultiSelecting)
									{
										selection.Clear();
									}
								}
							}
						}
						finally
						{
							isPointerDown = false;
							isBoxSelecting = false;
						}
					}
					break;
				case PointerEventData.InputButton.Right:
					{
						if (Physics.Raycast(
							camera.ScreenPointToRay(eventData.position),
							out RaycastHit hit,
							Mathf.Infinity,
							terrainLayerMask | unitLayerMask,
							QueryTriggerInteraction.Ignore))
						{
							var unit = hit.collider.GetComponentInParent<RosemaryUnit>();
							if (unit)
							{
								if (unit.faction.IsAlly(selection.faction))
								{
									selection.CommandFollow(unit);
								}
								else
								{
									selection.CommandAttack(unit);
								}
							}
							else
							{
								selection.CommandMove(hit.point, effectPoint: hit.point + hit.normal * 0.5f);
							}
						}
					}
					break;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			switch(eventData.button)
			{
				case PointerEventData.InputButton.Left:
					isBoxSelecting = true;
					boxSelectRectTransform.gameObject.SetActive(true);
					UpdateBoxSelectRect(eventData, false);
					break;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					boxSelectRectTransform.gameObject.SetActive(false);
					UpdateBoxSelectRect(eventData, false);
					break;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			switch(eventData.button)
			{
				case PointerEventData.InputButton.Left:
					if (!isPointerDown) { break; }

					UpdateBoxSelectRect(eventData, true);

					break;
			}
		}

		private void UpdateBoxSelectRect(PointerEventData eventData, bool updateRectTransform)
		{
			Vector2 initial = eventData.pressPosition;
			Vector2 current = eventData.position;
			Vector2 displacement = current - initial;
			boxSelectRect = new Rect(
				x: Mathf.Min(initial.x, current.x),
				y: Mathf.Min(initial.y, current.y),
				width: Mathf.Abs(displacement.x),
				height: Mathf.Abs(displacement.y));
			if (updateRectTransform)
			{
				boxSelectRectTransform.anchoredPosition = boxSelectRect.min;
				boxSelectRectTransform.sizeDelta = boxSelectRect.size;
			}
		}
	}
}
