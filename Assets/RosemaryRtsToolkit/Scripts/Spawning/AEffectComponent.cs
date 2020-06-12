using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Despawns a <see cref="RosemarySpawn"/> object when the method <see cref="IsReadyToDespawn"/> returns <see langword="true"/>.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="17/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	[RequireComponent(typeof(RosemarySpawn))]
	public abstract class AEffectComponent : MonoBehaviour
	{
		private RosemarySpawn m_spawn;
		public RosemarySpawn spawn {
			get
			{
				PrepareSpawn();
				return m_spawn;
			}
		}
		private void PrepareSpawn()
		{
			if (!m_spawn)
			{
				m_spawn = GetComponent<RosemarySpawn>();
				if (!m_spawn)
				{
					Debug.LogError($"{nameof(RosemarySpawn)} component not found on GameObject.", this);
				}
			}
		}

		protected virtual void Awake()
		{
			PrepareSpawn();
		}

		public abstract bool IsReadyToDespawn();

		protected virtual void Update()
		{
			if (IsReadyToDespawn())
			{
				spawn.Despawn();
			}
		}
	}
}
