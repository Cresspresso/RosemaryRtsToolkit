using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryPlayerCommander : MonoBehaviour
		{
			public class InputProvider : IRosemaryCommanderInputProvider
			{
				public readonly RosemaryPlayerCommander owner;

				public InputProvider(RosemaryPlayerCommander owner)
				{
					this.owner = owner;
				}

				public bool DidButtonGoDown() => owner ? owner.DidButtonGoDown() : false;
				public bool DidButtonGoUp() => owner ? owner.DidButtonGoUp() : false;
				public Vector2 GetPointerPositionInScreenSpace() => owner ? owner.GetPointerPositionInScreenSpace() : Vector2.zero;
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

			[Header("Button")]
			public string inputButtonName = "Fire2";

			public bool DidButtonGoDown() => Input.GetButtonDown(inputButtonName);
			public bool DidButtonGoUp() => Input.GetButtonUp(inputButtonName);
			public Vector2 GetPointerPositionInScreenSpace() => Input.mousePosition;

#pragma warning disable CS0649
			[Header("Target")]
			[SerializeField]
			private RosemaryCommander m_pawn;
#pragma warning restore CS0649

			private void Awake()
			{
				if (!m_pawn)
				{
					m_pawn = GetComponent<RosemaryCommander>();
					if (!m_pawn)
					{
						m_pawn = FindObjectOfType<RosemaryCommander>();
						if (!m_pawn)
						{
							Debug.LogWarning($"Component of type {nameof(RosemaryCommander)} not found in the scene.", this);
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
