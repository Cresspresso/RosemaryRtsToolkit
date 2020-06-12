using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public enum SpawnMode { PreExisting, Reserving, Spawning }

	/// <summary>
	///		<para>
	///			Manages a set of pre-allocated <see cref="RosemarySpawn"/> GameObjects to be activated and re-used at runtime.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemarySpawner : MonoBehaviour
	{
#pragma warning disable CS0649
		[SerializeField]
		private RosemarySpawn m_prefab;
#pragma warning restore CS0649
		public RosemarySpawn prefab => m_prefab;

#if UNITY_EDITOR
		public void InternalSetPrefab(RosemarySpawn value) => m_prefab = value;
#endif

#pragma warning disable CS0649
		[SerializeField]
		private int m_initialCapacity = 5;
#pragma warning restore CS0649

		private HashSet<RosemarySpawn> m_inactive = new HashSet<RosemarySpawn>();
		private HashSet<RosemarySpawn> m_active = new HashSet<RosemarySpawn>();

		public HashSet<RosemarySpawn> inactive => new HashSet<RosemarySpawn>(m_inactive);
		public HashSet<RosemarySpawn> active => new HashSet<RosemarySpawn>(m_active);

		public int count => m_active.Count;
		public int capacity => m_inactive.Count + m_active.Count;

		public SpawnMode spawnMode { get; private set; } = SpawnMode.PreExisting;

		public void OnSpawnAwake(RosemarySpawn spawn)
		{
			switch (spawnMode)
			{
				case SpawnMode.Reserving:
					{
						m_inactive.Add(spawn);
						spawn.gameObject.SetActive(false);
					}
					break;

				default:
				case SpawnMode.Spawning:
				case SpawnMode.PreExisting:
					{
						m_active.Add(spawn);
					}
					break;
			}
			OnSpawnAwakeExtra(spawn);
		}
		protected virtual void OnSpawnAwakeExtra(RosemarySpawn spawn) { }

		public void OnSpawnEnable(RosemarySpawn spawn)
		{
			m_active.Add(spawn);
			m_inactive.Remove(spawn);
			OnSpawnEnableExtra(spawn);
		}
		protected virtual void OnSpawnEnableExtra(RosemarySpawn spawn) { }

		public void OnSpawnDisable(RosemarySpawn spawn)
		{
			if (spawn)
			{
				m_inactive.Add(spawn);
				m_active.Remove(spawn);
			}
			OnSpawnDisableExtra(spawn);
		}
		protected virtual void OnSpawnDisableExtra(RosemarySpawn spawn) { }

		public void OnSpawnDestroy(RosemarySpawn spawn)
		{
			if (spawn)
			{
				m_inactive.Remove(spawn);
				m_active.Remove(spawn);
			}
			OnSpawnDestoryExtra(spawn);
		}
		protected virtual void OnSpawnDestoryExtra(RosemarySpawn spawn) { }

		/// <summary>
		/// This event must occur after the <see cref="RosemarySpawn.Awake"/> event of all child <see cref="RosemarySpawn"/> GameObjects when the scene starts.
		/// </summary>
		protected virtual void Start()
		{
			Reserve(m_initialCapacity);
		}

		public void Reserve(int capacity)
		{
			if (!isActiveAndEnabled)
			{
				Debug.LogError("Spawner not active and enabled", this);
				return;
			}

			if (prefab == null)
			{
				Debug.LogError($"Prefab is null", this);
				return;
			}

			for (int i = this.capacity; i < capacity; ++i)
			{
				InstantiateSpawnFromPrefab(Vector3.zero, Quaternion.identity, SpawnMode.Reserving);
			}
		}

		public void Trim()
		{
			while (m_inactive.Any())
			{
				var spawn = m_inactive.First();
				if (spawn)
				{
					m_inactive.Remove(spawn);
					Destroy(spawn.gameObject);
				}
			}
		}

		private RosemarySpawn InstantiateSpawnFromPrefab(Vector3 position, Quaternion rotation, SpawnMode mode)
		{
			var oldMode = spawnMode;
			spawnMode = mode;
			try
			{
				if (prefab == null)
				{
					Debug.LogError("Prefab is null", this);
					return null;
				}
				var go = Instantiate(prefab.gameObject, position, rotation, transform);
				go.name = prefab.name;
				var spawn = go.GetComponent<RosemarySpawn>();
				if (!spawn.isInitialised)
				{
					go.SetActive(true);
				}
				return spawn;
			}
			finally
			{
				spawnMode = oldMode;
			}
		}

		public RosemarySpawn Spawn(Vector3 position, Quaternion rotation)
		{
			if (!isActiveAndEnabled)
			{
				Debug.LogError("Spawner not active and enabled", this);
				return null;
			}
			if (!m_inactive.Any())
			{
				return InstantiateSpawnFromPrefab(position, rotation, SpawnMode.Spawning);
			}
			else
			{
				var oldMode = spawnMode;
				spawnMode = SpawnMode.Spawning;
				try
				{
					var spawn = m_inactive.First();
					spawn.transform.position = position;
					spawn.transform.rotation = rotation;
					spawn.gameObject.SetActive(true);
					return spawn;
				}
				finally
				{
					spawnMode = oldMode;
				}
			}
		}

		public void Despawn(RosemarySpawn spawn)
		{
			if (spawn)
			{
				spawn.gameObject.SetActive(false);
			}
		}
	}
}
