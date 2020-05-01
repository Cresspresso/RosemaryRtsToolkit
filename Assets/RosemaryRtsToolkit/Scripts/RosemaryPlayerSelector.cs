using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso
{
	namespace Rosemary
	{
		public class RosemaryPlayerSelector : MonoBehaviour
		{
			public class InputProvider : IRosemarySelectorInputProvider
			{
				public readonly RosemaryPlayerSelector owner;

				public InputProvider(RosemaryPlayerSelector owner)
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
			public string inputButtonName = "Fire1";

			public bool DidButtonGoDown() => Input.GetButtonDown(inputButtonName);
			public bool DidButtonGoUp() => Input.GetButtonUp(inputButtonName);
			public Vector2 GetPointerPositionInScreenSpace() => Input.mousePosition;

#pragma warning disable CS0649
			[Header("Target")]
			[SerializeField]
			private RosemarySelector m_pawn;
#pragma warning restore CS0649

			private void Awake()
			{
				if (!m_pawn)
				{
					m_pawn = GetComponent<RosemarySelector>();
					if (!m_pawn)
					{
						m_pawn = FindObjectOfType<RosemarySelector>();
						if (!m_pawn)
						{
							Debug.LogWarning($"Component of type {nameof(RosemarySelector)} not found in the scene.", this);
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
