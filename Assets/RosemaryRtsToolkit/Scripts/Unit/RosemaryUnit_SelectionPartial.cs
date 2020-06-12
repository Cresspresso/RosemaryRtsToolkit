using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cresspresso.Rosemary
{
	public partial class RosemaryUnit
	{
		[Header("Selection")]
		public GameObject controllableSelectionIndicator;
		public GameObject uncontrollableSelectionIndicator;

		public bool isSelected { get; private set; }

		private void OnEnableSelectionPartial()
		{
			OnDeselected();
		}

		private void OnDisableSelectionPartial()
		{
			var selection = RosemarySelection.instanceOrNull;
			if (selection) { selection.Remove(this); }
		}

		public void OnSelected(RosemarySelection selection)
		{
			isSelected = true;

			if (selection.faction == this.faction)
			{
				controllableSelectionIndicator.SetActive(true);
				uncontrollableSelectionIndicator.SetActive(false);
			}
			else
			{
				controllableSelectionIndicator.SetActive(false);
				uncontrollableSelectionIndicator.SetActive(true);
			}
		}

		public void OnDeselected()
		{
			isSelected = false;

			controllableSelectionIndicator.SetActive(false);
			uncontrollableSelectionIndicator.SetActive(false);
		}
	}
}
