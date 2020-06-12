using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	[RequireComponent(typeof(RosemarySpawner))]
	public sealed class RosemaryCommandEffectSpawner : MonoBehaviour
	{
		private RosemarySpawner m_spawner;
		public RosemarySpawner spawner { get { PrepareSpawner(); return m_spawner; } }
		private void PrepareSpawner()
		{
			if (!m_spawner)
			{
				m_spawner = GetComponent<RosemarySpawner>();
				if (!m_spawner)
				{
					Debug.LogError("Spawner is null", this);
				}
			}
		}

		public TargetPoint target { get; private set; } = TargetPoint.None;

		public void Spawn(Vector3 position, Quaternion rotation, TargetPoint target)
		{
			var oldTarget = this.target;
			this.target = target;
			try
			{
				spawner.Spawn(position, rotation);
			}
			finally
			{
				this.target = oldTarget;
			}
		}
	}
}
