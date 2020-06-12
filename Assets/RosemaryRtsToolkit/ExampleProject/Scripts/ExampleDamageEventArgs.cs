#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cresspresso.Rosemary;

namespace Cresspresso
{
	namespace ExampleProject
	{
		public class ExampleDamageEventArgs : IDamageEventArgs
		{
			public IDamager sender { get; private set; }
			public float damage { get; private set; }
			public bool heal { get; private set; }

			public ExampleDamageEventArgs(
				IDamager sender,
				float damage,
				bool heal = false)
			{
				this.sender = sender;
				this.damage = damage;
				this.heal = heal;
			}
		}
	}
}
#endif