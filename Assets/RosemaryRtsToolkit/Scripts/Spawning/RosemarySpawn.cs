using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			An object which is owned by a <see cref="RosemarySpawner"/>.
	///		</para>
	///		<para>
	///			Must be on a child transform of the <see cref="RosemarySpawner"/> transform.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public sealed class RosemarySpawn : MonoBehaviour
	{
		private RosemarySpawner m_spawner;
		public RosemarySpawner spawner {
			get
			{
				PrepareSpawner();
				return m_spawner;
			}
		}
		public bool isInitialised { get; private set; } = false;

		private void PrepareSpawner()
		{
			if (!m_spawner)
			{
				m_spawner = GetComponentInParent<RosemarySpawner>();
				if (!m_spawner)
				{
					Debug.LogError("Spawn is not a child of a Spawner", this);
				}
				else
				{
					isInitialised = true;
					m_spawner.OnSpawnAwake(this);
				}
			}
		}

		private void Awake()
		{
			PrepareSpawner();
		}

		private void OnEnable()
		{
			spawner.OnSpawnEnable(this);
		}

		private void OnDisable()
		{
			if (spawner) spawner.OnSpawnDisable(this);
		}

		private void OnDestroy()
		{
			if (spawner) spawner.OnSpawnDestroy(this);
		}

		public void Despawn()
		{
			gameObject.SetActive(false);
		}
	}
}
