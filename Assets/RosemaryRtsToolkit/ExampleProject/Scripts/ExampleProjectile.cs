#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cresspresso.Rosemary;

namespace Cresspresso.ExampleProject
{
	public class ExampleProjectile : MonoBehaviour, IProjectile
	{
		public ISpawner spawner { get; set; }
		public ITarget target { get; private set; }
		public float damage { get; private set; }

		public void OnSpawned(ISpawnEventArgs eventArgs)
		{
			var ourArgs = (IProjectileSpawnEventArgs)eventArgs;
			var ourWeapon = (ExampleWeapon)ourArgs.weapon;
			target = ourArgs.target;
			damage = ourWeapon.damage;

			transform.position = eventArgs.position;
			transform.rotation = eventArgs.rotation;
			gameObject.SetActive(true);
		}

		public void OnDespawned()
		{
			gameObject.SetActive(false);
		}
	}
}
#endif