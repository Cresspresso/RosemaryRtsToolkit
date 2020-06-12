#if false
using Cresspresso.Rosemary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.ExampleProject
{
	public class ExampleUnitSpawnEventArgs : ISpawnEventArgs
	{
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
		public IFaction faction { get; set; }

		public ExampleUnitSpawnEventArgs(Vector3 position, Quaternion rotation, IFaction faction)
		{
			this.position = position;
			this.rotation = rotation;
			this.faction = faction;
		}
	}
}
#endif