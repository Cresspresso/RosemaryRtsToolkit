using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Sends player input to a <see cref="RosemaryCameraPivot"/> to control the camera view.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryPlayerCameraPivot : MonoBehaviour
	{
		/// <changelog>
		///		<log date="01/05/2020"
		///			version="1.0"
		///			author="Elijah Shadbolt">
		///			Initial version.
		///		</log>
		/// </changelog>
		public class InputProvider : IRosemaryCameraPivotInputProvider
		{
			public readonly RosemaryPlayerCameraPivot owner;
			public InputProvider(RosemaryPlayerCameraPivot owner) { this.owner = owner; }
			public bool active => owner && owner.isActiveAndEnabled;

			public float GetBoomInput() => active ? owner.GetBoomInput() : 0;
			public float GetForwardsInput() => active ? owner.GetForwardsInput() : 0;
			public float GetSidewaysInput() => active ? owner.GetSidewaysInput() : 0;
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

		/// <changelog>
		///		<log date="01/05/2020"
		///			version="1.0"
		///			author="Elijah Shadbolt">
		///			Initial version.
		///		</log>
		/// </changelog>
		[System.Serializable]
		public struct InputAxisInfo
		{
			public string inputAxisName;
			public float sensitivity;
			public bool invert;

			public InputAxisInfo(string inputAxisName, float sensitivity = 1.0f, bool invert = false)
			{
				this.inputAxisName = inputAxisName;
				this.sensitivity = sensitivity;
				this.invert = invert;
			}

			public float GetInput() => Input.GetAxis(inputAxisName) * (invert ? -sensitivity : sensitivity);
		}

		[SerializeField]
		private RosemaryCameraPivot m_pawn;
		public RosemaryCameraPivot pawn {
			get => m_pawn;
			set
			{
				if (m_pawn && m_pawn.inputProvider == inputProvider)
				{
					m_pawn.inputProvider = null;
				}

				m_pawn = value;

				if (m_pawn)
				{
					m_pawn.inputProvider = inputProvider;
				}
			}
		}

		public InputAxisInfo sidewaysInput = new InputAxisInfo("Horizontal", 10.0f);
		public InputAxisInfo forwardsInput = new InputAxisInfo("Vertical", 10.0f);
		public InputAxisInfo boomInput = new InputAxisInfo("Mouse ScrollWheel", 0.5f, true);
		public KeyCode speedKey = KeyCode.LeftShift;
		public float speedMultiplier = 5.0f;

		public float GetSpeedMultiplier() => Input.GetKey(speedKey) ? speedMultiplier : 1.0f;
		public float GetSidewaysInput() => sidewaysInput.GetInput() * GetSpeedMultiplier();
		public float GetForwardsInput() => forwardsInput.GetInput() * GetSpeedMultiplier();
		public float GetBoomInput() => boomInput.GetInput() * GetSpeedMultiplier();

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
						enabled = false;
					}
				}
			}
			if (m_pawn)
			{
				m_pawn.inputProvider = inputProvider;
			}
		}
	}
}
