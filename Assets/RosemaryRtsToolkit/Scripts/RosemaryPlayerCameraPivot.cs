using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cresspresso
{
	namespace Rosemary
	{
		/// <summary>
		/// Top-down RTS camera.
		/// </summary>
		public class RosemaryPlayerCameraPivot : MonoBehaviour
		{
			public class InputProvider : IRosemaryCameraPivotInputProvider
			{
				public readonly RosemaryPlayerCameraPivot owner;

				public InputProvider(RosemaryPlayerCameraPivot owner)
				{
					this.owner = owner;
				}

				public float GetBoomInput() => owner ? owner.GetBoomInput() : 0;
				public float GetForwardsInput() => owner ? owner.GetForwardsInput() : 0;
				public float GetSidewaysInput() => owner ? owner.GetSidewaysInput() : 0;
			}

			private InputProvider m_inputProvider;
			public InputProvider inputProvider {
				get
				{
					if (m_inputProvider == null)
					{
						m_inputProvider = new InputProvider(this);
					}
					return m_inputProvider;
				}
			}

			[Header("Sideways")]
			public string inputAxisNameForSideways = "Horizontal";
			public bool invertForSideways = false;
			public float sensitivityForSideways = 10.0f;
			public float GetSidewaysInput() => sensitivityForSideways * Input.GetAxis(inputAxisNameForSideways) * (invertForSideways ? -1 : 1);

			[Header("Forwards")]
			public string inputAxisnameForForwards = "Vertical";
			public bool invertForForwards = false;
			public float sensitivityForForwards = 10.0f;
			public float GetForwardsInput() => sensitivityForForwards * Input.GetAxis(inputAxisnameForForwards) * (invertForForwards ? -1 : 1);

			[Header("Boom")]
			public string inputAxisNameForBoom = "Mouse ScrollWheel";
			public bool invertForBooom = true;
			public float sensitivityForBoom = 0.5f;
			public float GetBoomInput() => sensitivityForBoom * Input.GetAxis(inputAxisNameForBoom) * (invertForBooom ? -1 : 1);

#pragma warning disable CS0649
			[Header("Target")]
			[SerializeField]
			private RosemaryCameraPivot m_pawn;
#pragma warning restore CS0649

			private void Awake()
			{
				if (!m_pawn)
				{
					m_pawn = GetComponent<RosemaryCameraPivot>();
					if (!m_pawn)
					{
						m_pawn = FindObjectOfType<RosemaryCameraPivot>();
						if (!m_pawn)
						{
							Debug.LogWarning($"Component of type {nameof(RosemaryCameraPivot)} not found in the scene.", this);
						}
					}
				}
				if (m_pawn)
				{
					m_pawn.inputProvider = this.inputProvider;
				}
			}
		}
	}
}
